using System.Buffers.Binary;
using System.IO.Pipes;
using System.Text;
using CBus;

namespace CBus.Hosting;

/// <summary>
/// 负责监听命名管道请求并将其分发到已注册服务。
/// </summary>
public sealed class CBusPipeListener : IAsyncDisposable
{
    private const int MaxMessageByteLength = 16 * 1024 * 1024;

    private readonly string _pipeAddress;
    private readonly CBusDispatcher _dispatcher;
    private readonly CBusMessageSerializer _serializer = new();
    private readonly object _syncRoot = new();
    private CancellationTokenSource? _listeningCancellationTokenSource;
    private Task? _listeningTask;

    /// <summary>
    /// 使用指定的管道地址和分发器创建监听器。
    /// </summary>
    public CBusPipeListener(string pipeAddress, CBusDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new ArgumentException("Pipe address cannot be null or whitespace.", nameof(pipeAddress));
        }

        _pipeAddress = pipeAddress;
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
    }

    /// <summary>
    /// 获取当前监听的管道地址。
    /// </summary>
    public string PipeAddress => _pipeAddress;

    /// <summary>
    /// 启动命名管道监听循环。
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock (_syncRoot)
        {
            if (_listeningTask is not null)
            {
                return Task.CompletedTask;
            }

            _listeningCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _listeningTask = RunAsync(_listeningCancellationTokenSource.Token);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 停止命名管道监听循环。
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        CancellationTokenSource? listeningCancellationTokenSource;
        Task? listeningTask;

        lock (_syncRoot)
        {
            listeningCancellationTokenSource = _listeningCancellationTokenSource;
            listeningTask = _listeningTask;
            _listeningCancellationTokenSource = null;
            _listeningTask = null;
        }

        if (listeningCancellationTokenSource is null)
        {
            return;
        }

        listeningCancellationTokenSource.Cancel();

        if (listeningTask is not null)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                await listeningTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (listeningCancellationTokenSource.IsCancellationRequested)
            {
            }
        }

        listeningCancellationTokenSource.Dispose();
    }

    /// <summary>
    /// 释放监听器占用的资源。
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await StopAsync().ConfigureAwait(false);
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var server = CreateServerStream();

            try
            {
                await server.WaitForConnectionAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            await HandleConnectionAsync(server, cancellationToken).ConfigureAwait(false);
        }
    }

    private NamedPipeServerStream CreateServerStream()
    {
        return new NamedPipeServerStream(
            _pipeAddress,
            PipeDirection.InOut,
            NamedPipeServerStream.MaxAllowedServerInstances,
            PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous);
    }

    private async Task HandleConnectionAsync(NamedPipeServerStream server, CancellationToken cancellationToken)
    {
        CBusResponse response;

        try
        {
            var requestContent = await ReadMessageAsync(server, cancellationToken).ConfigureAwait(false);
            var request = _serializer.DeserializeRequest(requestContent);
            var result = await _dispatcher.DispatchAsync(request, cancellationToken).ConfigureAwait(false);
            response = result.Response;
        }
        catch (FormatException exception)
        {
            response = CreateErrorResponse(400, "Bad Request", exception.Message);
        }
        catch (ArgumentException exception)
        {
            response = CreateErrorResponse(400, "Bad Request", exception.Message);
        }
        catch (IOException exception)
        {
            response = CreateErrorResponse(400, "Bad Request", exception.Message);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return;
        }
        catch (InvalidOperationException exception)
        {
            response = CreateErrorResponse(500, "Internal Server Error", exception.Message);
        }

        var responseContent = _serializer.SerializeResponse(response);
        await WriteMessageAsync(server, responseContent, cancellationToken).ConfigureAwait(false);
    }

    private static CBusResponse CreateErrorResponse(int statusCode, string reasonPhrase, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        return new CBusResponse(statusCode, reasonPhrase, new Dictionary<string, string>
        {
            ["Content-Type"] = "text/plain; charset=utf-8"
        }, body);
    }

    private static async Task<string> ReadMessageAsync(PipeStream stream, CancellationToken cancellationToken)
    {
        var lengthBuffer = new byte[sizeof(int)];
        await ReadExactAsync(stream, lengthBuffer, cancellationToken).ConfigureAwait(false);
        var length = BinaryPrimitives.ReadInt32LittleEndian(lengthBuffer);

        if (length < 0 || length > MaxMessageByteLength)
        {
            throw new FormatException("Pipe message length is invalid.");
        }

        var payload = new byte[length];
        await ReadExactAsync(stream, payload, cancellationToken).ConfigureAwait(false);
        return Encoding.UTF8.GetString(payload);
    }

    private static async Task WriteMessageAsync(PipeStream stream, string content, CancellationToken cancellationToken)
    {
        var payload = Encoding.UTF8.GetBytes(content);
        if (payload.Length > MaxMessageByteLength)
        {
            throw new InvalidOperationException("Pipe message is too large.");
        }

        var lengthBuffer = new byte[sizeof(int)];
        BinaryPrimitives.WriteInt32LittleEndian(lengthBuffer, payload.Length);

        await stream.WriteAsync(lengthBuffer, cancellationToken).ConfigureAwait(false);
        await stream.WriteAsync(payload, cancellationToken).ConfigureAwait(false);
        await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task ReadExactAsync(Stream stream, byte[] buffer, CancellationToken cancellationToken)
    {
        var offset = 0;

        while (offset < buffer.Length)
        {
            var bytesRead = await stream.ReadAsync(buffer.AsMemory(offset), cancellationToken).ConfigureAwait(false);
            if (bytesRead == 0)
            {
                throw new IOException("Unexpected end of stream while reading pipe message.");
            }

            offset += bytesRead;
        }
    }
}

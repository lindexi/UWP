using System.Net;
using System.Net.Sockets;
using System.Text;
using CBus;

namespace CBus.Hosting;

/// <summary>
/// 负责监听 HTTP 请求并将其分发到已注册服务。
/// </summary>
public sealed class CBusHttpListener : IAsyncDisposable
{
    private readonly Uri _endpoint;
    private readonly CBusDispatcher _dispatcher;
    private readonly object _syncRoot = new();
    private TcpListener? _listener;
    private CancellationTokenSource? _listeningCancellationTokenSource;
    private Task? _listeningTask;

    /// <summary>
    /// 使用监听终结点和分发器创建 HTTP 监听器。
    /// </summary>
    public CBusHttpListener(Uri endpoint, CBusDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        if (!endpoint.IsAbsoluteUri)
        {
            throw new ArgumentException("Endpoint must be an absolute URI.", nameof(endpoint));
        }

        _endpoint = endpoint;
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
    }

    /// <summary>
    /// 获取当前监听的 HTTP 终结点。
    /// </summary>
    public Uri Endpoint => _endpoint;

    /// <summary>
    /// 启动 HTTP 监听循环。
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        lock (_syncRoot)
        {
            if (_listener is not null)
            {
                return Task.CompletedTask;
            }

            var listener = CreateListener(_endpoint);
            var listeningCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            listener.Start();

            _listener = listener;
            _listeningCancellationTokenSource = listeningCancellationTokenSource;
            _listeningTask = RunAsync(listener, listeningCancellationTokenSource.Token);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 停止 HTTP 监听循环。
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        TcpListener? listener;
        CancellationTokenSource? listeningCancellationTokenSource;
        Task? listeningTask;

        lock (_syncRoot)
        {
            listener = _listener;
            listeningCancellationTokenSource = _listeningCancellationTokenSource;
            listeningTask = _listeningTask;

            _listener = null;
            _listeningCancellationTokenSource = null;
            _listeningTask = null;
        }

        if (listener is null || listeningCancellationTokenSource is null)
        {
            return;
        }

        listeningCancellationTokenSource.Cancel();
        listener.Stop();

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

    private static TcpListener CreateListener(Uri endpoint)
    {
        if (!string.Equals(endpoint.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
        {
            throw new NotSupportedException($"Endpoint scheme '{endpoint.Scheme}' is not supported.");
        }

        var address = endpoint.Host switch
        {
            "localhost" => IPAddress.Loopback,
            _ when IPAddress.TryParse(endpoint.Host, out var ipAddress) => ipAddress,
            _ => throw new NotSupportedException($"Endpoint host '{endpoint.Host}' is not supported.")
        };

        return new TcpListener(address, endpoint.Port);
    }

    private async Task RunAsync(TcpListener listener, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            TcpClient client;

            try
            {
                client = await listener.AcceptTcpClientAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch (ObjectDisposedException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            catch (SocketException) when (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            using (client)
            {
                await HandleClientAsync(client, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        using var stream = client.GetStream();

        CBusResponse response;
        try
        {
            var request = await ReadRequestAsync(stream, cancellationToken).ConfigureAwait(false);
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

        await WriteResponseAsync(stream, response, cancellationToken).ConfigureAwait(false);
    }

    private static CBusResponse CreateErrorResponse(int statusCode, string reasonPhrase, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        return new CBusResponse(statusCode, reasonPhrase, new Dictionary<string, string>
        {
            ["Content-Type"] = "text/plain; charset=utf-8"
        }, body);
    }

    private static async Task<CBusRequest> ReadRequestAsync(NetworkStream stream, CancellationToken cancellationToken)
    {
        var headerBytes = await ReadHeaderBytesAsync(stream, cancellationToken).ConfigureAwait(false);
        var headerText = Encoding.ASCII.GetString(headerBytes.Buffer, 0, headerBytes.HeaderLength);
        var lines = headerText.Split("\r\n", StringSplitOptions.None);

        if (lines.Length == 0 || string.IsNullOrWhiteSpace(lines[0]))
        {
            throw new FormatException("HTTP request line is required.");
        }

        var requestLineParts = lines[0].Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
        if (requestLineParts.Length < 3)
        {
            throw new FormatException("HTTP request line is invalid.");
        }

        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            var separatorIndex = line.IndexOf(':');
            if (separatorIndex <= 0)
            {
                throw new FormatException($"Invalid HTTP header '{line}'.");
            }

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim();
            headers[key] = value;
        }

        var bodyLength = 0;
        if (headers.TryGetValue("Content-Length", out var contentLengthText))
        {
            if (!int.TryParse(contentLengthText, out bodyLength) || bodyLength < 0)
            {
                throw new FormatException("HTTP Content-Length is invalid.");
            }
        }

        var body = new byte[bodyLength];
        var bufferedBodyLength = headerBytes.BufferLength - headerBytes.BodyOffset;
        if (bufferedBodyLength > 0)
        {
            var copyLength = Math.Min(bufferedBodyLength, bodyLength);
            Buffer.BlockCopy(headerBytes.Buffer, headerBytes.BodyOffset, body, 0, copyLength);
        }

        var remainingLength = bodyLength - Math.Min(bufferedBodyLength, bodyLength);
        if (remainingLength > 0)
        {
            await ReadExactAsync(stream, body, bodyLength - remainingLength, remainingLength, cancellationToken).ConfigureAwait(false);
        }

        var path = NormalizePath(requestLineParts[1]);
        return new CBusRequest(requestLineParts[0], path, headers, body);
    }

    private static async Task WriteResponseAsync(NetworkStream stream, CBusResponse response, CancellationToken cancellationToken)
    {
        var headers = new Dictionary<string, string>(response.Headers, StringComparer.OrdinalIgnoreCase)
        {
            ["Content-Length"] = response.Body.Length.ToString(),
            ["Connection"] = "close"
        };

        var builder = new StringBuilder();
        builder.Append("HTTP/1.1 ")
            .Append(response.StatusCode)
            .Append(' ')
            .Append(response.ReasonPhrase)
            .Append("\r\n");

        foreach (var header in headers)
        {
            builder.Append(header.Key)
                .Append(": ")
                .Append(header.Value)
                .Append("\r\n");
        }

        builder.Append("\r\n");

        var headerBytes = Encoding.ASCII.GetBytes(builder.ToString());
        await stream.WriteAsync(headerBytes, cancellationToken).ConfigureAwait(false);
        if (response.Body.Length > 0)
        {
            await stream.WriteAsync(response.Body, cancellationToken).ConfigureAwait(false);
        }

        await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task<(byte[] Buffer, int HeaderLength, int BodyOffset, int BufferLength)> ReadHeaderBytesAsync(NetworkStream stream, CancellationToken cancellationToken)
    {
        using var bufferStream = new MemoryStream();
        var buffer = new byte[1024];
        var headerEndIndex = -1;

        while (headerEndIndex < 0)
        {
            var bytesRead = await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
            if (bytesRead == 0)
            {
                throw new IOException("Unexpected end of stream while reading HTTP headers.");
            }

            await bufferStream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);

            var written = bufferStream.GetBuffer();
            headerEndIndex = IndexOfHeaderSeparator(written.AsSpan(0, (int)bufferStream.Length));
        }

        var content = bufferStream.ToArray();
        return (content, headerEndIndex, headerEndIndex + 4, content.Length);
    }

    private static int IndexOfHeaderSeparator(ReadOnlySpan<byte> buffer)
    {
        for (var index = 0; index <= buffer.Length - 4; index++)
        {
            if (buffer[index] == '\r'
                && buffer[index + 1] == '\n'
                && buffer[index + 2] == '\r'
                && buffer[index + 3] == '\n')
            {
                return index;
            }
        }

        return -1;
    }

    private static async Task ReadExactAsync(NetworkStream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        while (count > 0)
        {
            var bytesRead = await stream.ReadAsync(buffer.AsMemory(offset, count), cancellationToken).ConfigureAwait(false);
            if (bytesRead == 0)
            {
                throw new IOException("Unexpected end of stream while reading HTTP body.");
            }

            offset += bytesRead;
            count -= bytesRead;
        }
    }

    private static string NormalizePath(string requestTarget)
    {
        if (Uri.TryCreate(requestTarget, UriKind.Absolute, out var absoluteUri))
        {
            return absoluteUri.AbsolutePath;
        }

        var querySeparatorIndex = requestTarget.IndexOf('?', StringComparison.Ordinal);
        return querySeparatorIndex >= 0
            ? requestTarget[..querySeparatorIndex]
            : requestTarget;
    }
}

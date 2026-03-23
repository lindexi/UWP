using System.Buffers.Binary;
using System.IO.Pipes;
using System.Text;

namespace CBus;

/// <summary>
/// 使用系统命名管道将请求发送到指定的管道地址。
/// </summary>
public sealed class SystemNamedPipeTransport : ICBusPipeTransport
{
    private const int MaxMessageByteLength = 16 * 1024 * 1024;

    private readonly CBusMessageSerializer _serializer = new();

    /// <summary>
    /// 通过命名管道发送请求并返回响应。
    /// </summary>
    public async Task<CBusResponse> SendAsync(string pipeAddress, CBusRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new ArgumentException("Pipe address cannot be null or whitespace.", nameof(pipeAddress));
        }

        ArgumentNullException.ThrowIfNull(request);

        using var client = new NamedPipeClientStream(".", pipeAddress, PipeDirection.InOut, PipeOptions.Asynchronous);
        await client.ConnectAsync(cancellationToken).ConfigureAwait(false);

        var requestContent = _serializer.SerializeRequest(request);
        await WriteMessageAsync(client, requestContent, cancellationToken).ConfigureAwait(false);

        var responseContent = await ReadMessageAsync(client, cancellationToken).ConfigureAwait(false);
        return _serializer.DeserializeResponse(responseContent);
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

namespace CBus;

public sealed class PipeConnector
{
    private readonly string _pipeAddress;
    private readonly ICBusPipeTransport _transport;
    private readonly CBusMessageSerializer _serializer;

    public PipeConnector(string pipeAddress, ICBusPipeTransport transport)
        : this(pipeAddress, transport, new CBusMessageSerializer())
    {
    }

    public PipeConnector(string pipeAddress, ICBusPipeTransport transport, CBusMessageSerializer serializer)
    {
        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new ArgumentException("Pipe address cannot be null or whitespace.", nameof(pipeAddress));
        }

        _pipeAddress = pipeAddress;
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public string PipeAddress => _pipeAddress;

    /// <summary>
    /// 使用 Pipe 风格连接发送请求。
    /// </summary>
    public Task<CBusResponse> SendAsync(CBusRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _transport.SendAsync(_pipeAddress, request, cancellationToken);
    }

    /// <summary>
    /// 将请求序列化为 Pipe 消息。
    /// </summary>
    public string Serialize(CBusRequest request)
    {
        return _serializer.SerializeRequest(request);
    }

    /// <summary>
    /// 将 Pipe 消息反序列化为请求。
    /// </summary>
    public CBusRequest Deserialize(string content)
    {
        return _serializer.DeserializeRequest(content);
    }

    /// <summary>
    /// 将响应序列化为 Pipe 消息。
    /// </summary>
    public string SerializeResponse(CBusResponse response)
    {
        return _serializer.SerializeResponse(response);
    }

    /// <summary>
    /// 将 Pipe 消息反序列化为响应。
    /// </summary>
    public CBusResponse DeserializeResponse(string content)
    {
        return _serializer.DeserializeResponse(content);
    }
}

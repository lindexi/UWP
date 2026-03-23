namespace CBus;

public sealed class HttpConnector
{
    private readonly Uri _endpoint;
    private readonly ICBusHttpTransport _transport;
    private readonly CBusMessageSerializer _serializer;

    public HttpConnector(Uri endpoint, ICBusHttpTransport transport)
        : this(endpoint, transport, new CBusMessageSerializer())
    {
    }

    public HttpConnector(Uri endpoint, ICBusHttpTransport transport, CBusMessageSerializer serializer)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        if (!endpoint.IsAbsoluteUri)
        {
            throw new ArgumentException("Endpoint must be an absolute URI.", nameof(endpoint));
        }

        _endpoint = endpoint;
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public Uri Endpoint => _endpoint;

    /// <summary>
    /// 使用 HTTP 风格连接发送请求。
    /// </summary>
    public Task<CBusResponse> SendAsync(CBusRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _transport.SendAsync(_endpoint, request, cancellationToken);
    }

    /// <summary>
    /// 将请求序列化为 HTTP 风格消息。
    /// </summary>
    public string Serialize(CBusRequest request)
    {
        return _serializer.SerializeRequest(request);
    }

    /// <summary>
    /// 将 HTTP 风格消息反序列化为请求。
    /// </summary>
    public CBusRequest Deserialize(string content)
    {
        return _serializer.DeserializeRequest(content);
    }

    /// <summary>
    /// 将响应序列化为 HTTP 风格消息。
    /// </summary>
    public string SerializeResponse(CBusResponse response)
    {
        return _serializer.SerializeResponse(response);
    }

    /// <summary>
    /// 将 HTTP 风格消息反序列化为响应。
    /// </summary>
    public CBusResponse DeserializeResponse(string content)
    {
        return _serializer.DeserializeResponse(content);
    }
}

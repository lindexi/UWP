namespace CBus;

public sealed class HttpConnector
{
    private readonly CBusDispatcher _dispatcher;
    private readonly CBusMessageSerializer _serializer;

    public HttpConnector(CBusDispatcher dispatcher)
        : this(dispatcher, new CBusMessageSerializer())
    {
    }

    public HttpConnector(CBusDispatcher dispatcher, CBusMessageSerializer serializer)
    {
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    /// <summary>
    /// 使用 HTTP 风格连接发送请求。
    /// </summary>
    public async Task<CBusResponse> SendAsync(CBusRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var result = await _dispatcher.DispatchAsync(request, cancellationToken).ConfigureAwait(false);
        return result.Response;
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

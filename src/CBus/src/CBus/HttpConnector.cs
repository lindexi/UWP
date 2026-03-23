namespace CBus;

/// <summary>
/// 提供面向 HTTP 终结点的客户端调用入口。
/// </summary>
public sealed class HttpConnector
{
    private readonly Uri _endpoint;
    private readonly ICBusHttpTransport _transport;

    /// <summary>
    /// 使用目标终结点和 HTTP 传输实现创建连接器。
    /// </summary>
    public HttpConnector(Uri endpoint, ICBusHttpTransport transport)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        if (!endpoint.IsAbsoluteUri)
        {
            throw new ArgumentException("Endpoint must be an absolute URI.", nameof(endpoint));
        }

        _endpoint = endpoint;
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

    /// <summary>
    /// 获取目标 HTTP 终结点。
    /// </summary>
    public Uri Endpoint => _endpoint;

    /// <summary>
    /// 使用 HTTP 风格连接发送请求。
    /// </summary>
    public Task<CBusResponse> SendAsync(CBusRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return _transport.SendAsync(_endpoint, request, cancellationToken);
    }
}

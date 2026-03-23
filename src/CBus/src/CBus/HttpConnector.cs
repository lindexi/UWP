namespace CBus;

public sealed class HttpConnector
{
    private readonly Uri _endpoint;
    private readonly ICBusHttpTransport _transport;

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

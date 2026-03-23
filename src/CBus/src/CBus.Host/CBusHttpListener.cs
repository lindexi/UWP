using CBus;

namespace CBus.Hosting;

public sealed class CBusHttpListener : ICBusHttpTransport
{
    private readonly Uri _endpoint;
    private readonly CBusDispatcher _dispatcher;

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

    public Uri Endpoint => _endpoint;

    /// <summary>
    /// 处理发往监听 HTTP 地址的请求。
    /// </summary>
    public async Task<CBusResponse> SendAsync(Uri endpoint, CBusRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        ArgumentNullException.ThrowIfNull(request);

        if (_endpoint != endpoint)
        {
            throw new InvalidOperationException($"Listener endpoint '{_endpoint}' does not match '{endpoint}'.");
        }

        var result = await _dispatcher.DispatchAsync(request, cancellationToken).ConfigureAwait(false);
        return result.Response;
    }
}

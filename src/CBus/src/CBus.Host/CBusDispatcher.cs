using CBus;

namespace CBus.Hosting;

public sealed class CBusDispatcher
{
    private readonly CBusServiceRegistry _serviceRegistry;

    public CBusDispatcher(CBusServiceRegistry serviceRegistry)
    {
        _serviceRegistry = serviceRegistry ?? throw new ArgumentNullException(nameof(serviceRegistry));
    }

    /// <summary>
    /// 根据请求路径将请求分发到已注册服务。
    /// </summary>
    public async Task<CBusDispatchResult> DispatchAsync(CBusRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!_serviceRegistry.TryGetRegisteredServiceByPath(request.Path, out var service) || service is null)
        {
            return CBusDispatchResult.NotFound(CBusResponse.NotFound($"No service registered for path '{request.Path}'."));
        }

        var response = await service.Handler(request, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Registered service returned a null response.");

        return CBusDispatchResult.Success(service.Registration.ServiceName, response);
    }
}

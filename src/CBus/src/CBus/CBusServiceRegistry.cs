namespace CBus;

public sealed class CBusServiceRegistry
{
    private readonly Dictionary<string, RegisteredService> _servicesByRoot = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<CBusServiceRegistration> _registrations = [];

    /// <summary>
    /// 注册一个服务及其处理逻辑。
    /// </summary>
    public void Register(CBusServiceRegistration registration, Func<CBusRequest, CancellationToken, Task<CBusResponse>> handler)
    {
        ArgumentNullException.ThrowIfNull(registration);
        ArgumentNullException.ThrowIfNull(handler);

        var routeRoots = registration.Routes
            .Select(static route => route.RouteRoot)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        foreach (var routeRoot in routeRoots)
        {
            if (_servicesByRoot.TryGetValue(routeRoot, out var existingService))
            {
                throw new InvalidOperationException($"Route root '{routeRoot}' is already registered by service '{existingService.Registration.ServiceName}'.");
            }
        }

        var registeredService = new RegisteredService(registration, handler);
        foreach (var routeRoot in routeRoots)
        {
            _servicesByRoot[routeRoot] = registeredService;
        }

        _registrations.Add(registration);
    }

    /// <summary>
    /// 按请求路径查找服务注册信息。
    /// </summary>
    public bool TryGetRegistrationByPath(string path, out CBusServiceRegistration? registration)
    {
        if (TryGetRegisteredServiceByPath(path, out var service))
        {
            registration = service.Registration;
            return true;
        }

        registration = null;
        return false;
    }

    /// <summary>
    /// 获取当前所有注册的服务。
    /// </summary>
    public IReadOnlyCollection<CBusServiceRegistration> GetRegistrations()
    {
        return _registrations.AsReadOnly();
    }

    internal bool TryGetRegisteredServiceByPath(string path, out RegisteredService? service)
    {
        var routeRoot = CBusRouteDefinition.GetRouteRoot(path);
        if (_servicesByRoot.TryGetValue(routeRoot, out var registeredService))
        {
            service = registeredService;
            return true;
        }

        service = null;
        return false;
    }

    internal sealed class RegisteredService
    {
        public RegisteredService(CBusServiceRegistration registration, Func<CBusRequest, CancellationToken, Task<CBusResponse>> handler)
        {
            Registration = registration;
            Handler = handler;
        }

        public CBusServiceRegistration Registration { get; }

        public Func<CBusRequest, CancellationToken, Task<CBusResponse>> Handler { get; }
    }
}

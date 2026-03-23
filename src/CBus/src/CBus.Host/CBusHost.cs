using CBus;

namespace CBus.Hosting;

public sealed class CBusHost
{
    private readonly CBusServiceRegistry _serviceRegistry;
    private readonly CBusDispatcher _dispatcher;
    private readonly CBusAddressPublisher _addressPublisher;

    public CBusHost(CBusHostOptions options, ICBusRegistryStore registryStore, ICBusFileStore fileStore)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        _serviceRegistry = new CBusServiceRegistry();
        _dispatcher = new CBusDispatcher(_serviceRegistry);
        _addressPublisher = new CBusAddressPublisher(registryStore, fileStore);
        HttpListener = new CBusHttpListener(new Uri($"http://127.0.0.1:{options.HttpPort}/", UriKind.Absolute), _dispatcher);
        PipeListener = new CBusPipeListener(options.PipeAddress, _dispatcher);
    }

    public CBusHostOptions Options { get; }

    public CBusHttpListener HttpListener { get; }

    public CBusPipeListener PipeListener { get; }

    /// <summary>
    /// 注册一个服务处理程序。
    /// </summary>
    public void RegisterService(CBusServiceRegistration registration, Func<CBusRequest, CancellationToken, Task<CBusResponse>> handler)
    {
        _serviceRegistry.Register(registration, handler);
    }

    /// <summary>
    /// 启动宿主并发布监听地址。
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return _addressPublisher.PublishAsync(Options, cancellationToken);
    }

    /// <summary>
    /// 创建与当前宿主匹配的发现配置。
    /// </summary>
    public CBusDiscoveryOptions CreateDiscoveryOptions()
    {
        return new CBusDiscoveryOptions(Options.PublishDirectory, Options.RegistrySubKey);
    }
}

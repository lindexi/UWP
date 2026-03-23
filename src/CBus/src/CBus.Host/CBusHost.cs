using CBus;

namespace CBus.Hosting;

/// <summary>
/// 表示承载 CBus 服务注册、监听和地址发布的宿主。
/// </summary>
public sealed class CBusHost : IAsyncDisposable
{
    private readonly CBusServiceRegistry _serviceRegistry;
    private readonly CBusDispatcher _dispatcher;
    private readonly CBusAddressPublisher _addressPublisher;
    private bool _isStarted;

    /// <summary>
    /// 使用宿主配置和发现存储实现创建宿主。
    /// </summary>
    public CBusHost(CBusHostOptions options, ICBusRegistryStore registryStore, ICBusFileStore fileStore)
    {
        Options = options ?? throw new ArgumentNullException(nameof(options));
        _serviceRegistry = new CBusServiceRegistry();
        _dispatcher = new CBusDispatcher(_serviceRegistry);
        _addressPublisher = new CBusAddressPublisher(registryStore, fileStore);
        HttpListener = new CBusHttpListener(new Uri($"http://127.0.0.1:{options.HttpPort}/", UriKind.Absolute), _dispatcher);
        PipeListener = new CBusPipeListener(options.PipeAddress, _dispatcher);
    }

    /// <summary>
    /// 获取宿主配置。
    /// </summary>
    public CBusHostOptions Options { get; }

    /// <summary>
    /// 获取宿主使用的 HTTP 监听器。
    /// </summary>
    public CBusHttpListener HttpListener { get; }

    /// <summary>
    /// 获取宿主使用的命名管道监听器。
    /// </summary>
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
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (_isStarted)
        {
            return;
        }

        await HttpListener.StartAsync(cancellationToken).ConfigureAwait(false);
        await PipeListener.StartAsync(cancellationToken).ConfigureAwait(false);

        var isStarted = false;
        try
        {
            await _addressPublisher.PublishAsync(Options, cancellationToken).ConfigureAwait(false);
            _isStarted = true;
            isStarted = true;
        }
        finally
        {
            if (!isStarted)
            {
                await PipeListener.StopAsync(CancellationToken.None).ConfigureAwait(false);
                await HttpListener.StopAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 停止宿主监听。
    /// </summary>
    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        await PipeListener.StopAsync(cancellationToken).ConfigureAwait(false);
        await HttpListener.StopAsync(cancellationToken).ConfigureAwait(false);
        _isStarted = false;
    }

    /// <summary>
    /// 创建与当前宿主匹配的发现配置。
    /// </summary>
    public CBusDiscoveryOptions CreateDiscoveryOptions()
    {
        return new CBusDiscoveryOptions(Options.PublishDirectory, Options.RegistrySubKey);
    }

    /// <summary>
    /// 停止宿主并释放监听资源。
    /// </summary>
    public ValueTask DisposeAsync()
    {
        return new ValueTask(StopAsync());
    }
}

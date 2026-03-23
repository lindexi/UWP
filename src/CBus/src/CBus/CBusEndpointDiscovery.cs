namespace CBus;

/// <summary>
/// 负责从发布信息中发现 CBus 终结点。
/// </summary>
public sealed class CBusEndpointDiscovery
{
    private readonly ICBusRegistryStore _registryStore;
    private readonly ICBusFileStore _fileStore;
    private readonly CBusDiscoveryOptions _options;

    /// <summary>
    /// 使用指定的注册表和文件存储创建终结点发现器。
    /// </summary>
    public CBusEndpointDiscovery(ICBusRegistryStore registryStore, ICBusFileStore fileStore, CBusDiscoveryOptions? options = null)
    {
        _registryStore = registryStore ?? throw new ArgumentNullException(nameof(registryStore));
        _fileStore = fileStore ?? throw new ArgumentNullException(nameof(fileStore));
        _options = options ?? new CBusDiscoveryOptions(CBusDefaults.DefaultPublishDirectory, CBusDefaults.DefaultRegistrySubKey);
    }

    /// <summary>
    /// 从注册表和文件系统中发现 CBus 终结点。
    /// </summary>
    public async Task<CBusEndpointDescriptor> DiscoverAsync(CancellationToken cancellationToken = default)
    {
        var portText = await _registryStore.GetValueAsync(_options.RegistrySubKey, CBusDefaults.HttpPortValueName, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(portText))
        {
            throw new InvalidOperationException("HTTP port is not published in registry.");
        }

        if (!int.TryParse(portText, out var httpPort) || httpPort <= 0)
        {
            throw new FormatException("Published HTTP port is invalid.");
        }

        var pipeFilePath = Path.Combine(_options.PublishDirectory, CBusDefaults.PipeAddressFileName);
        var pipeAddress = await _fileStore.ReadAllTextAsync(pipeFilePath, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new InvalidOperationException("Pipe address is not published in file system.");
        }

        return new CBusEndpointDescriptor(new Uri($"http://127.0.0.1:{httpPort}/", UriKind.Absolute), pipeAddress.Trim());
    }
}

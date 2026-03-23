using CBus;

namespace CBus.Hosting;

/// <summary>
/// 负责将宿主监听地址发布到发现介质。
/// </summary>
public sealed class CBusAddressPublisher
{
    private readonly ICBusRegistryStore _registryStore;
    private readonly ICBusFileStore _fileStore;

    /// <summary>
    /// 使用指定的注册表和文件存储创建地址发布器。
    /// </summary>
    public CBusAddressPublisher(ICBusRegistryStore registryStore, ICBusFileStore fileStore)
    {
        _registryStore = registryStore ?? throw new ArgumentNullException(nameof(registryStore));
        _fileStore = fileStore ?? throw new ArgumentNullException(nameof(fileStore));
    }

    /// <summary>
    /// 将宿主监听地址发布到注册表和文件系统。
    /// </summary>
    public async Task PublishAsync(CBusHostOptions options, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        await _registryStore
            .SetValueAsync(options.RegistrySubKey, CBusDefaults.HttpPortValueName, options.HttpPort.ToString(), cancellationToken)
            .ConfigureAwait(false);

        var pipeFilePath = Path.Combine(options.PublishDirectory, CBusDefaults.PipeAddressFileName);
        await _fileStore.WriteAllTextAsync(pipeFilePath, options.PipeAddress, cancellationToken).ConfigureAwait(false);
    }
}

namespace CBus;

/// <summary>
/// 表示发现 CBus 终结点时使用的配置。
/// </summary>
public sealed record CBusDiscoveryOptions
{
    /// <summary>
    /// 使用发布目录和注册表子键创建发现配置。
    /// </summary>
    public CBusDiscoveryOptions(string publishDirectory, string registrySubKey)
    {
        if (string.IsNullOrWhiteSpace(publishDirectory))
        {
            throw new ArgumentException("Publish directory cannot be null or whitespace.", nameof(publishDirectory));
        }

        if (string.IsNullOrWhiteSpace(registrySubKey))
        {
            throw new ArgumentException("Registry sub key cannot be null or whitespace.", nameof(registrySubKey));
        }

        PublishDirectory = publishDirectory;
        RegistrySubKey = registrySubKey;
    }

    /// <summary>
    /// 获取发布命名管道地址的目录。
    /// </summary>
    public string PublishDirectory { get; }

    /// <summary>
    /// 获取发布 HTTP 端口的注册表子键。
    /// </summary>
    public string RegistrySubKey { get; }
}

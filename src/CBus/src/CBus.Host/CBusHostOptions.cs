using CBus;

namespace CBus.Hosting;

/// <summary>
/// 表示 CBus 宿主的运行配置。
/// </summary>
public sealed record CBusHostOptions
{
    /// <summary>
    /// 使用 HTTP 端口、管道地址和发布配置创建宿主选项。
    /// </summary>
    public CBusHostOptions(int httpPort, string pipeAddress, string publishDirectory, string registrySubKey)
    {
        if (httpPort <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(httpPort));
        }

        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new ArgumentException("Pipe address cannot be null or whitespace.", nameof(pipeAddress));
        }

        if (string.IsNullOrWhiteSpace(publishDirectory))
        {
            throw new ArgumentException("Publish directory cannot be null or whitespace.", nameof(publishDirectory));
        }

        if (string.IsNullOrWhiteSpace(registrySubKey))
        {
            throw new ArgumentException("Registry sub key cannot be null or whitespace.", nameof(registrySubKey));
        }

        HttpPort = httpPort;
        PipeAddress = pipeAddress;
        PublishDirectory = publishDirectory;
        RegistrySubKey = registrySubKey;
    }

    /// <summary>
    /// 获取 HTTP 监听端口。
    /// </summary>
    public int HttpPort { get; }

    /// <summary>
    /// 获取命名管道地址。
    /// </summary>
    public string PipeAddress { get; }

    /// <summary>
    /// 获取发布目录。
    /// </summary>
    public string PublishDirectory { get; }

    /// <summary>
    /// 获取发布 HTTP 端口使用的注册表子键。
    /// </summary>
    public string RegistrySubKey { get; }
}

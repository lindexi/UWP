namespace CBus;

/// <summary>
/// 提供 CBus 默认配置值。
/// </summary>
public static class CBusDefaults
{
    /// <summary>
    /// 获取默认 HTTP 监听端口。
    /// </summary>
    public const int DefaultPort = 3323;

    /// <summary>
    /// 获取默认命名管道地址。
    /// </summary>
    public const string DefaultPipeAddress = "cbus.pipe";

    /// <summary>
    /// 获取默认注册表子键。
    /// </summary>
    public const string DefaultRegistrySubKey = @"Software\CBus";

    /// <summary>
    /// 获取发布 HTTP 端口时使用的注册表值名称。
    /// </summary>
    public const string HttpPortValueName = "HttpPort";

    /// <summary>
    /// 获取发布命名管道地址时使用的文件名称。
    /// </summary>
    public const string PipeAddressFileName = "cbus.pipe";

    /// <summary>
    /// 获取默认的发布目录。
    /// </summary>
    public static string DefaultPublishDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "CBus");
}

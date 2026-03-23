namespace CBus;

/// <summary>
/// 定义发现信息使用的注册表存储能力。
/// </summary>
public interface ICBusRegistryStore
{
    /// <summary>
    /// 读取注册表中的字符串值。
    /// </summary>
    Task<string?> GetValueAsync(string subKey, string valueName, CancellationToken cancellationToken = default);

    /// <summary>
    /// 将字符串值写入注册表。
    /// </summary>
    Task SetValueAsync(string subKey, string valueName, string value, CancellationToken cancellationToken = default);
}

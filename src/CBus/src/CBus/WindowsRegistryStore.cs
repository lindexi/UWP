using Microsoft.Win32;

namespace CBus;

public sealed class WindowsRegistryStore : ICBusRegistryStore
{
    /// <summary>
    /// 读取注册表中的字符串值。
    /// </summary>
    public Task<string?> GetValueAsync(string subKey, string valueName, CancellationToken cancellationToken = default)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("Windows registry is only available on Windows.");
        }

        if (string.IsNullOrWhiteSpace(subKey))
        {
            throw new ArgumentException("Sub key cannot be null or whitespace.", nameof(subKey));
        }

        if (string.IsNullOrWhiteSpace(valueName))
        {
            throw new ArgumentException("Value name cannot be null or whitespace.", nameof(valueName));
        }

        cancellationToken.ThrowIfCancellationRequested();

        using var key = Registry.CurrentUser.OpenSubKey(subKey, writable: false);
        return Task.FromResult(key?.GetValue(valueName)?.ToString());
    }

    /// <summary>
    /// 将字符串值写入注册表。
    /// </summary>
    public Task SetValueAsync(string subKey, string valueName, string value, CancellationToken cancellationToken = default)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("Windows registry is only available on Windows.");
        }

        if (string.IsNullOrWhiteSpace(subKey))
        {
            throw new ArgumentException("Sub key cannot be null or whitespace.", nameof(subKey));
        }

        if (string.IsNullOrWhiteSpace(valueName))
        {
            throw new ArgumentException("Value name cannot be null or whitespace.", nameof(valueName));
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
        }

        cancellationToken.ThrowIfCancellationRequested();

        using var key = Registry.CurrentUser.CreateSubKey(subKey);
        key.SetValue(valueName, value, RegistryValueKind.String);
        return Task.CompletedTask;
    }
}

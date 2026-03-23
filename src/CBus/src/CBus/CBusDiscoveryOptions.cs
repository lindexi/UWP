namespace CBus;

public sealed record CBusDiscoveryOptions
{
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

    public string PublishDirectory { get; }

    public string RegistrySubKey { get; }
}

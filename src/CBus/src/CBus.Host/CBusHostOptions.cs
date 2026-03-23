using CBus;

namespace CBus.Hosting;

public sealed record CBusHostOptions
{
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

    public int HttpPort { get; }

    public string PipeAddress { get; }

    public string PublishDirectory { get; }

    public string RegistrySubKey { get; }
}

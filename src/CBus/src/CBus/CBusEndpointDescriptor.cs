namespace CBus;

public sealed record CBusEndpointDescriptor
{
    public CBusEndpointDescriptor(Uri httpEndpoint, string pipeAddress)
    {
        ArgumentNullException.ThrowIfNull(httpEndpoint);
        if (!httpEndpoint.IsAbsoluteUri)
        {
            throw new ArgumentException("HTTP endpoint must be an absolute URI.", nameof(httpEndpoint));
        }

        if (string.IsNullOrWhiteSpace(pipeAddress))
        {
            throw new ArgumentException("Pipe address cannot be null or whitespace.", nameof(pipeAddress));
        }

        HttpEndpoint = httpEndpoint;
        PipeAddress = pipeAddress;
    }

    public Uri HttpEndpoint { get; }

    public string PipeAddress { get; }
}

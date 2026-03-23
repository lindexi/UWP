using CBus.Hosting;

namespace CBus.Tests;

internal sealed class InMemoryRegistryStore : ICBusRegistryStore
{
    private readonly Dictionary<(string SubKey, string ValueName), string> _values = new();

    public Task<string?> GetValueAsync(string subKey, string valueName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _values.TryGetValue((subKey, valueName), out var value);
        return Task.FromResult<string?>(value);
    }

    public Task SetValueAsync(string subKey, string valueName, string value, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _values[(subKey, valueName)] = value;
        return Task.CompletedTask;
    }
}

internal sealed class InMemoryFileStore : ICBusFileStore
{
    private readonly Dictionary<string, string> _files = new(StringComparer.OrdinalIgnoreCase);

    public Task<string?> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _files.TryGetValue(path, out var content);
        return Task.FromResult<string?>(content);
    }

    public Task WriteAllTextAsync(string path, string content, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _files[path] = content;
        return Task.CompletedTask;
    }
}

internal sealed class CapturingHttpTransport : ICBusHttpTransport
{
    public Uri? LastEndpoint { get; private set; }

    public CBusRequest? LastRequest { get; private set; }

    public Task<CBusResponse> SendAsync(Uri endpoint, CBusRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastEndpoint = endpoint;
        LastRequest = request;
        return Task.FromResult(CBusResponse.Ok("http-transport"));
    }
}

internal sealed class CapturingPipeTransport : ICBusPipeTransport
{
    public string? LastPipeAddress { get; private set; }

    public CBusRequest? LastRequest { get; private set; }

    public Task<CBusResponse> SendAsync(string pipeAddress, CBusRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        LastPipeAddress = pipeAddress;
        LastRequest = request;
        return Task.FromResult(CBusResponse.Ok("pipe-transport"));
    }
}

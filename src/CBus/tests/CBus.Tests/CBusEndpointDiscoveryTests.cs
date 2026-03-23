namespace CBus.Tests;

public class CBusEndpointDiscoveryTests
{
    [Fact]
    public async Task WhenPublishedEndpointsExistThenDiscoveryReturnsConfiguredHttpEndpointAsync()
    {
        var registryStore = new InMemoryRegistryStore();
        var fileStore = new InMemoryFileStore();
        var options = new CBusDiscoveryOptions("cbus-discovery", "Software\\CBus\\Tests");
        await registryStore.SetValueAsync(options.RegistrySubKey, CBusDefaults.HttpPortValueName, "4321");
        await fileStore.WriteAllTextAsync(Path.Combine(options.PublishDirectory, CBusDefaults.PipeAddressFileName), "pipe-a");
        var discovery = new CBusEndpointDiscovery(registryStore, fileStore, options);

        var result = await discovery.DiscoverAsync();

        Assert.Equal(new Uri("http://127.0.0.1:4321/"), result.HttpEndpoint);
    }

    [Fact]
    public async Task WhenPublishedEndpointsExistThenDiscoveryReturnsConfiguredPipeAddressAsync()
    {
        var registryStore = new InMemoryRegistryStore();
        var fileStore = new InMemoryFileStore();
        var options = new CBusDiscoveryOptions("cbus-discovery", "Software\\CBus\\Tests");
        await registryStore.SetValueAsync(options.RegistrySubKey, CBusDefaults.HttpPortValueName, "4321");
        await fileStore.WriteAllTextAsync(Path.Combine(options.PublishDirectory, CBusDefaults.PipeAddressFileName), "pipe-a");
        var discovery = new CBusEndpointDiscovery(registryStore, fileStore, options);

        var result = await discovery.DiscoverAsync();

        Assert.Equal("pipe-a", result.PipeAddress);
    }
}

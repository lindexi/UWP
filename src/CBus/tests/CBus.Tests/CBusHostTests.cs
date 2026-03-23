using CBus.Hosting;

namespace CBus.Tests;

public class CBusHostTests
{
    [Fact]
    public async Task WhenHostStartsThenDiscoveryReturnsPublishedHttpEndpointAsync()
    {
        var registryStore = new InMemoryRegistryStore();
        var fileStore = new InMemoryFileStore();
        var host = CreateHost(registryStore, fileStore);
        await host.StartAsync();
        var discovery = new CBusEndpointDiscovery(registryStore, fileStore, host.CreateDiscoveryOptions());

        var result = await discovery.DiscoverAsync();

        Assert.Equal(new Uri("http://127.0.0.1:4455/"), result.HttpEndpoint);
    }

    [Fact]
    public async Task WhenHostStartsThenDiscoveryReturnsPublishedPipeAddressAsync()
    {
        var registryStore = new InMemoryRegistryStore();
        var fileStore = new InMemoryFileStore();
        var host = CreateHost(registryStore, fileStore);
        await host.StartAsync();
        var discovery = new CBusEndpointDiscovery(registryStore, fileStore, host.CreateDiscoveryOptions());

        var result = await discovery.DiscoverAsync();

        Assert.Equal("cbus-test-pipe", result.PipeAddress);
    }

    [Fact]
    public async Task WhenHttpListenerReceivesRegisteredRouteThenReturnsHandlerBodyAsync()
    {
        var host = CreateHost(new InMemoryRegistryStore(), new InMemoryFileStore());
        host.RegisterService(
            new CBusServiceRegistration("FooService", "FooService.exe", [], [new CBusRouteDefinition("/Foo/Path1")]),
            static (_, _) => Task.FromResult(CBusResponse.Ok("from-http-listener")));

        var response = await host.HttpListener.SendAsync(host.HttpListener.Endpoint, new CBusRequest("GET", "/Foo/Path1"));

        Assert.Equal("from-http-listener", response.GetBodyAsString());
    }

    [Fact]
    public async Task WhenPipeListenerReceivesUnknownRouteThenReturnsNotFoundStatusCodeAsync()
    {
        var host = CreateHost(new InMemoryRegistryStore(), new InMemoryFileStore());

        var response = await host.PipeListener.SendAsync(host.PipeListener.PipeAddress, new CBusRequest("GET", "/Missing/Path"));

        Assert.Equal(404, response.StatusCode);
    }

    private static CBusHost CreateHost(ICBusRegistryStore registryStore, ICBusFileStore fileStore)
    {
        return new CBusHost(
            new CBusHostOptions(4455, "cbus-test-pipe", "cbus-host-tests", "Software\\CBus\\HostTests"),
            registryStore,
            fileStore);
    }
}

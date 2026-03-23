using System.Net;
using System.Net.Sockets;
using CBus.Hosting;

namespace CBus.Tests;

public class CBusHostTests
{
    [Fact]
    public async Task WhenHostStartsThenDiscoveryReturnsPublishedHttpEndpointAsync()
    {
        var registryStore = new InMemoryRegistryStore();
        var fileStore = new InMemoryFileStore();
        var httpPort = GetAvailablePort();
        await using var host = CreateHost(registryStore, fileStore, httpPort, CreatePipeAddress());
        await host.StartAsync();
        var discovery = new CBusEndpointDiscovery(registryStore, fileStore, host.CreateDiscoveryOptions());

        var result = await discovery.DiscoverAsync();

        Assert.Equal(new Uri($"http://127.0.0.1:{httpPort}/"), result.HttpEndpoint);
    }

    [Fact]
    public async Task WhenHostStartsThenDiscoveryReturnsPublishedPipeAddressAsync()
    {
        var registryStore = new InMemoryRegistryStore();
        var fileStore = new InMemoryFileStore();
        var pipeAddress = CreatePipeAddress();
        await using var host = CreateHost(registryStore, fileStore, GetAvailablePort(), pipeAddress);
        await host.StartAsync();
        var discovery = new CBusEndpointDiscovery(registryStore, fileStore, host.CreateDiscoveryOptions());

        var result = await discovery.DiscoverAsync();

        Assert.Equal(pipeAddress, result.PipeAddress);
    }

    [Fact]
    public async Task WhenHttpListenerReceivesRegisteredRouteThenReturnsHandlerBodyAsync()
    {
        await using var host = CreateHost(new InMemoryRegistryStore(), new InMemoryFileStore(), GetAvailablePort(), CreatePipeAddress());
        host.RegisterService(
            new CBusServiceRegistration("FooService", "FooService.exe", [], [new CBusRouteDefinition("/Foo/Path1")]),
            static (request, _) => Task.FromResult(CBusResponse.Ok(request.GetBodyAsString())));

        await host.StartAsync();

        var connector = new HttpConnector(host.HttpListener.Endpoint, new SystemNetHttpTransport());
        var response = await connector.SendAsync(CBusRequest.CreateText("POST", "/Foo/Path1", "from-http-listener"));

        Assert.Equal("from-http-listener", response.GetBodyAsString());
    }

    [Fact]
    public async Task WhenPipeListenerReceivesRegisteredRouteThenReturnsHandlerBodyAsync()
    {
        await using var host = CreateHost(new InMemoryRegistryStore(), new InMemoryFileStore(), GetAvailablePort(), CreatePipeAddress());
        host.RegisterService(
            new CBusServiceRegistration("FooService", "FooService.exe", [], [new CBusRouteDefinition("/Foo/Path1")]),
            static (request, _) => Task.FromResult(CBusResponse.Ok(request.GetBodyAsString())));

        await host.StartAsync();

        var connector = new PipeConnector(host.PipeListener.PipeAddress, new SystemNamedPipeTransport());
        var response = await connector.SendAsync(CBusRequest.CreateText("POST", "/Foo/Path1", "from-pipe-listener"));

        Assert.Equal("from-pipe-listener", response.GetBodyAsString());
    }

    [Fact]
    public async Task WhenPipeListenerReceivesUnknownRouteThenReturnsNotFoundStatusCodeAsync()
    {
        await using var host = CreateHost(new InMemoryRegistryStore(), new InMemoryFileStore(), GetAvailablePort(), CreatePipeAddress());

        await host.StartAsync();

        var connector = new PipeConnector(host.PipeListener.PipeAddress, new SystemNamedPipeTransport());
        var response = await connector.SendAsync(new CBusRequest("GET", "/Missing/Path"));

        Assert.Equal(404, response.StatusCode);
    }

    private static CBusHost CreateHost(ICBusRegistryStore registryStore, ICBusFileStore fileStore, int httpPort, string pipeAddress)
    {
        return new CBusHost(
            new CBusHostOptions(httpPort, pipeAddress, "cbus-host-tests", "Software\\CBus\\HostTests"),
            registryStore,
            fileStore);
    }

    private static int GetAvailablePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }

    private static string CreatePipeAddress()
    {
        return $"cbus-test-pipe-{Guid.NewGuid():N}";
    }
}

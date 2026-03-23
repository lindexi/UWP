using CBus.Hosting;

namespace CBus.Tests;

public class CBusDispatcherTests
{
    [Fact]
    public async Task WhenDispatchMatchesServiceThenReturnsServiceResponseBodyAsync()
    {
        var registry = new CBusServiceRegistry();
        registry.Register(
            new CBusServiceRegistration("FooService", "FooService.exe", [], [new CBusRouteDefinition("/Foo/Path1")]),
            static (_, _) => Task.FromResult(CBusResponse.Ok("from-service")));
        var dispatcher = new CBusDispatcher(registry);

        var result = await dispatcher.DispatchAsync(new CBusRequest("GET", "/Foo/Path1"));

        Assert.Equal("from-service", result.Response.GetBodyAsString());
    }

    [Fact]
    public async Task WhenDispatchMatchesServiceThenReturnsMatchedServiceNameAsync()
    {
        var registry = new CBusServiceRegistry();
        registry.Register(
            new CBusServiceRegistration("FooService", "FooService.exe", [], [new CBusRouteDefinition("/Foo/Path1")]),
            static (_, _) => Task.FromResult(CBusResponse.Ok("from-service")));
        var dispatcher = new CBusDispatcher(registry);

        var result = await dispatcher.DispatchAsync(new CBusRequest("GET", "/Foo/Path1"));

        Assert.Equal("FooService", result.MatchedServiceName);
    }

    [Fact]
    public async Task WhenDispatchDoesNotMatchServiceThenReturnsNotFoundAsync()
    {
        var dispatcher = new CBusDispatcher(new CBusServiceRegistry());

        var result = await dispatcher.DispatchAsync(new CBusRequest("GET", "/Missing/Path"));

        Assert.Equal(404, result.Response.StatusCode);
    }
}

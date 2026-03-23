namespace CBus.Tests;

public class ConnectorTests
{
    [Fact]
    public async Task WhenSendingSameRequestThenHttpConnectorAndPipeConnectorReturnSameBodyAsync()
    {
        var registry = new CBusServiceRegistry();
        registry.Register(
            new CBusServiceRegistration("FooService", "FooService.exe", [], [new CBusRouteDefinition("/Foo/Path1")]),
            static (request, _) => Task.FromResult(CBusResponse.Ok($"handled:{request.Path}")));
        var dispatcher = new CBusDispatcher(registry);
        var httpConnector = new HttpConnector(dispatcher);
        var pipeConnector = new PipeConnector(dispatcher);
        var request = new CBusRequest("GET", "/Foo/Path1");

        var httpResponse = await httpConnector.SendAsync(request);
        var pipeResponse = await pipeConnector.SendAsync(request);

        Assert.Equal(httpResponse.GetBodyAsString(), pipeResponse.GetBodyAsString());
    }
}

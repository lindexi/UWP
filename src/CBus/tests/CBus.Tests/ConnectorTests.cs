namespace CBus.Tests;

public class ConnectorTests
{
    [Fact]
    public async Task WhenHttpConnectorSendsRequestThenTransportReceivesConfiguredEndpointAsync()
    {
        var transport = new CapturingHttpTransport();
        var endpoint = new Uri("http://127.0.0.1:3323/");
        var connector = new HttpConnector(endpoint, transport);

        await connector.SendAsync(new CBusRequest("GET", "/Foo/Path1"));

        Assert.Equal(endpoint, transport.LastEndpoint);
    }

    [Fact]
    public async Task WhenHttpConnectorSendsRequestThenTransportResponseIsReturnedAsync()
    {
        var connector = new HttpConnector(new Uri("http://127.0.0.1:3323/"), new CapturingHttpTransport());

        var response = await connector.SendAsync(new CBusRequest("GET", "/Foo/Path1"));

        Assert.Equal("http-transport", response.GetBodyAsString());
    }

    [Fact]
    public async Task WhenPipeConnectorSendsRequestThenTransportReceivesConfiguredPipeAddressAsync()
    {
        var transport = new CapturingPipeTransport();
        var connector = new PipeConnector("cbus.pipe", transport);

        await connector.SendAsync(new CBusRequest("GET", "/Foo/Path1"));

        Assert.Equal("cbus.pipe", transport.LastPipeAddress);
    }

    [Fact]
    public async Task WhenPipeConnectorSendsRequestThenTransportResponseIsReturnedAsync()
    {
        var connector = new PipeConnector("cbus.pipe", new CapturingPipeTransport());

        var response = await connector.SendAsync(new CBusRequest("GET", "/Foo/Path1"));

        Assert.Equal("pipe-transport", response.GetBodyAsString());
    }
}

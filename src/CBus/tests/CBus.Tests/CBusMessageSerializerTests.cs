namespace CBus.Tests;

public class CBusMessageSerializerTests
{
    [Fact]
    public void WhenSerializeThenDeserializeRequestThenBodyIsPreserved()
    {
        var serializer = new CBusMessageSerializer();
        var request = CBusRequest.CreateText("POST", "/Foo/Path1", "hello", new Dictionary<string, string>
        {
            ["X-Test"] = "1"
        });

        var result = serializer.DeserializeRequest(serializer.SerializeRequest(request));

        Assert.Equal("hello", result.GetBodyAsString());
    }

    [Fact]
    public void WhenSerializeThenDeserializeResponseThenStatusCodeIsPreserved()
    {
        var serializer = new CBusMessageSerializer();
        var response = CBusResponse.Ok("world", new Dictionary<string, string>
        {
            ["X-Test"] = "1"
        });

        var result = serializer.DeserializeResponse(serializer.SerializeResponse(response));

        Assert.Equal(200, result.StatusCode);
    }
}

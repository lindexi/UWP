namespace CBus.Tests;

public class CBusRouteDefinitionTests
{
    [Fact]
    public void WhenPathIsNullThenCtorThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new CBusRouteDefinition(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("Foo/Bar")]
    [InlineData("/")]
    public void WhenPathIsInvalidThenCtorThrowsArgumentException(string path)
    {
        Assert.Throws<ArgumentException>(() => new CBusRouteDefinition(path));
    }

    [Fact]
    public void WhenPathContainsMultipleSegmentsThenRouteRootUsesFirstSegment()
    {
        var routeDefinition = new CBusRouteDefinition("/Foo/Path1");

        Assert.Equal("Foo", routeDefinition.RouteRoot);
    }
}

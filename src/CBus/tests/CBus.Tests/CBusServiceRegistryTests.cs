using CBus.Hosting;

namespace CBus.Tests;

public class CBusServiceRegistryTests
{
    [Fact]
    public void WhenRegisterUniqueRouteRootThenRegistrationCanBeResolvedByPath()
    {
        var registry = new CBusServiceRegistry();
        var registration = CreateRegistration("FooService", "/Foo/Path1");

        registry.Register(registration, static (_, _) => Task.FromResult(CBusResponse.Ok("handled")));

        var result = registry.TryGetRegistrationByPath("/Foo/Path1", out var resolvedRegistration);

        Assert.True(result);
    }

    [Fact]
    public void WhenRegistrationIsResolvedThenResolvedRegistrationMatchesInput()
    {
        var registry = new CBusServiceRegistry();
        var registration = CreateRegistration("FooService", "/Foo/Path1");
        registry.Register(registration, static (_, _) => Task.FromResult(CBusResponse.Ok("handled")));

        registry.TryGetRegistrationByPath("/Foo/Path1", out var resolvedRegistration);

        Assert.Same(registration, resolvedRegistration);
    }

    [Fact]
    public void WhenRegisterDuplicateRouteRootThenRegisterThrowsInvalidOperationException()
    {
        var registry = new CBusServiceRegistry();
        registry.Register(CreateRegistration("FooService", "/Foo/Path1"), static (_, _) => Task.FromResult(CBusResponse.Ok("handled")));

        Assert.Throws<InvalidOperationException>(() =>
            registry.Register(CreateRegistration("BarService", "/Foo/Path2"), static (_, _) => Task.FromResult(CBusResponse.Ok("handled"))));
    }

    private static CBusServiceRegistration CreateRegistration(string serviceName, string path)
    {
        return new CBusServiceRegistration(serviceName, $"{serviceName}.exe", [], [new CBusRouteDefinition(path)]);
    }
}

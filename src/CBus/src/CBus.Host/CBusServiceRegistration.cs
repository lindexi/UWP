using CBus;

namespace CBus.Hosting;

public sealed class CBusServiceRegistration
{
    public CBusServiceRegistration(string serviceName, string executablePath, IReadOnlyList<string>? arguments, IReadOnlyList<CBusRouteDefinition> routes)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            throw new ArgumentException("Service name cannot be null or whitespace.", nameof(serviceName));
        }

        if (string.IsNullOrWhiteSpace(executablePath))
        {
            throw new ArgumentException("Executable path cannot be null or whitespace.", nameof(executablePath));
        }

        ArgumentNullException.ThrowIfNull(routes);
        if (routes.Count == 0)
        {
            throw new ArgumentException("At least one route is required.", nameof(routes));
        }

        ServiceName = serviceName;
        ExecutablePath = executablePath;
        Arguments = arguments?.ToArray() ?? [];
        Routes = routes.ToArray();
    }

    public string ServiceName { get; }

    public string ExecutablePath { get; }

    public IReadOnlyList<string> Arguments { get; }

    public IReadOnlyList<CBusRouteDefinition> Routes { get; }
}

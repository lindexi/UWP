using CBus;

namespace CBus.Hosting;

/// <summary>
/// 表示一个可由宿主承载的服务注册信息。
/// </summary>
public sealed class CBusServiceRegistration
{
    /// <summary>
    /// 使用服务名称、可执行文件和路由定义创建注册信息。
    /// </summary>
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

    /// <summary>
    /// 获取服务名称。
    /// </summary>
    public string ServiceName { get; }

    /// <summary>
    /// 获取服务可执行文件路径。
    /// </summary>
    public string ExecutablePath { get; }

    /// <summary>
    /// 获取启动服务时使用的参数列表。
    /// </summary>
    public IReadOnlyList<string> Arguments { get; }

    /// <summary>
    /// 获取服务声明的路由集合。
    /// </summary>
    public IReadOnlyList<CBusRouteDefinition> Routes { get; }
}

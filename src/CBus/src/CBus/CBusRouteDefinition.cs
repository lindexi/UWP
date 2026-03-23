namespace CBus;

public sealed class CBusRouteDefinition
{
    public CBusRouteDefinition(string path)
    {
        Path = ValidatePath(path);
        RouteRoot = GetRouteRoot(path);
    }

    public string Path { get; }

    public string RouteRoot { get; }

    /// <summary>
    /// 从路由路径中提取根路径段。
    /// </summary>
    public static string GetRouteRoot(string path)
    {
        var normalizedPath = ValidatePath(path);
        var segments = normalizedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return segments[0];
    }

    internal static string ValidateAndReturnPath(string path)
    {
        return ValidatePath(path);
    }

    private static string ValidatePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or whitespace.", nameof(path));
        }

        if (!path.StartsWith("/", StringComparison.Ordinal))
        {
            throw new ArgumentException("Path must start with '/'.", nameof(path));
        }

        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0)
        {
            throw new ArgumentException("Path must contain at least one segment.", nameof(path));
        }

        foreach (var segment in segments)
        {
            if (string.IsNullOrWhiteSpace(segment))
            {
                throw new ArgumentException("Path segments cannot be whitespace.", nameof(path));
            }
        }

        return path;
    }
}

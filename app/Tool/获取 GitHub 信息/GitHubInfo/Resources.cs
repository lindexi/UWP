using System.Globalization;
using System.Resources;

namespace GitHubInfo;

internal static class Resources
{
    private static readonly ResourceManager ResourceManager =
        new("GitHubInfo.Resources", typeof(Resources).Assembly);

    public static string Usage => GetString(nameof(Usage));

    public static string InvalidGitHubUrl => GetString(nameof(InvalidGitHubUrl));

    public static string OutputWritten => GetString(nameof(OutputWritten));

    public static string IssueTitle => GetString(nameof(IssueTitle));

    public static string UrlLabel => GetString(nameof(UrlLabel));

    public static string StateLabel => GetString(nameof(StateLabel));

    public static string SaidAt => GetString(nameof(SaidAt));

    public static string Format(string value, params object[] arguments) =>
        string.Format(CultureInfo.CurrentCulture, value, arguments);

    private static string GetString(string name) =>
        ResourceManager.GetString(name, CultureInfo.CurrentUICulture)
        ?? throw new MissingManifestResourceException($"Resource '{name}' was not found.");
}

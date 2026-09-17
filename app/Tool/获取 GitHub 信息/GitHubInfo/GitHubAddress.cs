using System.Globalization;

namespace GitHubInfo;

internal enum GitHubAddressKind
{
    PullRequest,
    Issue,
}

internal sealed record GitHubAddress
(
    string Owner,
    string Repository,
    GitHubAddressKind Kind,
    int Number
)
{
    public Uri ApiContentUri => new
    (
        $"https://api.github.com/repos/{Uri.EscapeDataString(Owner)}/{Uri.EscapeDataString(Repository)}/pulls/{Number}"
    );

    public static GitHubAddress Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value)
            || !Uri.TryCreate(value, UriKind.Absolute, out var uri)
            || !string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
            || !string.Equals(uri.IdnHost, "github.com", StringComparison.OrdinalIgnoreCase)
            || !string.IsNullOrEmpty(uri.UserInfo)
            || (!uri.IsDefaultPort && uri.Port != 443))
        {
            throw new ArgumentException(Resources.InvalidGitHubUrl, nameof(value));
        }

        var segments = uri.GetComponents(UriComponents.Path, UriFormat.Unescaped)
            .Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length != 4
            || !IsValidRepositorySegment(segments[0])
            || !IsValidRepositorySegment(segments[1])
            || !int.TryParse(segments[3], NumberStyles.None, CultureInfo.InvariantCulture, out var number)
            || number <= 0)
        {
            throw new ArgumentException(Resources.InvalidGitHubUrl, nameof(value));
        }

        var kind = segments[2].ToLowerInvariant() switch
        {
            "pull" => GitHubAddressKind.PullRequest,
            "issues" => GitHubAddressKind.Issue,
            _ => throw new ArgumentException(Resources.InvalidGitHubUrl, nameof(value)),
        };

        return new GitHubAddress(segments[0], segments[1], kind, number);
    }

    private static bool IsValidRepositorySegment(string value) =>
        !string.IsNullOrWhiteSpace(value)
        && value is not "." and not ".."
        && value.All
        (character =>
            char.IsAsciiLetterOrDigit(character)
            || character is '-' or '_' or '.'
        );
}

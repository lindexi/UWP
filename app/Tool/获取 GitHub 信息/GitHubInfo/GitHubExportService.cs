using System.Net.Http.Headers;
using System.Text;
using Octokit;

namespace GitHubInfo;

internal sealed class GitHubExportService
{
    private const string Separator = "========================================";
    private readonly GitHubClient _client;
    private readonly HttpClient _httpClient;

    public GitHubExportService(string? token)
    {
        _client = new GitHubClient(new Octokit.ProductHeaderValue("GitHubInfo"));
        if (!string.IsNullOrWhiteSpace(token))
        {
            _client.Credentials = new Credentials(token);
        }

        _httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(2), };
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GitHubInfo", "1.0"));
        _httpClient.DefaultRequestHeaders.Accept.Add
            (new MediaTypeWithQualityHeaderValue("application/vnd.github.patch"));
        _httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task ExportAsync
    (
        GitHubAddress address,
        string outputPath,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(address);
        if (string.IsNullOrWhiteSpace(outputPath))
        {
            throw new ArgumentException("Output path is required.", nameof(outputPath));
        }

        var content = address.Kind switch
        {
            GitHubAddressKind.PullRequest => await GetPullRequestPatchAsync(address, cancellationToken)
                .ConfigureAwait(false),
            GitHubAddressKind.Issue => await GetIssueMarkdownAsync(address, cancellationToken)
                .ConfigureAwait(false),
            _ => throw new InvalidOperationException($"Unsupported GitHub address kind: {address.Kind}."),
        };

        var fullPath = Path.GetFullPath(outputPath);
        var directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(fullPath, content, new UTF8Encoding(false), cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<string> GetPullRequestPatchAsync
    (
        GitHubAddress address,
        CancellationToken cancellationToken
    )
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, address.ApiContentUri);
        using var response = await _httpClient.SendAsync
            (
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken
            )
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task<string> GetIssueMarkdownAsync
    (
        GitHubAddress address,
        CancellationToken cancellationToken
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        var issueTask = _client.Issue.Get(address.Owner, address.Repository, address.Number)
            .WaitAsync(cancellationToken);
        var commentsTask = _client.Issue.Comment.GetAllForIssue
            (
                address.Owner,
                address.Repository,
                address.Number,
                new ApiOptions { PageSize = 100 }
            )
            .WaitAsync(cancellationToken);
        await Task.WhenAll(issueTask, commentsTask).ConfigureAwait(false);

        var issue = await issueTask.ConfigureAwait(false);
        var comments = await commentsTask.ConfigureAwait(false);
        var builder = new StringBuilder();
        builder.Append("# ")
            .Append(Resources.IssueTitle)
            .Append(' ')
            .Append(address.Owner)
            .Append('/')
            .Append(address.Repository)
            .Append('#')
            .Append(address.Number)
            .Append(": ")
            .AppendLine(issue.Title)
            .AppendLine()
            .Append(Resources.UrlLabel)
            .Append(": ")
            .AppendLine(issue.HtmlUrl)
            .Append(Resources.StateLabel)
            .Append(": ")
            .AppendLine(issue.State.StringValue)
            .AppendLine()
            .AppendLine(Separator)
            .AppendLine();

        AppendSpeech(builder, issue.User.Login, issue.CreatedAt, issue.Body);
        foreach (var comment in comments.OrderBy(comment => comment.CreatedAt))
        {
            builder.AppendLine(Separator).AppendLine();
            AppendSpeech(builder, comment.User.Login, comment.CreatedAt, comment.Body);
        }

        return builder.ToString();
    }

    private static void AppendSpeech
    (
        StringBuilder builder,
        string author,
        DateTimeOffset createdAt,
        string? body
    )
    {
        builder.AppendLine(Resources.Format(Resources.SaidAt, author, createdAt.ToString("O")))
            .AppendLine()
            .AppendLine(body ?? string.Empty)
            .AppendLine();
    }
}

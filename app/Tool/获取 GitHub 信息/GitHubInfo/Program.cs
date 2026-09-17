using GitHubInfo;

if (args.Length is < 1 or > 2)
{
    Console.Error.WriteLine(Resources.Usage);
    return 2;
}

using var cancellationTokenSource = new CancellationTokenSource();
ConsoleCancelEventHandler cancelHandler = (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellationTokenSource.Cancel();
};
Console.CancelKeyPress += cancelHandler;

try
{
    var address = GitHubAddress.Parse(args[0]);
    var outputPath = args.Length == 2
        ? args[1]
        : GetDefaultOutputPath(address);
    var service = new GitHubExportService(Environment.GetEnvironmentVariable("GITHUB_TOKEN"));
    await service.ExportAsync(address, outputPath, cancellationTokenSource.Token);
    Console.WriteLine(Resources.Format(Resources.OutputWritten, Path.GetFullPath(outputPath)));
    return 0;
}
catch (OperationCanceledException)
{
    return 130;
}
catch (Exception exception) when (exception is ArgumentException or HttpRequestException or Octokit.ApiException
                                      or IOException or UnauthorizedAccessException)
{
    Console.Error.WriteLine(exception.Message);
    return 1;
}
finally
{
    Console.CancelKeyPress -= cancelHandler;
}

static string GetDefaultOutputPath(GitHubAddress address)
{
    var extension = address.Kind == GitHubAddressKind.PullRequest ? ".patch" : ".md";
    var kind = address.Kind == GitHubAddressKind.PullRequest ? "pr" : "issue";
    return $"{address.Owner}-{address.Repository}-{kind}-{address.Number}{extension}";
}

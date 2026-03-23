using CBus;
using CBus.Hosting;

var cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    cancellationTokenSource.Cancel();
};

var processPath = Environment.ProcessPath ?? "CBus.Host";
var host = new CBusHost(
    new CBusHostOptions(
        CBusDefaults.DefaultPort,
        CBusDefaults.DefaultPipeAddress,
        CBusDefaults.DefaultPublishDirectory,
        CBusDefaults.DefaultRegistrySubKey),
    new WindowsRegistryStore(),
    new SystemFileStore());

host.RegisterService(
    new CBusServiceRegistration("CBus", processPath, [], [new CBusRouteDefinition("/CBus/Ping")]),
    static (_, _) => Task.FromResult(CBusResponse.Ok("pong")));

await host.StartAsync(cancellationTokenSource.Token);

Console.WriteLine($"CBus host started. HttpEndpoint={host.HttpListener.Endpoint}, PipeAddress={host.PipeListener.PipeAddress}");

try
{
    await Task.Delay(Timeout.InfiniteTimeSpan, cancellationTokenSource.Token);
}
catch (OperationCanceledException)
{
    return;
}

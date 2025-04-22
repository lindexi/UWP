// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

Console.WriteLine("Guardian Start");
if (args.Length == 0)
{
    Console.WriteLine($"Can not find args. 不传参数，守护个寂寞");
    return;
}

Console.WriteLine($"Arguments: {Environment.CommandLine}");

while (true)
{
    var process = StartProcess();
    if (process is null)
    {
        Console.WriteLine($"[Guardian] Start Process Fail. 启动失败，返回 null 值");
        return;
    }

    var processId = process.Id;
    Console.WriteLine($"[Guardian] Start Process Success. 启动成功，PID: {processId}");

    int count = 0;
    while (true)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));

        count++;
        if (count > 100)
        {
            Console.WriteLine($"[Guardian] Process alive.");
            count = 0;
        }

        try
        {
            if (Process.GetProcessById(processId).HasExited)
            {
                Console.WriteLine($"[Guardian] Process HasExited.");
                break;
            }
            else
            {
                // 继续监控
            }
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"[Guardian] Process HasExited GetProcessById Fail. {e.Message}.");
            break;
        }
    }
}


Process? StartProcess()
{
    var processStartInfo = new ProcessStartInfo(args[0]);
    for (var i = 1; i < args.Length; i++)
    {
        processStartInfo.ArgumentList.Add(args[i]);
    }

    return Process.Start(processStartInfo);
}

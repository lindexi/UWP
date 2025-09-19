// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using System.Diagnostics;

long visitVersion = 0;
var failVisitDictionary = new Dictionary<int, ProcessInfo>();

while (true)
{
    visitVersion++;
    Span<string> name = ["SamsungMagician", "QQGuild"];
    Span<string> path = [@"C:\Program Files\ASUS", @"C:\Program Files (x86)\ASUS"];
    var processes = Process.GetProcesses();
    foreach (var process in processes)
    {
        if (failVisitDictionary.TryGetValue(process.Id, out var info))
        {
            if (info.VisitStopwatch.Elapsed < TimeSpan.FromMinutes(10))
            {
                failVisitDictionary[process.Id] = info with
                {
                    VisitVersion = visitVersion
                };

                continue;
            }
            else
            {
                // 超过10分钟，移除，尝试重新访问一下
                failVisitDictionary.Remove(process.Id);
            }
        }
        else
        {
            // 没有在列表里面的，继续访问
        }

        try
        {
            if (name.Contains(process.ProcessName))
            {
                process.Kill();
                continue;
            }

            var fileName = process.MainModule?.FileName;
            foreach (var temp in path)
            {
                if (fileName != null && fileName.StartsWith(temp))
                {
                    process.Kill();
                    continue;
                }
            }
        }
        catch (Exception ex)
        {
            bool ensureFailVisit = false;
            if (ex is Win32Exception win32Exception)
            {
                if ((uint) win32Exception.NativeErrorCode is 0x5 or 0x80004005)
                {
                    ensureFailVisit = true;
                }
            }

            if (ensureFailVisit)
            {
                // 访问失败的，记录下来
                failVisitDictionary[process.Id] = new ProcessInfo(process.Id, Stopwatch.StartNew(), visitVersion);
            }

            Console.WriteLine($"Could not access process ID {process.Id}: {ex.Message}");
        }

        process.Dispose();
    }

    // 清理一下已经不再访问的进程
    var toRemove = failVisitDictionary.Values.Where(info => info.VisitVersion != visitVersion).Select(info => info.ProcessId).ToList();
    foreach (var processId in toRemove)
    {
        failVisitDictionary.Remove(processId);
    }

    await Task.Delay(TimeSpan.FromSeconds(1));
}

record ProcessInfo(int ProcessId, Stopwatch VisitStopwatch, long VisitVersion);

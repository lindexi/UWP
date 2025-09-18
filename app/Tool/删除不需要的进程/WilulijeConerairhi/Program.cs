// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

while (true)
{
    Span<string> name = ["SamsungMagician"];
    Span<string> path = [@"C:\Program Files\ASUS", @"C:\Program Files (x86)\ASUS"];
    var processes = Process.GetProcesses();
    foreach (var process in processes)
    {
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
            Console.WriteLine($"Could not access process ID {process.Id}: {ex.Message}");
        }
    }

    await Task.Delay(TimeSpan.FromSeconds(1));
}

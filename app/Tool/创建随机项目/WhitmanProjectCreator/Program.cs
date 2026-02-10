// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

using Lindexi.Src.WhitmanRandomIdentifier;

var projectType = "wpf";
if (args.Length > 0)
{
    projectType = args[0];
}

var randomIdentifier = new RandomIdentifier();

var folderName = randomIdentifier.Generate(true);

var folderPath = Path.Join(Environment.CurrentDirectory, folderName);

Directory.CreateDirectory(folderPath);

RunCommand($"dotnet new {projectType}", folderPath);
RunCommand($"dotnet new sln", folderPath);
RunCommand($"dotnet sln add .", folderPath);

var slnFile = Path.Join(folderPath, $"{folderName}.sln");
if (!File.Exists(slnFile))
{
    slnFile = Path.Join(folderPath, $"{folderName}.slnx");
    if (!File.Exists(slnFile))
    {
        Console.WriteLine($"无法找到 '{slnFile}' 文件");
        return;
    }
}

Process.Start(new ProcessStartInfo(slnFile) { UseShellExecute = true });


void RunCommand(string command, string workingDirectory)
{
    var processStartInfo = new ProcessStartInfo("cmd")
    {
        RedirectStandardInput = true,
        //RedirectStandardOutput = true,
        //RedirectStandardError = true,
        UseShellExecute = false,
        //CreateNoWindow = true,
        WorkingDirectory = workingDirectory,
    };

    using var process = new Process();
    process.StartInfo = processStartInfo;
    process.Start();

    process.StandardInput.WriteLine(command);
    process.StandardInput.WriteLine("exit");
    process.WaitForExit();
}

// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using dotnetCampus.Cli;
using Lindexi.Src.WhitmanRandomIdentifier;

var option = CommandLine.Parse(args).As<Option>();

if (string.IsNullOrEmpty(option.TemplateFolder) || string.IsNullOrEmpty(option.OutputFolder))
{
    Console.WriteLine($"必须传入模版文件夹和输出文件夹");
    return -1;
}

if (!Directory.Exists(option.TemplateFolder))
{
    Console.WriteLine($"传入的模版文件夹不存在 {option.TemplateFolder}");
}

var randomIdentifier = new RandomIdentifier();
var randomName = randomIdentifier.Generate(true);

var templateFolder = new DirectoryInfo(option.TemplateFolder);
var outputFolder = new DirectoryInfo(option.OutputFolder);

var desFolder = outputFolder.CreateSubdirectory(randomName);

CopyFolder(templateFolder, desFolder, name => name.Replace(templateFolder.Name, randomName));

return 0;

static void CopyFolder(DirectoryInfo originFolder, DirectoryInfo desFolder, Rename rename)
{
    foreach (var directory in originFolder.EnumerateDirectories())
    {
        var originName = directory.Name;
        var newName = rename(originName);

        var newDirectory = desFolder.CreateSubdirectory(newName);
        CopyFolder(directory, newDirectory, rename);
    }

    foreach (var file in originFolder.EnumerateFiles())
    {
        var originName = file.Name;
        var newName = rename(originName);

        var destFileName = Path.Join(desFolder.FullName, newName);

        try
        {
            // 尝试读取文件重写一下
            var content = File.ReadAllText(file.FullName);
            var output = rename(content);
            File.WriteAllText(destFileName, output);
        }
        catch
        {
            file.CopyTo(destFileName, overwrite: true);
        }
    }
}

delegate string Rename(string name);

class Option
{
    [Option("TemplateFolder")]
    public string? TemplateFolder { get; set; }

    [Option("OutputFolder")]
    public string? OutputFolder { get; set; }
}

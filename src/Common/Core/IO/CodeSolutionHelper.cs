namespace Lindexi.Src.Core.IO;

public static class CodeSolutionHelper
{
    public static string? GetCodeSolutionPath(string? currentPath = null)
    {
        currentPath ??= Directory.GetCurrentDirectory();

        while (currentPath != null)
        {
            var files = Directory.EnumerateFiles(currentPath, "*.sln");
            if (files.Any())
            {
                return currentPath;
            }

            currentPath = Path.GetDirectoryName(currentPath);
        }

        return null;
    }
}

using System;
using System.Windows;

namespace Lindexi.Tool.CopyToClipboard
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var commandLine = string.Join(" ", args);
            if (string.IsNullOrEmpty(commandLine))
            {
                Console.WriteLine("啥都没有");
            }
            else
            {
                Clipboard.SetText(commandLine);
                Console.WriteLine($"已复制{commandLine}到剪贴板");
            }
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Office.Interop.Word;
using Path = System.IO.Path;
using Window = System.Windows.Window;

namespace WordPageToImages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertWordPageToImages(FileInfo wordFile, DirectoryInfo outputFolder)
        {
            var applicationClass = new ApplicationClass();
            //applicationClass.Visible = false; 默认值就是 false 值
            var folder = outputFolder.FullName;

            // 截图使用只读方式打开，这里传入的要求是绝对路径
            Document document = applicationClass.Documents.Open(wordFile.FullName, ReadOnly: true);

            var count = 0;

            foreach (Microsoft.Office.Interop.Word.Window documentWindow in document.Windows)
            {
                var documentWindowPanes = documentWindow.Panes;
                for (var index = 0; index < documentWindowPanes.Count; index++)
                {
                    Pane documentWindowPane = documentWindowPanes[index + 1];
                    var pagesCount = documentWindowPane.Pages.Count;
                    for (int i = 0; i < pagesCount; i++)
                    {
                        var page = documentWindowPane.Pages[i + 1];
                        Console.WriteLine($"{page.Width};{page.Height}");
                        count++;
                        var file = Path.Combine(folder, $"{count}.png");

                        var bits = page.EnhMetaFileBits;
                        using (var ms = new MemoryStream((byte[]) (bits)))
                        {
                            var image = System.Drawing.Image.FromStream(ms);
                            image.Save(file);
                        }
                        //page.SaveAsPNG(file); // System.Runtime.InteropServices.COMException: '该方法无法用于对那个对象。'
                    }
                }
            }

            document.Close();
            applicationClass.Quit();
        }

        private void Grid_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                var data = (string[]?) e.Data.GetData(DataFormats.FileDrop);
                if (data is not null)
                {
                    var wordFile = data[0];
                    var folder = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    ConvertWordPageToImages(new FileInfo(wordFile), Directory.CreateDirectory(folder));

                    Process.Start("explorer", $" \"{folder}\" ");
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}

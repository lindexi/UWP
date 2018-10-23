using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using lindexi.MVVM.Framework.ViewModel;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace PandocMarkdown2Docx
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ViewModel = new ViewModel();
            DataContext = ViewModel;
            InitializeComponent();
            ViewModel.OnNavigatedTo(this, null);
        }

        public ViewModel ViewModel { get; }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(ViewModel.Pandoc))
            {
                Tracer.Text = "找不到 Pandoc 文件";
                return;
            }

            if (!File.Exists(ViewModel.Markdown))
            {
                Tracer.Text = "找不到需要转换的文件";
                return;
            }

            var processStartInfo = new ProcessStartInfo()
            {
                FileName = ViewModel.Pandoc,
                Arguments = $" -s -o \"{ViewModel.Docx}\" \"{ViewModel.Markdown}\" --mathjax "
            };

            var process = new Process { StartInfo = processStartInfo };
            process.Start();
            process.WaitForExit();

            Tracer.Text = "转换完成";

            var argument = "/select, \"" + ViewModel.Docx + "\"";

            Process.Start("explorer.exe", argument);
        }

        private void FindMarkdown_OnClick(object sender, RoutedEventArgs e)
        {
            var pick = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Markdown文件|*.md"
            };

            if (pick.ShowDialog(this) is true)
            {
                ViewModel.Markdown = pick.FileName;
                if (!File.Exists(pick.FileName))
                {
                    Tracer.Text = "无法找到 Markdown 文件";
                }
            }
        }

        private void FindDocx_OnClick(object sender, RoutedEventArgs e)
        {
            var pick = new SaveFileDialog()
            {
                Filter = "Word文件|*.docx"
            };

            if (pick.ShowDialog(this) is true)
            {
                ViewModel.Docx = pick.FileName;
            }
        }

        private void FindPandoc_OnClick(object sender, RoutedEventArgs e)
        {
            var pick = new OpenFileDialog
            {
                Multiselect = false,
                FileName = "pandoc.exe",
                Filter = "Pandoc文件|*.exe"
            };

            if (pick.ShowDialog(this) is true)
            {
                ViewModel.Pandoc = pick.FileName;
                if (!File.Exists(pick.FileName))
                {
                    Tracer.Text = "无法找到 Pandoc 文件";
                }
            }
        }
    }

    public class ViewModel : NavigateViewModel
    {
        /// <inheritdoc />
        public ViewModel()
        {
            PropertyChanged += ViewModel_PropertyChanged;
        }

        /// <inheritdoc />
        public override void OnNavigatedTo(object sender, object obj)
        {
            Read();
            base.OnNavigatedTo(sender, obj);
        }


        public string Markdown
        {
            get => _markdown;
            set
            {
                _markdown = value;

                try
                {
                    Docx = Path.Combine(Path.GetDirectoryName(_markdown),
                        Path.GetFileNameWithoutExtension(_markdown) + ".docx");
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }

                OnPropertyChanged();
            }
        }

        public string Docx
        {
            get => _docx;
            set
            {
                if (value == _docx)
                    return;
                _docx = value;
                OnPropertyChanged();
            }
        }

        public string Pandoc
        {
            get => _pandoc;
            set
            {
                if (value == _pandoc)
                    return;
                _pandoc = value;
                OnPropertyChanged();
            }
        }

        private string _markdown;
        private string _docx;
        private string _pandoc;
        private RequiringTask _requiringTask;

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_requiringTask == null)
            {
                _requiringTask = new RequiringTask(Storage, TimeSpan.FromSeconds(3));
            }

            _requiringTask.InvalidateTask();
        }

        private void Read()
        {
            var folder = System.IO.Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var file = Path.Combine(folder, "Pandoc.data");
            if (File.Exists(file))
            {
                Pandoc = File.ReadAllText(file);
            }
            else
            {
                Pandoc = Path.Combine(folder, "pandoc.exe");
            }
        }

        private void Storage()
        {
            var folder = System.IO.Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var file = Path.Combine(folder, "Pandoc.data");

            try
            {
                File.WriteAllText(file, Pandoc);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
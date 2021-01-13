using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OTAManager.Demo.WPF
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

        public static readonly DependencyProperty ApplicationIdProperty = DependencyProperty.Register(
            "ApplicationId", typeof(string), typeof(MainWindow), new PropertyMetadata("123123123123"));

        public string ApplicationId
        {
            get { return (string) GetValue(ApplicationIdProperty); }
            set { SetValue(ApplicationIdProperty, value); }
        }

        public static readonly DependencyProperty VersionProperty = DependencyProperty.Register(
            "Version", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public string Version
        {
            get { return (string) GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

        public static readonly DependencyProperty ApplicationNameProperty = DependencyProperty.Register(
            "ApplicationName", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public string ApplicationName
        {
            get { return (string) GetValue(ApplicationNameProperty); }
            set { SetValue(ApplicationNameProperty, value); }
        }

        public static readonly DependencyProperty InstallerFileNameProperty = DependencyProperty.Register(
            "InstallerFileName", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public string InstallerFileName
        {
            get { return (string) GetValue(InstallerFileNameProperty); }
            set { SetValue(InstallerFileNameProperty, value); }
        }

        public static readonly DependencyProperty InstallerArgumentProperty = DependencyProperty.Register(
            "InstallerArgument", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public string InstallerArgument
        {
            get { return (string) GetValue(InstallerArgumentProperty); }
            set { SetValue(InstallerArgumentProperty, value); }
        }

        public static readonly DependencyProperty ClientApplicationFileInfoTextProperty = DependencyProperty.Register(
            "ClientApplicationFileInfoText", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public string ClientApplicationFileInfoText
        {
            get { return (string) GetValue(ClientApplicationFileInfoTextProperty); }
            set { SetValue(ClientApplicationFileInfoTextProperty, value); }
        }

        private async void Foo()
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                LogText.Text += i.ToString() + "\r\n";
                await Task.Delay(1);
            }
        }

        private void UploadContextButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void GetContextButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

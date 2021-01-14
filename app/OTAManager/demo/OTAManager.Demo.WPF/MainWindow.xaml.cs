using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
using System.Windows.Threading;
using Newtonsoft.Json;
using OTAManager.ClientUpdateCore;
using OTAManager.Server.Controllers;

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

        /// <summary>
        /// 下载文件列表内容
        /// </summary>
        /// 格式是 文件名|下载链接|md5 这里使用电信的下载链接测试
        public static readonly DependencyProperty ClientApplicationFileInfoTextProperty = DependencyProperty.Register(
            "ClientApplicationFileInfoText", typeof(string), typeof(MainWindow), new PropertyMetadata("Installer.exe|https://10000.gd.cn/10000.gd_speedtest.exe|9f650f3eb7be0a8e82efeb822f53f13a"));


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

        private ApplicationUpdateManager ApplicationUpdateManager { get; } = new ApplicationUpdateManager(Host);

        private async void UploadContextButton_OnClick(object sender, RoutedEventArgs e)
        {
            var clientUpdateManifest = new ClientUpdateManifest()
            {
                Name = ApplicationName,
                InstallerArgument = InstallerArgument,
                InstallerFileName = InstallerFileName,
                ClientApplicationFileInfoList =
                    new List<ClientApplicationFileInfo>(ParseClientApplicationFileInfoText()),
            };

            if (string.IsNullOrEmpty(Version) || !System.Version.TryParse(Version, out var applicationVersion))
            {
                Log($"无法转换 Version={Version} 为版本号");
                return;
            }

            var responseApplicationUpdateInfoModel = await ApplicationUpdateManager.UpdateServerInfo(
                new ApplicationUpdateContext()
                {
                    ApplicationId = ApplicationId,
                    ApplicationVersion = applicationVersion,
                    ClientUpdateManifest = clientUpdateManifest
                });


            ParseApplicationUpdateInfoModel(responseApplicationUpdateInfoModel);
        }

        private void ParseApplicationUpdateInfoModel(ApplicationUpdateContext model)
        {
            ApplicationId = model.ApplicationId;
            Version = model.ApplicationVersion.ToString();

            var clientUpdateManifest = model.ClientUpdateManifest;

            if (clientUpdateManifest == null)
            {
                Log("用户下载信息是空");
                return;
            }

            ApplicationName = clientUpdateManifest.Name;
            InstallerFileName = clientUpdateManifest.InstallerFileName;
            InstallerArgument = clientUpdateManifest.InstallerArgument;
            ClientApplicationFileInfoText = string.Join("\r\n",
                clientUpdateManifest.ClientApplicationFileInfoList.Select(temp =>
                    $"{temp.FilePath}|{temp.DownloadUrl}|{temp.Md5}"));
        }

        public const string Host = "http://localhost:5000";

        private IEnumerable<ClientApplicationFileInfo> ParseClientApplicationFileInfoText()
        {
            var clientApplicationFileInfoText = ClientApplicationFileInfoText;

            if (string.IsNullOrEmpty(clientApplicationFileInfoText))
            {
                Log($"没有 ClientApplicationFileInfoText 内容文件");
                yield break;
            }

            var list = clientApplicationFileInfoText.Split("\n").Select(temp => temp.Replace("\r", ""))
                .Where(temp => !string.IsNullOrEmpty(temp)).ToList();
            foreach (var text in list)
            {
                var textList = text.Split("|");
                yield return new ClientApplicationFileInfo()
                {
                    FilePath = textList[0], DownloadUrl = textList[1], Md5 = textList[2],
                };
            }
        }

        private async void GetContextButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!System.Version.TryParse(Version, out var currentVersion))
            {
                currentVersion = new Version();
            }

            var applicationUpdateContext =
                await ApplicationUpdateManager.GetUpdate(
                    new ApplicationUpdateInfoRequest(ApplicationId, currentVersion));

            ParseApplicationUpdateInfoModel(applicationUpdateContext);
        }

        private void Log(string message)
        {
            Dispatcher.InvokeAsync(() => LogText.Text += message + "\r\n", DispatcherPriority.Send);
        }
    }
}

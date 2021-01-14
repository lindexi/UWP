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

        private async void UploadContextButton_OnClick(object sender, RoutedEventArgs e)
        {
            var httpClient = GetClient();

            var clientUpdateManifest = new ClientUpdateManifest()
            {
                Name = ApplicationName,
                InstallerArgument = InstallerArgument,
                InstallerFileName = InstallerFileName,
                ClientApplicationFileInfoList = new List<ClientApplicationFileInfo>(ParseClientApplicationFileInfoText()),
            };

            var clientUpdateManifestSerializer = new ClientUpdateManifestSerializer();
            var serializedClientUpdateManifest = clientUpdateManifestSerializer.Serialize(clientUpdateManifest);

            var applicationUpdateInfo = new ApplicationUpdateInfoModel()
            {
                ApplicationId = ApplicationId,
                Version = Version,
                UpdateContext = serializedClientUpdateManifest
            };

            var response = await httpClient.PutAsJsonAsync("/UpdateManager", applicationUpdateInfo);

            var responseApplicationUpdateInfoModel = await response.Content.ReadFromJsonAsync<ApplicationUpdateInfoModel>();
            ParseApplicationUpdateInfoModel(responseApplicationUpdateInfoModel);
        }

        private void ParseApplicationUpdateInfoModel(ApplicationUpdateInfoModel model)
        {
            ApplicationId = model.ApplicationId;
            Version = model.Version;

            if (string.IsNullOrEmpty(model.UpdateContext))
            {
                Log($"没有找到 ApplicationId={ApplicationId} 的 UpdateContext 方法");
                return;
            }

            var clientUpdateManifestSerializer = new ClientUpdateManifestSerializer();
            var clientUpdateManifest = clientUpdateManifestSerializer.Deserialize(model.UpdateContext);

            ApplicationName = clientUpdateManifest.Name;
            InstallerFileName = clientUpdateManifest.InstallerFileName;
            InstallerArgument = clientUpdateManifest.InstallerArgument;
            ClientApplicationFileInfoText = string.Join("\r\n",
                clientUpdateManifest.ClientApplicationFileInfoList.Select(temp =>
                    $"{temp.FilePath}|{temp.DownloadUrl}|{temp.Md5}"));
        }


        private static HttpClient GetClient()
        {
            return new HttpClient()
            {
                BaseAddress = new Uri(Host)
            };
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

            var list = clientApplicationFileInfoText.Split("\n").Select(temp => temp.Replace("\r", "")).Where(temp => !string.IsNullOrEmpty(temp)).ToList();
            foreach (var text in list)
            {
                var textList = text.Split("|");
                yield return new ClientApplicationFileInfo()
                {
                    FilePath = textList[0],
                    DownloadUrl = textList[1],
                    Md5 = textList[2],
                };
            }
        }

        private async void GetContextButton_OnClick(object sender, RoutedEventArgs e)
        {
            var httpClient = GetClient();
            var applicationUpdateInfoText = await httpClient.GetStringAsync($"/UpdateManager?applicationId={ApplicationId}");

            if (string.IsNullOrEmpty(applicationUpdateInfoText))
            {
                Log($"找不到应用为 ApplicationId={ApplicationId} 的信息");
                return;
            }

            var applicationUpdateInfoModel = JsonConvert.DeserializeObject<ApplicationUpdateInfoModel>(applicationUpdateInfoText);

            ParseApplicationUpdateInfoModel(applicationUpdateInfoModel);
        }

        private void Log(string message)
        {
            Dispatcher.InvokeAsync(() => LogText.Text += message + "\r\n", DispatcherPriority.Send);
        }
    }
}

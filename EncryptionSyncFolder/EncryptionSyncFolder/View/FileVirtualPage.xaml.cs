// lindexi
// 15:58

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EncryptionSyncFolder.Model;
using EncryptionSyncFolder.ViewModel;
using XmlNodeList = Windows.Data.Xml.Dom.XmlNodeList;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace EncryptionSyncFolder.View
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FileVirtualPage : Page
    {
        public FileVirtualPage()
        {
            this.InitializeComponent();
            DataContext = View;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Action onNotAccount = e.Parameter as Action;
            //如果没有登录
            if (!Account.AccountVirtual.AreAccountConfirm)
            {
                new Task(async () =>
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            onNotAccount?.Invoke();
                        });
                }).Start();

                //提示
                ToastText("用户没有登录");
            }
            base.OnNavigatedTo(e);
        }

        private FileVirtualModel View
        {
            set;
            get;
        } = new FileVirtualModel();

        private void ToastText(string str)
        {
            var toastText = ToastTemplateType.ToastText01;
            var content = ToastNotificationManager.GetTemplateContent(toastText);
            XmlNodeList xml = content.GetElementsByTagName("text");
            xml[0].AppendChild(content.CreateTextNode(str));
            ToastNotification toast = new ToastNotification(content);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void Rename_OnClick(object sender, RoutedEventArgs e)
        {
            View.Rename();
        }

        private void ToFolder_OnClick(object sender, RoutedEventArgs e)
        {
            View.ToFolder();
        }
    }
}
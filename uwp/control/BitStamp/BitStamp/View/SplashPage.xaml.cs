// lindexi
// 11:11

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BitStamp.ViewModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace BitStamp.View
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SplashPage : Page
    {
        public SplashPage()
        {
            this.InitializeComponent();

            //ApplicationView.PreferredLaunchViewSize = new Size(200, 1000);

            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            //ApplicationView.GetForCurrentView().TryResizeView(new Size(200, 1000));

            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Read();
            base.OnNavigatedTo(e);
        }

        private async void Read()
        {
            await AccoutGoverment.AccountModel.Read();
            AccoutGoverment.AccountModel.OnReadEventHandler += async (s, e) =>
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        Frame rootFrame = new Frame();
                        rootFrame.Navigate(typeof(MainPage));
                        Window.Current.Content = rootFrame;
                    });
            };
        }
    }
}
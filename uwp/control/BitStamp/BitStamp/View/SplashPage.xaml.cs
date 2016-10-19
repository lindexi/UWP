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

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                // 需要引用Windows Mobile Extensions for the UWP
                StatusBar sb = StatusBar.GetForCurrentView();
                // 背景色设置为需要颜色
                sb.BackgroundColor = Color.FromArgb(255, 100, 149, 237);
                sb.BackgroundOpacity = 1;
            }

            // 针对desktop
            ApplicationView appView = ApplicationView.GetForCurrentView();
            ApplicationViewTitleBar titleBar = appView.TitleBar;
            // 背景色设置为需要颜色
            Color bc = Color.FromArgb(255, 100, 149, 237);
            titleBar.BackgroundColor = bc;
            titleBar.InactiveBackgroundColor = bc;
            // 按钮背景色按需进行设置
            titleBar.ButtonBackgroundColor = bc;
            titleBar.ButtonHoverBackgroundColor = bc;
            titleBar.ButtonPressedBackgroundColor = bc;
            titleBar.ButtonInactiveBackgroundColor = bc;

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

        //void PositionImage()
        //{
        //    // desktop
        //    if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily.Equals("windows.desktop", StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
        //        extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
        //        extendedSplashImage.Height = splashImageRect.Height;
        //        extendedSplashImage.Width = splashImageRect.Width;
        //    }
        //    // mobile
        //    else if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily.Equals("windows.mobile", StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        // 获取一个值，该值表示每个视图（布局）像素的原始（物理）像素数。
        //        double density = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

        //        extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X / density);
        //        extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y / density);
        //        extendedSplashImage.Height = splashImageRect.Height / density;
        //        extendedSplashImage.Width = splashImageRect.Width / density;
        //    }
        //    // xbox等没试过，编不出来
        //    else
        //    {
        //    }
        //}
    }
}
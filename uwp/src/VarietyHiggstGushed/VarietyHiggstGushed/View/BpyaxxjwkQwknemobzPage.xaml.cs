using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VarietyHiggstGushed.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VarietyHiggstGushed.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BpyaxxjwkQwknemobzPage : Page
    {
        public BpyaxxjwkQwknemobzPage()
        {
            this.InitializeComponent();
            Read();
        }

        private async void Read()
        {
            await AccountGoverment.JwAccountGoverment.Read();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var frame = (Frame) Window.Current.Content;
            frame.Navigate(typeof(StockPage));
        }

        private async void DxpoihQprdqbip_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await AccountGoverment.JwAccountGoverment.ReadJwStorage())
            {
                await new MessageDialog("没有找到存档", "没有存档").ShowAsync();
                return;
            }
            var frame = (Frame) Window.Current.Content;
            frame.Navigate(typeof(StockPage));
        }

        private async void YxbrbfgEakhybi_OnClick(object sender, RoutedEventArgs e)
        {
                await new MessageDialog("开源游戏 https://github.com/lindexi/UWP/tree/master/uwp/src/VarietyHiggstGushed", "关于").ShowAsync();
        }
    }
}

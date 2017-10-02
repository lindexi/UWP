using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    [lindexi.uwp.Framework.ViewModel.ViewModel(typeof(TvrwgrnNnuModel))]
    public sealed partial class BpyaxxjwkQwknemobzPage : Page
    {
        public BpyaxxjwkQwknemobzPage()
        {
            this.InitializeComponent();
        }

        public TvrwgrnNnuModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (TvrwgrnNnuModel) e.Parameter;
            base.OnNavigatedTo(e);
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AdraqbqhUgtwg();
        }

        private void DxpoihQprdqbip_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DxpoihQprdqbip();
        }

        private async void YxbrbfgEakhybi_OnClick(object sender, RoutedEventArgs e)
        {
            await new MessageDialog("开源游戏 https://github.com/lindexi/UWP/tree/master/uwp/src/VarietyHiggstGushed", "关于")
            {
                Options = MessageDialogOptions.None,
                CancelCommandIndex = 1,
                Commands =
                {
                    new UICommand("打开链接")
                    {
                        Invoked = async command =>
                        {
                            await Launcher.LaunchUriAsync(new Uri(
                                "https://github.com/lindexi/UWP/tree/master/uwp/src/VarietyHiggstGushed"));
                        }
                    },
                    new UICommand("关闭")
                }
            }.ShowAsync();

        }
    }
}

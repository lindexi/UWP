using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace ScalableVectorGraphic
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            View=new ViewModel.ViewModel();
            this.InitializeComponent();
            DataContext = View;
            Svgimage();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Svg.SafeUnload();
            base.OnNavigatingFrom(e);
        }

        private async void Svgimage()
        {
            var file =await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///assets//weather_sun.svg"));
            await Svg.LoadFileAsync(file);
        }

        private ViewModel.ViewModel View { set; get; }
    }
}

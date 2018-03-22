using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BitStamp.ViewModel;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BitStamp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HrbHtlad : Page
    {
        public HrbHtlad()
        {
            this.InitializeComponent();
        }

        public HrbHtladModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (HrbHtladModel) e.Parameter;
            DataContext = ViewModel;

            base.OnNavigatedTo(e);
        }
    }
}

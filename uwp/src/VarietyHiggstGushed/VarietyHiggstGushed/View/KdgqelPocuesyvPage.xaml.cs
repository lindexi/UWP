using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    [lindexi.uwp.Framework.ViewModel.ViewModel(ViewModel=typeof(KdgderhlMzhpModel))]
    public sealed partial class KdgqelPocuesyvPage : Page
    {
        public KdgqelPocuesyvPage()
        {
            this.InitializeComponent();
        }

        public KdgderhlMzhpModel ViewModel { get; set; }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (KdgderhlMzhpModel) e.Parameter;
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }
    }
}

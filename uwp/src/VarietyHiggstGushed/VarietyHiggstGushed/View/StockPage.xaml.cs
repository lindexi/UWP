using System;
using System.Collections.Generic;
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
using lindexi.uwp.Framework.ViewModel;
using VarietyHiggstGushed.Model;
using VarietyHiggstGushed.ViewModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace VarietyHiggstGushed.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    [ViewModel(typeof(StorageModel))]
    public sealed partial class StockPage : Page
    {
        public StockPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            View = (StorageModel) e.Parameter;
            DataContext = View;

            base.OnNavigatedTo(e);
        }

        private StorageModel View
        {
            set;
            get;
        }

        private async void ListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //点击的时候，输入是否要买还是要卖

            View.CarloPiperIsaacProperty = (WqmnygDcxwptivk) e.ClickedItem;
            var temp = new JediahPage()
            {
                ViewModel = View,
            };
            ContentDialog contentDialog = new ContentDialog()
            {
                Content = temp,
                IsPrimaryButtonEnabled = false,
                IsSecondaryButtonEnabled = false,
            };

            temp.Close += (s, args) => contentDialog.Hide();

            await contentDialog.ShowAsync();
        }
    }

    public class StrInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return 0;
                }
                if (int.TryParse(str, out int temp))
                {
                    return temp;
                }
                return 0;
            }
            return 0;
        }
    }
}

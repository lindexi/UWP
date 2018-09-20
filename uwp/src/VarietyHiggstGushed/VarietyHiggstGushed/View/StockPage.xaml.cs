using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using lindexi.MVVM.Framework.ViewModel;
using VarietyHiggstGushed.Model;
using VarietyHiggstGushed.ViewModel;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace VarietyHiggstGushed.View
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    [ViewModel(typeof(StorageModel))]
    public sealed partial class StockPage : Page
    {
        public StockPage()
        {
            InitializeComponent();
        }

        private StorageModel View { set; get; }

        private async void ListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //点击的时候，输入是否要买还是要卖

            View.CarloPiperIsaacProperty = (WqmnygDcxwptivk) e.ClickedItem;
            var temp = new JediahPage
            {
                ViewModel = View
            };
            var contentDialog = new ContentDialog
            {
                Content = temp,
                IsPrimaryButtonEnabled = false,
                IsSecondaryButtonEnabled = false
            };

            temp.Close += (s, args) => contentDialog.Hide();

            await contentDialog.ShowAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            View = (StorageModel) e.Parameter;
            DataContext = View;

            base.OnNavigatedTo(e);
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

                if (int.TryParse(str, out var temp))
                {
                    return temp;
                }

                return 0;
            }

            return 0;
        }
    }
}
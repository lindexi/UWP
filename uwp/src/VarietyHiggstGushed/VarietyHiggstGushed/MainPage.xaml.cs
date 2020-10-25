using System;
using lindexi.uwp.Framework;
using VarietyHiggstGushed.Model;
using VarietyHiggstGushed.ViewModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace VarietyHiggstGushed
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            ViewModel = new IckixyYofiModel();
            InitializeComponent();
            ViewModel.Content = (NavigateFrame) RuvJruhditrj;
        }

        public IckixyYofiModel ViewModel { get; set; }

        private AjuvqrDqsoljna _ajuvqrDqsoljna;

        private bool _ajuvqrDqsoljnaMkfsjNiydfobt;

        private void ViewModel_LyfxkdxmSzjd(object sender, string e)
        {
            LyfxkdxmSzjd.Text = e;
            LyfxkdxmSzjd.Visibility = Visibility;
            LyfxkdxmSzjd.Opacity = 1;

            var sb = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(2))
            };
            Storyboard.SetTargetName(animation, nameof(LyfxkdxmSzjd));
            Storyboard.SetTargetProperty(animation, "Opacity");
            sb.Children.Add(animation);
            sb.Completed += (o, ee) =>
            {
                LyfxkdxmSzjd.Visibility = Visibility.Collapsed;
                _ajuvqrDqsoljnaMkfsjNiydfobt = false;
            };
            Storyboard.SetTarget(animation, LyfxkdxmSzjd);
            sb.Begin();
        }

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            FjyhtrOcbhzjwi.Fhnazmoul.Request();
            e.Handled = true;
        }

        private void FiontwzNdqd()
        {
            _ajuvqrDqsoljna = new AjuvqrDqsoljna(fjyhtrOcbhzjwi =>
            {
                //双击退出
                if (fjyhtrOcbhzjwi.Handle)
                {
                    _ajuvqrDqsoljnaMkfsjNiydfobt = false;
                    return;
                }

                if (_ajuvqrDqsoljnaMkfsjNiydfobt)
                {
                    Application.Current.Exit();
                }
                else
                {
                    ViewModel_LyfxkdxmSzjd(this, "再次点击退出");
                    _ajuvqrDqsoljnaMkfsjNiydfobt = true;
                }
            });
            FjyhtrOcbhzjwi.Fhnazmoul.AddSuccessor(_ajuvqrDqsoljna);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.LyfxkdxmSzjd += ViewModel_LyfxkdxmSzjd;
            ViewModel.NavigatedTo(this, e);
            base.OnNavigatedTo(e);
            //启动后退关闭
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
            FiontwzNdqd();
        }
    }
}

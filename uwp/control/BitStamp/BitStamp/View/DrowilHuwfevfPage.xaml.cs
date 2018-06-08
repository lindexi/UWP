using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BitStamp.View;
using BitStamp.ViewModel;
using lindexi.uwp.Framework.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace BitStamp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DrowilHuwfevfPage : Page
    {
        public DrowilHuwfevfPage()
        {
            this.InitializeComponent();

            ViewModel = (DrowilHuwfevfModel) DataContext;

            ViewModel.Content = KasibkqeStkxaij;

            ViewModel.ViewModel = new List<ViewModelPage>()
            {
                new ViewModelPage(typeof(HrbHtladModel), typeof(HrbHtlad)),
                new ViewModelPage(typeof(SaeHqeupqModel), typeof(SaeHqeupqPage)),
            };

            Loaded += (s, e) => { ViewModel.NavigatedTo(this, null); };

            KasibkqeStkxaij.Navigated += HgySmdwraxp;
        }

        private void HgySmdwraxp(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                e.SourcePageType == typeof(SaeHqeupqPage)
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;

            NavigatHrmfTpu = true;
        }


        public DrowilHuwfevfModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SystemNavigationManager telTtxxskne = SystemNavigationManager.GetForCurrentView();
            telTtxxskne.BackRequested += BackRequested;

            telTtxxskne.AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Collapsed;

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            var dmbyzkfscDycoue = ApplicationView.GetForCurrentView();

            dmbyzkfscDycoue.TitleBar.BackgroundColor = Colors.Black;

            dmbyzkfscDycoue.TitleBar.ButtonBackgroundColor = Colors.Transparent;

            ApplicationView.PreferredLaunchViewSize = new Size(1024, 800);

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            dmbyzkfscDycoue.SetPreferredMinSize(new Size(1024, 800));
        }

        /// <summary>
        /// 跳转完成
        /// </summary>
        private bool NavigatHrmfTpu { get; set; } = true;

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (NavigatHrmfTpu)
            {
                NavigatHrmfTpu = false;
                ViewModel.NavigateHrbHtlad();
            }

            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            //    AppViewBackButtonVisibility.Collapsed;
        }

        private void NavigateSaeHqeupq_OnClick(object sender, RoutedEventArgs e)
        {
            if (NavigatHrmfTpu)
            {
                NavigatHrmfTpu = false;
                ViewModel.NavigateSaeHqeupq();
            }

            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            //    AppViewBackButtonVisibility.Visible;
        }
    }
}
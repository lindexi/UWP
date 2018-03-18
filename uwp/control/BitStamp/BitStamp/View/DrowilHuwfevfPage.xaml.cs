using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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

            KasibkqeStkxaij.Navigate(typeof(BitStamp.HrbHtlad));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SystemNavigationManager telTtxxskne = SystemNavigationManager.GetForCurrentView();
            telTtxxskne.BackRequested += BackRequested;

            telTtxxskne.AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Visible;

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            var dmbyzkfscDycoue = ApplicationView.GetForCurrentView();

            dmbyzkfscDycoue.TitleBar.BackgroundColor = Colors.Black;

            dmbyzkfscDycoue.TitleBar.ButtonBackgroundColor = Colors.Transparent;

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(1024, 800));
        }

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
    }
}
using lindexi.MVVM.Framework.ViewModel;
using lindexi.uwp.Framework;
using VarietyHiggstGushed.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VarietyHiggstGushed.View
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    [ViewModel(ViewModel = typeof(KdgderhlMzhpModel))]
    public sealed partial class KdgqelPocuesyvPage : Page
    {
        public KdgqelPocuesyvPage()
        {
            InitializeComponent();
        }

        public KdgderhlMzhpModel ViewModel { get; set; }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (KdgderhlMzhpModel) e.Parameter;
            ViewModel.Content = (NavigateFrame) VjagWrgesebmy;
            ViewModel.UmfqawovKaxkrdrg();
            DataContext = ViewModel;
            base.OnNavigatedTo(e);
        }
    }
}

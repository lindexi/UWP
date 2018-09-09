using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using lindexi.uwp.Framework.ViewModel;
using TpwlxnpDfyecpeoh.ViewModel;

namespace TpwlxnpDfyecpeoh.View
{
    /// <summary>
    /// HnlcDbtdhsdjPage.xaml 的交互逻辑
    /// </summary>
    [ViewModel(ViewModel = typeof(HnlcDbtdhsdjModel))]
    public partial class HnlcDbtdhsdjPage : Page
    {
        public HnlcDbtdhsdjPage()
        {
            InitializeComponent();
        }

        public HnlcDbtdhsdjModel ViewModel { get; set; }

        private void HzmzKgeu_OnClick(object sender, RoutedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)sender;

            if (frameworkElement.DataContext is DexqurhctSjyfozae dexqurhctSjyfozae)
            {
                ViewModel.KdfoeDoct(dexqurhctSjyfozae);
            }
        }

        private void DlsuqHmopxh_OnClick(object sender, RoutedEventArgs e)
        {
            var frameworkElement = (FrameworkElement)sender;

            if (frameworkElement.DataContext is IKdgvtziaSfs kdgvtziaSfs)
            {
                kdgvtziaSfs.DdwTynktxyx();
            }
        }
    }
}

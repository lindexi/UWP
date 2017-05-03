using System;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls;
#elif wpf
using System.Windows.Controls;
#endif


namespace lindexi.uwp.Framework.ViewModel
{
    public interface INavigato: IViewModel
    {
        Frame Content { set; get; }

        void Navigate(Type viewModel, object parameter);
    }
}
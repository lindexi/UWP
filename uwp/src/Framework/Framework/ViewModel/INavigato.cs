using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;


namespace lindexi.uwp.Framework.ViewModel
{
    public interface INavigato
    {
        Frame Content { set; get; }

        void Navigate(Type viewModel, object parameter);
    }
}
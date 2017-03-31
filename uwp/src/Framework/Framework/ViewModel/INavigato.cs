using System;
using Windows.UI.Xaml.Controls;

namespace Framework.ViewModel
{
    public interface INavigato
    {
        Frame Content
        {
            set;
            get;
        }

        void Navigate(Type viewModel, object parameter);
    }
}
using System;
using Windows.UI.Xaml.Controls;

namespace ImageMoseClick.ViewModel
{
    public interface INavigato
    {
        Frame Content
        {
            set;
            get;
        }

        void Navigateto(Type viewModel, object parameter);
    }
}
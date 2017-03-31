using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Framework.ViewModel
{
    public abstract class NavigateViewModel : ViewModelBase, INavigato
    {
        public Frame Content
        {
            set;
            get;
        }

        public ViewModelBase this[string str]
        {
            get { return ViewModel.FirstOrDefault(temp => temp.Key == str)?.ViewModel; }
        }

        public List<ViewModelPage> ViewModel
        {
            set;
            get;
        } 

        public async void Navigate(Type viewModel, object paramter)
        {
            _viewModel?.OnNavigatedFrom(null);
            ViewModelPage view = ViewModel.Find(temp =>temp.Equals(viewModel));
            await view.Navigate(Content, paramter);
            _viewModel = view.ViewModel;
        }
        //当前ViewModel
        private ViewModelBase _viewModel;
    }
}
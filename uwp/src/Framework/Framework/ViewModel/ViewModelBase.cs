// lindexi

using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Framework.ViewModel
{
    public abstract class ViewModelBase : NotifyProperty, INavigable, INavigato
    {
        public List<ViewModelPage> ViewModel
        {
            set;
            get;
        } = new List<ViewModelPage>();

        public Frame Content
        {
            set;
            get;
        }

        public abstract void OnNavigatedFrom(object obj);
        public abstract void OnNavigatedTo(object obj);

        public async void Navigateto(Type viewModel, object paramter)
        {
            _viewModel?.OnNavigatedFrom(null);
            ViewModelPage view = ViewModel.Find(temp => temp.ViewModel.GetType() == viewModel);
            await view.Navigate(Content, paramter);
            _viewModel = view.ViewModel;
        }
        
        //当前ViewModel
        private ViewModelBase _viewModel;
    }
}
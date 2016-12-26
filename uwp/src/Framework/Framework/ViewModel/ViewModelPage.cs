// lindexi

using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Framework.ViewModel
{
    public class ViewModelPage
    {
        //public ViewModelPage()
        //{
        //    //if (ViewModel == null)
        //    //{
        //    //    //ViewModel=View.GetConstructor(null)
        //    //}
        //}

        public ViewModelPage(Type viewModel, Type page)
        {
            _viewModel = viewModel;
            Page = page;
            Key = _viewModel.Name;
        }

        public ViewModelPage(ViewModelBase viewModel, Type page)
        {
            ViewModel = viewModel;
            Page = page;
            Key = viewModel.GetType().Name;
            _viewModel = viewModel.GetType();
        }

        public string Key
        {
            set;
            get;
        }


        public ViewModelBase ViewModel
        {
            set;
            get;
        }

        public Type Page
        {
            set;
            get;
        }

        public async Task Navigate(Frame content, object paramter)
        {
            if (ViewModel == null)
            {
                ViewModel = (ViewModelBase) _viewModel.GetConstructor(Type.EmptyTypes).Invoke(null);
            }
            ViewModel.OnNavigatedTo(paramter);
#if NOGUI
            return;
#endif
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    content.Navigate(Page);
                });
        }

        private Type _viewModel;

    }
}
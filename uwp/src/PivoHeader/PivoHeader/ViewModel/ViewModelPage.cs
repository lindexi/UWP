// lindexi

using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace PivoHeader.ViewModel
{
    public class ViewModelPage
    {
        public ViewModelPage()
        {
            //if (ViewModel == null)
            //{
            //    //ViewModel=View.GetConstructor(null)
            //}
        }


        public ViewModelPage(ViewModelBase viewModel, Type page)
        {
            ViewModel = viewModel;
            Page = page;
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
                ViewModel = (ViewModelBase) View.GetConstructor(Type.EmptyTypes).Invoke(null);
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

        private Type View
        {
            set;
            get;
        }
    }
}

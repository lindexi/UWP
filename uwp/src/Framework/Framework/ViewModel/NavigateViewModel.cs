using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if WINDOWS_UWP
#elif wpf
#endif

#if WINDOWS_UWP
using Windows.UI.Xaml.Controls;
#elif wpf
using System.Windows.Controls;
#endif

namespace lindexi.uwp.Framework.ViewModel
{
    public abstract class NavigateViewModel : ViewModelBase, INavigato
    {
        //当前ViewModel
        private ViewModelBase _viewModel;

        public ViewModelBase this[string str]
        {
            get
            {
                return ViewModel.FirstOrDefault(temp => temp.Key == str)?.ViewModel;
            }
        }

        public List<ViewModelPage> ViewModel
        {
            set; get;
        }

        //public List<Composite> Composite { set; get; }

        public Frame Content
        {
            set; get;
        }

        public async void Navigate(Type viewModel, object paramter)
        {
            _viewModel?.OnNavigatedFrom(this, null);
            var send = _viewModel as ISendMessage;
            if (send?.SendMessageHandler != null)
            {
                send.SendMessageHandler -= (sender, message) => ReceiveMessage(sender, message);
            }
            await Navigate(viewModel, paramter, Content);
            ViewModelPage view = ViewModel.Find(temp => temp.Equals(viewModel));
            _viewModel = view.ViewModel;
        }

        public async Task Navigate(Type viewModel, object paramter,Frame content)
        {
            ViewModelPage view = ViewModel.Find(temp => temp.Equals(viewModel));
            await Navigate(paramter, view, content);
        }

        private async Task Navigate(object paramter, ViewModelPage view, Frame content)
        {
            await view.Navigate(content, paramter);

            ISendMessage send = view.ViewModel;
            if (send != null)
            {
                send.SendMessageHandler += (sender, message) => ReceiveMessage(sender, message);
            }
        }

        //public virtual void ReceiveMessage(object sender, Message message)
        //{
        //}
    }
}
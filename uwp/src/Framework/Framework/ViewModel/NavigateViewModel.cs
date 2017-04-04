using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace lindexi.uwp.Framework.ViewModel
{
    public abstract class NavigateViewModel : ViewModelBase, INavigato, IReceiveMessage
    {
        //当前ViewModel
        private ViewModelBase _viewModel;

        public ViewModelBase this[string str]
        {
            get { return ViewModel.FirstOrDefault(temp => temp.Key == str)?.ViewModel; }
        }

        public List<ViewModelPage> ViewModel { set; get; }

        public List<Composite> Composite { set; get; }

        public Frame Content { set; get; }

        public async void Navigate(Type viewModel, object paramter)
        {
            _viewModel?.OnNavigatedFrom(this, null);
            var send = _viewModel as ISendMessage;
            if (send?.SendMessageHandler != null)
            {
                send.SendMessageHandler -= ReceiveMessage;
            }

            ViewModelPage view = ViewModel.Find(temp => temp.Equals(viewModel));
            await view.Navigate(Content, paramter);

            send = view.ViewModel as ISendMessage;
            if (send != null)
            {
                send.SendMessageHandler += ReceiveMessage;
            }
            _viewModel = view.ViewModel;
        }

        public virtual void ReceiveMessage(object sender, Message message)
        {
        }
    }
}
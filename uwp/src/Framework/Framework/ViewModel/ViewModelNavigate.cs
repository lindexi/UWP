using System;
using lindexi.uwp.Framework.ViewModel;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    ///     用来跳转 ViewModel 的类
    /// </summary>
    public class ViewModelNavigate
    {
        /// <inheritdoc />
        public ViewModelNavigate(INavigateFrame frame)
        {
            Frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        /// <summary>
        /// 用来跳转的类
        /// </summary>
        public INavigateFrame Frame { get; set; }

        /// <summary>
        ///     跳转到指定的 <paramref name="viewModelPage" /> 添加消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="viewModelPage"></param>
        /// <param name="parameter"></param>
        public void Navigate(INavigateable sender, ViewModelPage viewModelPage, object parameter = null)
        {
            if (viewModelPage == null)
            {
                throw new ArgumentNullException(nameof(viewModelPage));
            }

            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            var receiveMessage = sender as IReceiveMessage;


            var lastViewModel = _lastViewModelPage?.ViewModel?.GetViewModel();
            if (lastViewModel is INavigable lastNavigableViewModel)
            {
                lastNavigableViewModel.NavigatedFrom(sender, parameter);
            }

            if (receiveMessage != null && lastViewModel is ISendMessage lastSendMessage)
            {
                lastSendMessage.Send -= receiveMessage.ReceiveMessage;
            }


            // 开始跳转
            var viewModel = viewModelPage.ViewModel.GetViewModel();
            if (viewModel == null)
            {
                throw new ArgumentException("无法从ViewModel.GetViewModel拿到ViewModel值");
            }

            if (receiveMessage != null)
            {
                if (viewModel is ISendMessage sendMessage)
                {
                    sendMessage.Send -= receiveMessage.ReceiveMessage;
                    sendMessage.Send += receiveMessage.ReceiveMessage;
                }
            }

            if (viewModel is INavigable navigable)
            {
                navigable.NavigatedTo(sender, parameter);
            }

            Frame.Navigate(viewModelPage.Page, viewModel);

            _lastViewModelPage = viewModelPage;
        }

        private ViewModelPage _lastViewModelPage;
    }
}

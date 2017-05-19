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
    /// <summary>
    /// 包含有页面的ViewModel
    /// </summary>
    public abstract class NavigateViewModel : ViewModelMessage, INavigato
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

        /// <summary>
        /// 包含的页面集合
        /// </summary>
        public List<ViewModelPage> ViewModel
        {
            set; get;
        }


        /// <summary>
        /// 提供跳转的控件
        /// </summary>
        public Frame Content
        {
            set; get;
        }

        /// <summary>
        /// 跳转到页面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="paramter"></param>
        public async void Navigate(Type viewModel, object paramter)
        {
            //_viewModel?.OnNavigatedFrom(this, null);
            _viewModel?.NavigatedFrom(this,null);
            await Navigate(viewModel, paramter, Content);
            ViewModelPage view = ViewModel.Find(temp => temp.Equals(viewModel));
            _viewModel = view.ViewModel;
        }

        /// <summary>
        /// 跳转到页面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="paramter"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task Navigate(Type viewModel, object paramter,Frame content)
        {
            ViewModelPage view = ViewModel.Find(temp => temp.Equals(viewModel));
            await Navigate(paramter, view, content);
        }

        /// <summary>
        /// 跳转到页面
        /// </summary>
        /// <param name="paramter"></param>
        /// <param name="view"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task Navigate(object paramter, ViewModelPage view, Frame content)
        {
            await view.Navigate(content,this, paramter);

            ISendMessage send = view.ViewModel as ViewModelMessage;
            if (send != null)
            {
                send.Send += ReceiveMessage;
            }
        }

      
    }
}
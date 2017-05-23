using System;
using System.Reflection;
using System.Threading.Tasks;

#if WINDOWS_UWP
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#elif wpf
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
#endif



namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 包含 抽象页面和页面
    /// </summary>
    public class ViewModelPage : IEquatable<Type>
    {
        /// <summary>
        /// 页面类型，用于创建页面
        /// </summary>
        private Type _viewModel;

        private ViewModelBase viewModel;

        public ViewModelPage()
        {
            
        }

        public ViewModelPage(Type viewModel)
        {
            _viewModel = viewModel;
            Key = _viewModel.Name;
        }

        public ViewModelPage(Type viewModel, Type page)
        {
            _viewModel = viewModel;
            Page = page;
            Key = _viewModel.Name;
        }

        public ViewModelPage(ViewModelBase viewModel)
        {
            ViewModel = viewModel;
            Key = viewModel.GetType().Name;
            _viewModel = viewModel.GetType();
        }

        public ViewModelPage(ViewModelBase viewModel, Type page)
        {
            ViewModel = viewModel;
            Page = page;
            Key = viewModel.GetType().Name;
            _viewModel = viewModel.GetType();
        }

        /// <summary>
        /// 用于表示页面
        /// </summary>
        public string Key
        {
            set; get;
        }

        /// <summary>
        /// 抽象页面
        /// </summary>
        public ViewModelBase ViewModel
        {
            set
            {
                viewModel = value;
            }
            get
            {
                return viewModel ?? (viewModel = (ViewModelBase)_viewModel.GetConstructor(Type.EmptyTypes).Invoke(null));
            }
        }

        /// <summary>
        /// 页面
        /// </summary>
        public Type Page
        {
            set; get;
        }

        /// <summary>
        /// 判断输入的类型是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Type other)
        {
            return _viewModel == other;
        }

        /// <summary>
        /// 跳转到
        /// </summary>
        /// <param name="content"></param>
        /// <param name="source">从那里跳转</param>
        /// <param name="paramter">参数</param>
        /// <returns></returns>
        public async Task Navigate(Frame content, ViewModelMessage source, object paramter)
        {
            ViewModel.NavigatedTo(source, paramter);
#if NOGUI
            return;
#endif
            try
            {
#if WINDOWS_UWP
                content.Navigate(Page, ViewModel);
#elif wpf
                content.Navigate(Page.GetConstructor(Type.EmptyTypes).Invoke(null) , ViewModel);
#endif

            }
            catch
            {
#if WINDOWS_UWP
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   content.Navigate(Page, ViewModel);
               });
#elif wpf
                SynchronizationContext.SetSynchronizationContext(new
   DispatcherSynchronizationContext(Application.Current.Dispatcher));
                SynchronizationContext.Current.Send(obj =>
                {
                    content.Navigate(Page, ViewModel);
                }, null);
#endif

            }

        }

        public void NavigateFrom(object source, object e = null)
        {
            ViewModel.NavigatedFrom(source, e);
        }

        protected bool Equals(ViewModelPage other)
        {
            return _viewModel == other._viewModel;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((ViewModelPage)obj);
        }

        public override int GetHashCode()
        {
            return _viewModel?.GetHashCode() ?? 0;
        }
    }
}
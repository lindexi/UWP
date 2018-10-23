using System;

#if WINDOWS_UWP&&false
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


namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 包含 抽象页面和页面
    /// </summary>
    public class ViewModelPage : IEquatable<string>
    {
        internal ViewModelPage()
        {
        }

        /// <summary>
        /// 创建包含页面
        /// </summary>
        /// <param name="viewModel"></param>
        public ViewModelPage(INavigatableViewModel viewModel)
        {
            ViewModel = viewModel;
            Key = viewModel.Name;
        }

        /// <summary>
        /// 创建包含页面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="page"></param>
        public ViewModelPage(INavigatableViewModel viewModel, INavigatablePage page)
        {
            ViewModel = viewModel;
            Page = page;
            Key = viewModel.Name;
        }

        /// <summary>
        /// 用于表示页面
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 抽象页面
        /// </summary>
        public INavigatableViewModel ViewModel { private set; get; }

        /// <summary>
        /// 页面
        /// </summary>
        public INavigatablePage Page { get; }


#if false
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
            if (NoGui.NOGUI)
            {
                return;
            }

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
#endif

        //public void NavigateFrom(object source, object e = null)
        //{
        //    ViewModel.NavigatedFrom(source, e);
        //}


        /// <inheritdoc />
        public bool Equals(string other)
        {
            return Key.Equals(other);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals(((ViewModelPage) obj).Key);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (Key != null ? Key.GetHashCode() : 0);
        }
    }
}
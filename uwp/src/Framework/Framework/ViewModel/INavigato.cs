using System;
using lindexi.uwp.Framework.ViewModel;

#if WINDOWS_UWP
using Windows.UI.Xaml.Controls;
#elif wpf
using System.Windows.Controls;
#endif


namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 支持跳转
    /// </summary>
    public interface INavigateable : IViewModel
    {
        /// <summary>
        /// 提供跳转的控件
        /// </summary>
        INavigateFrame Content { set; get; }

        /// <summary>
        /// 跳转到页面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="parameter"></param>
        /// <param name="content"></param>
        void Navigate(Type viewModel, object parameter, INavigateFrame content);

        /// <summary>
        /// 正在跳转事件
        /// </summary>
         event EventHandler<ViewModelPage> Navigating;

        /// <summary>
        /// 跳转完成
        /// </summary>
         event EventHandler<ViewModelPage> Navigated;
    }
}
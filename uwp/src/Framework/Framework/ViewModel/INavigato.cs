using System;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.UI.Xaml.Controls;
#elif wpf
using System.Windows.Controls;
#endif


namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 支持跳转
    /// </summary>
    public interface INavigateable : IViewModel
    {
        /// <summary>
        /// 提供跳转的控件
        /// </summary>
        Frame Content { set; get; }

        /// <summary>
        /// 跳转到页面
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="parameter"></param>
        /// <param name="content"></param>
        void Navigate(Type viewModel, object parameter, Frame content);
    }
}
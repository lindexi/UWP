using System;
using System.Collections.Generic;
using System.Text;
#if wpf
using System.Windows.Navigation;
using Windows.UI.Xaml.Navigation;
#endif
using lindexi.uwp.Framework.ViewModel;

namespace Framework.View
{
    /// <summary>
    /// 框架需要使用的页面
    /// </summary>
    internal class Page<T>
#if WINDOWS_UWP&&false
        : Windows.UI.Xaml.Controls.Page
#endif
        where T : IViewModel
    {
        
        /// <summary>
        /// 获取设置 ViewModel
        /// </summary>
        public T ViewModel
        {
            set => _viewModel = value;
            get => _viewModel;
        }


#if wpf
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _viewModel = (T) e.Parameter;
            DataContext = _viewModel;
            base.OnNavigatedTo(e);
        }
#endif

        private T _viewModel;
    }
}

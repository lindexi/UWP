using System;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 对应的 页面使用对应的 ViewModel 用于反射
    /// </summary>
    public class ViewModelAttribute : Attribute
    {
        /// <summary>
        /// 设置页面对应的 ViewModel 是哪个
        /// </summary>
        public ViewModelAttribute()
        {

        }

        /// <summary>
        /// 设置页面对应的 ViewModel 类型
        /// </summary>
        /// <param name="viewModel"></param>
        public ViewModelAttribute(Type viewModel)
        {
            ViewModel = viewModel;
        }

        /// <summary>
        /// 使用的页面
        /// </summary>
        public Type ViewModel { get; set; }
    }
}

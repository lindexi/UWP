using System;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 对应的 页面使用对应的
    /// </summary>
    public class ViewModelAttribute : Attribute
    {
        /// <summary>
        /// 使用的页面
        /// </summary>
        public Type ViewModel { get; set; }
    }
}
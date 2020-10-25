using lindexi.uwp.Framework.ViewModel;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 可以跳转的 ViewModel 
    /// </summary>
    public interface INavigatableViewModel
    {
        /// <summary>
        /// 设置或获取 ViewModel 的名字
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 获取 ViewModel 类
        /// </summary>
        /// <returns></returns>
        IViewModel GetViewModel();

        /// <summary>
        /// 是否已经加载
        /// </summary>
        bool IsLoaded { get; }
    }
}

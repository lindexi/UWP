using lindexi.MVVM.Framework.ViewModel;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 可跳转
    /// </summary>
    public interface INavigable : IViewModel
    {
        /// <summary>
        ///     不使用这个页面
        ///     清理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="obj"></param>
        void NavigatedFrom(object sender, object obj);

        /// <summary>
        ///     跳转到
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="obj"></param>
        void NavigatedTo(object sender, object obj);
    }

    /// <summary>
    /// 支持是否加载
    /// </summary>
    public interface ILoadableMode : IViewModel
    {
        /// <summary>
        /// 当前的 ViewModel 是否加载
        /// </summary>
        bool IsLoaded { get; }
    }
}

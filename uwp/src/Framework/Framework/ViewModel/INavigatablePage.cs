namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 可跳转的页面
    /// </summary>
    public interface INavigatablePage
    {
        /// <summary>
        /// 平台的页面
        /// </summary>
        object PlatformPage { get; }
    }
}

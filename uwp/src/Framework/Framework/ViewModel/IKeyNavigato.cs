using lindexi.MVVM.Framework.ViewModel;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    ///     使用Key获得ViewModel
    /// </summary>
    public interface IKeyNavigato : INavigateable
    {

        /// <summary>
        /// 跳转到<paramref name="key"/>页面
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parameter"></param>
        /// <param name="content"></param>
        void Navigate(string key, object parameter, INavigateFrame content = null);
    }
}

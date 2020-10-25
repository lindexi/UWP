namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 提供跳转的Frame
    /// </summary>

    public interface INavigateFrame
    {
        /// <summary>
        /// 跳转到指定页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        bool Navigate(INavigatablePage page, object parameter);
    }
}

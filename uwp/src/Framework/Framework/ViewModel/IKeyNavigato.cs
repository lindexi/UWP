namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    ///     使用Key获得ViewModel
    /// </summary>
    public interface IKeyNavigato : INavigateable
    {
        void Navigate(string key, object parameter);
    }
}
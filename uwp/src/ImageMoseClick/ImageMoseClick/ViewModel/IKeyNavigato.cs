namespace ImageMoseClick.ViewModel
{
    /// <summary>
    ///     使用Key获得ViewModel
    /// </summary>
    public interface IKeyNavigato : INavigato
    {
        void Navigateto(string key, object parameter);
    }
}

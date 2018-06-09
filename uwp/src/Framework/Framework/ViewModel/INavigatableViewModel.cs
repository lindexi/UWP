namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 可以跳转的 ViewModel 
    /// </summary>
    public interface INavigatableViewModel
    {
        string Name { get; set; }
        IViewModel GetViewModel();
    }
}
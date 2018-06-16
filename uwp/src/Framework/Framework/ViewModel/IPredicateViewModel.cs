namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 判断指定的 ViewModel 是否符合
    /// </summary>
    public interface IPredicateViewModel
    {
        /// <summary>
        /// 判断viewModel是否符合
        /// </summary>
        /// <returns></returns>
        bool Predicate(IViewModel viewModel);
    }
}
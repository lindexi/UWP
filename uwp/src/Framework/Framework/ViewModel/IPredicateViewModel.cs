namespace lindexi.uwp.Framework.ViewModel
{
    public interface IPredicateViewModel
    {
        /// <summary>
        /// 判断viewModel是否符合
        /// </summary>
        /// <returns></returns>
        bool Predicate(ViewModelPage viewModel);
    }
}
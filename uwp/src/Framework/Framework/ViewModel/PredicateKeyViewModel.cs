namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 通过Key判断ViewModel是否需要
    /// </summary>
    public class PredicateKeyViewModel : IPredicateViewModel
    {
        /// <summary>
        /// 创建判断ViewModel是否需要
        /// </summary>
        /// <param name="key"></param>
        public PredicateKeyViewModel(string key)
        {
            Key = key;
        }

        /// <summary>
        /// 指定ViewModel，在知道是哪个ViewModel使用这个类
        /// </summary>
        public string Key { get; set; }

        /// <inheritdoc />
        public bool Predicate(IViewModel viewModel)
        {
            return viewModel.GetType().Name == Key;
        }
    }
}
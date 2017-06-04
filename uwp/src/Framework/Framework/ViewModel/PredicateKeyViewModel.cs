namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 通过Key判断ViewModel是否需要
    /// </summary>
    public class PredicateKeyViewModel : IPredicateViewModel
    {
        public PredicateKeyViewModel(string key)
        {
            Key = key;
        }

        /// <summary>
        /// 指定ViewModel，在知道是哪个ViewModel使用这个类
        /// </summary>
        public string Key { get; set; }

        /// <inheritdoc />
        public bool Predicate(ViewModelPage viewModel)
        {
            return viewModel.Key == Key;
        }
    }
}
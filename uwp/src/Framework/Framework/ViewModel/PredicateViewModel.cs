using System;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 写规则判断ViewModel是否需要
    /// </summary>
    public class PredicateViewModel:IPredicateViewModel
    {
        private Predicate<ViewModelPage> _predicate;

        public PredicateViewModel(Predicate<ViewModelPage> predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public bool Predicate(ViewModelPage viewModel)
        {
            return _predicate(viewModel);
        }
    }
}
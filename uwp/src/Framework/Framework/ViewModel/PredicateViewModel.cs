using System;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    ///     写规则判断ViewModel是否需要
    /// </summary>
    public class PredicateViewModel : IPredicateViewModel
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        public PredicateViewModel(Predicate<ViewModelPage> predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public bool Predicate(ViewModelPage viewModel)
        {
            return _predicate(viewModel);
        }

        private Predicate<ViewModelPage> _predicate;
    }
}
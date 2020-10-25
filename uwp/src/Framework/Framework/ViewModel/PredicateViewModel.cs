using System;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    ///     写规则判断ViewModel是否需要
    /// </summary>
    public class PredicateViewModel : IPredicateViewModel
    {
        /// <summary>
        /// 自己创建规则判断
        /// </summary>
        /// <param name="predicate"></param>
        public PredicateViewModel(Predicate<IViewModel> predicate)
        {
            _predicate = predicate;
        }

        /// <inheritdoc />
        public bool Predicate(IViewModel viewModel)
        {
            return _predicate(viewModel);
        }

        private readonly Predicate<IViewModel> _predicate;
    }
}

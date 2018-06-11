using System;
using System.Reflection;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    ///     通过继承判断viewmodel是否需要
    /// </summary>
    public class PredicateInheritViewModel : IPredicateViewModel
    {
        /// <inheritdoc />
        public PredicateInheritViewModel(Type key)
        {
            Key = key;
        }

        /// <summary>
        /// 用来判断当前时候符合类型
        /// </summary>
        public Type Key { get; set; }

        /// <inheritdoc />
        public bool Predicate(IViewModel viewModel)
        {
            return Key.IsInstanceOfType(viewModel);
        }
    }
}
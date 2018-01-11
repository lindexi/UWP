using System;
using System.Reflection;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    ///     通过继承判断viewmodel是否需要
    /// </summary>
    public class PredicateInheritViewModel : IPredicateViewModel
    {
        public PredicateInheritViewModel(Type key)
        {
            Key = key;
        }


        public Type Key { get; set; }

        /// <inheritdoc />
        public bool Predicate(ViewModelPage viewModel)
        {
#if wpf
            if (Key.IsInterface)
            {
                return Key.IsAssignableFrom(viewModel.ViewModel.GetType());
            }
            return viewModel.ViewModel.GetType().IsSubclassOf(Key);
#elif WINDOWS_UWP


            return Key.IsAssignableFrom(viewModel.ViewModel.GetType());
#endif
        }
    }
}
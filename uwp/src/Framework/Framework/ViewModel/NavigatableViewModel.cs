using lindexi.MVVM.Framework.Annotations;
using lindexi.uwp.Framework.ViewModel;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <inheritdoc />
    [PublicAPI]
    public class NavigatableViewModel<T> : INavigatableViewModel where T : IViewModel, new()
    {
        /// <inheritdoc />
        public NavigatableViewModel()
        {
            Name = typeof(T).Name;
        }

        /// <inheritdoc />
        public IViewModel GetViewModel()
        {
            if (ViewModel == null)
            {
                ViewModel = new T();
            }

            return ViewModel;
        }

        private T ViewModel { set; get; }

        /// <inheritdoc />
        public string Name { get; set; }
    }
}
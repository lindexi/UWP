using System;
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

        /// <inheritdoc />
        public bool IsLoaded
        {
            get
            {
                if (ViewModel == null)
                {
                    return false;
                }

                var viewModel = ViewModel as ViewModelBase;
                if (viewModel != null)
                {
                    return viewModel.IsLoaded;
                }

                return false;
            }
        }

        private T ViewModel { set; get; }

        /// <inheritdoc />
        public string Name { get; set; }
    }

    /// <summary>
    /// 可以跳转的 ViewModel
    /// </summary>
    /// <inheritdoc />
    [PublicAPI]
    public class NavigatableViewModel : INavigatableViewModel
    {
        /// <inheritdoc />
        public NavigatableViewModel(Type viewModel)
        {
            _viewModel = viewModel;
            Name = viewModel.Name;
        }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public IViewModel GetViewModel()
        {
            if (ViewModel == null)
            {
                ViewModel = (ViewModelBase) Activator.CreateInstance(_viewModel);
            }

            return ViewModel;
        }

        private Type _viewModel;

        private ViewModelBase ViewModel { get; set; }

        /// <inheritdoc />
        public bool IsLoaded => ViewModel?.IsLoaded ?? false;

        /// <summary>
        /// 转换<see cref="Type"/> 为可跳转类
        /// </summary>
        /// <param name="t"></param>
        public static implicit operator NavigatableViewModel(Type t)
        {
            return new NavigatableViewModel(t);
        }
    }
}

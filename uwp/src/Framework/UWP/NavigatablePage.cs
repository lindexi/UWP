using System;
using lindexi.MVVM.Framework.Annotations;
using lindexi.MVVM.Framework.ViewModel;

namespace lindexi.uwp.Framework
{
    /// <summary>
    ///     可以跳转的页面
    /// </summary>
    [PublicAPI]
    public class NavigatablePage : INavigatablePage
    {
        /// <inheritdoc />
        public NavigatablePage(Type page)
        {
            Page = page;
        }

        /// <inheritdoc />
        public object PlatformPage => Page;

        private Type Page { get; }
    }

    /// <summary>
    ///     可以跳转的页面
    /// </summary>
    [PublicAPI]
    public class NavigatablePage<T> : NavigatablePage
    {
        /// <inheritdoc />
        public NavigatablePage() : base(typeof(T))
        {
        }
    }
}
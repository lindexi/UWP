using System;
using System.Windows.Controls;
using lindexi.MVVM.Framework.Annotations;
using lindexi.MVVM.Framework.ViewModel;

namespace lindexi.wpf.Framework
{
    /// <summary>
    ///     可以跳转的页面
    /// </summary>
    [PublicAPI]
    public class NavigatablePage<T> : INavigatablePage where T : Page, new()
    {
        /// <inheritdoc />
        public NavigatablePage()
        {
        }

        /// <inheritdoc />
        public object PlatformPage
        {
            get
            {
                Page page;
                if (_page == null)
                {
                    page = new T();
                    _page = new WeakReference<Page>(page);
                }
                else if (!_page.TryGetTarget(out page))
                {
                    page = new T();
                    _page.SetTarget(page);
                }

                return page;
            }
        }

        private WeakReference<Page> _page;
    }

    /// <summary>
    ///     可以跳转的页面
    /// </summary>
    [PublicAPI]
    public class NavigatablePage : INavigatablePage
    {
        /// <inheritdoc />
        public NavigatablePage(Type page)
        {
            _page = page;
        }

        /// <inheritdoc />
        public object PlatformPage
        {
            get
            {
                Page page;
                if (Page == null)
                {
                    page = (Page) Activator.CreateInstance(_page);
                    Page = new WeakReference<Page>(page);
                }
                else if (!Page.TryGetTarget(out page))
                {
                    page = (Page) Activator.CreateInstance(_page);
                    Page.SetTarget(page);
                }

                return page;
            }
        }

        private Type _page;

        private WeakReference<Page> Page { get; set; }
    }
}
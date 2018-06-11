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
}
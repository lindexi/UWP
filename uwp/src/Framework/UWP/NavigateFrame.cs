using System;
using Windows.UI.Xaml.Controls;
using lindexi.MVVM.Framework.ViewModel;

namespace lindexi.uwp.Framework
{
    /// <inheritdoc />
    public class NavigateFrame : INavigateFrame
    {
        /// <inheritdoc />
        public NavigateFrame(Frame frame)
        {
            Frame = frame;
        }

        /// <summary>显示 Page 实例，支持针对新页面的导航，并保留导航历史记录以支持向前和向后导航。</summary>
        public Frame Frame { get; set; }

        /// <inheritdoc />
        public static implicit operator NavigateFrame(Frame frame)
        {
            return new NavigateFrame(frame);
        }

        /// <inheritdoc />
        public bool Navigate(INavigatablePage page, object parameter)
        {
            if (page.PlatformPage is Type pageType)
            {
                return Frame.Navigate(pageType, parameter);
            }

            throw new ArgumentException("指定平台 page.PlatformPage 不是 Type" +
                                        $"\r\n can not convert {page.PlatformPage.GetType()} to Type");
        }
    }
}
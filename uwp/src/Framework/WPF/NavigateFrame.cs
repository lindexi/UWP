using System.Windows.Controls;
using lindexi.MVVM.Framework.ViewModel;

namespace lindexi.wpf.Framework
{
    /// <summary>
    ///     提供跳转的 Frame
    /// </summary>
    public class NavigateFrame : INavigateFrame
    {
        /// <inheritdoc />
        public NavigateFrame(Frame frame)
        {
            Frame = frame;
        }

        /// <summary>显示 Page 实例，支持针对新页面的导航，并保留导航历史记录以支持向前和向后导航。</summary>
        public Frame Frame { get; set; }

        /// <summary>
        /// 从 <see cref="Frame"/> 转换为 <see cref="NavigateFrame"/> 可以在不同的平台使用不同的类
        /// </summary>
        /// <param name="frame"></param>
        public static implicit operator NavigateFrame(Frame frame)
        {
            return new NavigateFrame(frame);
        }

        /// <inheritdoc />
        public bool Navigate(INavigatablePage page, object parameter)
        {
            return Frame.Navigate(page.PlatformPage, parameter);
        }
    }
}

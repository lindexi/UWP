using System.Windows.Controls;
using lindexi.MVVM.Framework.ViewModel;

namespace WPF
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

        public Frame Frame { get; set; }

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
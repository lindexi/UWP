using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using lindexi.MVVM.Framework.ViewModel;

namespace UWP
{
    /// <inheritdoc />
    public class NavigateFrame : INavigateFrame
    {
        /// <inheritdoc />
        public NavigateFrame(Frame frame)
        {
            Frame = frame;
        }

        public Frame Frame { get; set; }

        /// <inheritdoc />
        public bool Navigate(INavigatablePage page, object parameter)
        {
            if (page.PlatformPage is Type pageType)
            {
                return Frame.Navigate(pageType, parameter);
            }
            else
            {
                throw new ArgumentException("指定平台 page.PlatformPage 不是 Type" +
                                            $"\r\n can not convert {page.PlatformPage.GetType()} to Type");
            }
        }

        public static implicit operator NavigateFrame(Frame frame)
        {
            return new NavigateFrame(frame);
        }
    }
}
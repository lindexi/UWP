using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace InteresRoller
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            View = new ViewModel.ViewModel();
            this.InitializeComponent();
        }
        private ViewModel.ViewModel View
        {
            set;
            get;
        }

        private void GridColection_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            MenuFlyout flyout = new MenuFlyout();
            MenuFlyoutItem firstItem = new MenuFlyoutItem { Text = "OneIt" };
            MenuFlyoutItem secondItem = new MenuFlyoutItem { Text = "TwoIt" };
            if (flyout.Items != null)
            {
                flyout.Items.Add(firstItem);
                flyout.Items.Add(secondItem);
            }

            //if you only want to show in left or buttom 
            //myFlyout.Placement = FlyoutPlacementMode.Left;

            FrameworkElement senderElement = sender as FrameworkElement;
            //the code can show the flyout in your mouse click 
            flyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }
    }
}

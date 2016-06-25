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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EncryptionSyncFolder.View
{
    public sealed partial class SplitViewItem : UserControl
    {
        public SplitViewItem()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty IconStringProperty = DependencyProperty.Register(
            "IconString", typeof(string), typeof(SplitViewItem), new PropertyMetadata(default(string)));

        public string IconString
        {
            set
            {
                SetValue(IconStringProperty, value);
            }
            get
            {
                return (string) GetValue(IconStringProperty);
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(SplitViewItem), new PropertyMetadata(default(string)));

        public string Text
        {
            set
            {
                SetValue(TextProperty, value);
            }
            get
            {
                return (string) GetValue(TextProperty);
            }
        }
    }
}

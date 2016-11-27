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

namespace Button.Control
{
    public sealed partial class ProgressButton : UserControl
    {
        public ProgressButton()
        {
            InitializeComponent();
            Enable = true;
            ProgressVisibility = Visibility.Collapsed;
        }

        public EventHandler<ProgressButtonEventArgs> ProgressButtonClick
        {
            set;
            get;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(ProgressButton), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty EnableProperty = DependencyProperty.Register(
            "Enable", typeof(bool), typeof(ProgressButton), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty ProgressVisibilityProperty = DependencyProperty.Register(
            "ProgressVisibility", typeof(Visibility), typeof(ProgressButton), new PropertyMetadata(default(Visibility)));

        public Visibility ProgressVisibility
        {
            set
            {
                SetValue(ProgressVisibilityProperty, value);
            }
            get
            {
                return (Visibility)GetValue(ProgressVisibilityProperty);
            }
        }
        public bool Enable
        {
            set
            {
                SetValue(EnableProperty, value);
            }
            get
            {
                return (bool)GetValue(EnableProperty);
            }
        }
        public string Text
        {
            set
            {
                SetValue(TextProperty, value);
            }
            get
            {
                return (string)GetValue(TextProperty);
            }
        }

        public void OnFinished()
        {
            Enable = true;
            ProgressVisibility = Visibility.Collapsed;
        }

        private void ProgressButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (ProgressButtonClick != null)
            {
                ProgressButtonClick.Invoke(sender, new ProgressButtonEventArgs(OnFinished,e));
                Enable = false;
                ProgressVisibility = Visibility.Visible;
            }
        }

        public class ProgressButtonEventArgs : EventArgs
        {
            public ProgressButtonEventArgs(Action onFinished, RoutedEventArgs e)
            {
                OnFinished = onFinished;
                OriginalSource = e;
            }

            public RoutedEventArgs OriginalSource
            {
                get;
            }

            public Action OnFinished
            {
                get;
            }
        }
    }
}

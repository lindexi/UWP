// lindexi
// 15:48

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
    public sealed partial class NewFileDialog : UserControl
    {
        public NewFileDialog()
        {
            this.InitializeComponent();
        }

        public string Reminder
        {
            set
            {
                SetValue(ReminderProperty, value);
            }
            get
            {
                return (string) GetValue(ReminderProperty);
            }
        }

        public string Size
        {
            set
            {
                SetValue(SizeProperty, value);
            }
            get
            {
                return (string) GetValue(SizeProperty);
            }
        }

        public string FileName
        {
            set
            {
                SetValue(FileNameProperty, value);
            }
            get
            {
                return (string) GetValue(FileNameProperty);
            }
        }

        public string SecondaryButtonText
        {
            set
            {
                SetValue(SecondaryButtonTextProperty, value);
            }
            get
            {
                return (string) GetValue(SecondaryButtonTextProperty);
            }
        }

        public string PrimaryButtonText
        {
            set
            {
                SetValue(PrimaryButtonTextProperty, value);
            }
            get
            {
                return (string) GetValue(PrimaryButtonTextProperty);
            }
        }

        public Action<object, RoutedEventArgs> PrimaryButtonClick
        {
            set;
            get;
        }

        public Action<object, RoutedEventArgs> SecondaryButtonClick
        {
            set;
            get;
        }

        public Visibility PrimaryButtonVisibility
        {
            set
            {
                SetValue(PrimaryButtonVisibilityProperty, value);
            }
            get
            {
                return (Visibility) GetValue(PrimaryButtonVisibilityProperty);
            }
        }

        public Visibility SecondaryButtonVisibility
        {
            set
            {
                SetValue(SecondaryButtonVisibilityProperty, value);
            }
            get
            {
                return (Visibility) GetValue(SecondaryButtonVisibilityProperty);
            }
        }

        /// <summary>
        ///     对话完成，如果没有完成会继续显示
        /// </summary>
        public bool Complete
        {
            set;
            get;
        }

        private void PrimaryButton_OnClick(object sender, RoutedEventArgs e)
        {
            PrimaryButtonClick?.Invoke(sender, e);
        }

        private void SecondaryButton_OnClick(object sender, RoutedEventArgs e)
        {
            SecondaryButtonClick?.Invoke(sender, e);
        }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register(
            "FileName", typeof(string), typeof(NewFileDialog), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(string), typeof(NewFileDialog), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ReminderProperty = DependencyProperty.Register(
            "Reminder", typeof(string), typeof(NewFileDialog), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty PrimaryButtonTextProperty = DependencyProperty.Register(
            "PrimaryButtonText", typeof(string), typeof(NewFileDialog), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty SecondaryButtonTextProperty = DependencyProperty.Register(
            "SecondaryButtonText", typeof(string), typeof(NewFileDialog), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty PrimaryButtonVisibilityProperty = DependencyProperty.Register(
            "PrimaryButtonVisibility", typeof(Visibility), typeof(NewFileDialog),
            new PropertyMetadata(default(Visibility)));

        public static readonly DependencyProperty SecondaryButtonVisibilityProperty = DependencyProperty.Register(
            "SecondaryButtonVisibility", typeof(Visibility), typeof(NewFileDialog),
            new PropertyMetadata(default(Visibility)));
    }
}
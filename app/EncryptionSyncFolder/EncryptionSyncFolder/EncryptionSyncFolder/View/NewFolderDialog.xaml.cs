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
    public sealed partial class NewFolderDialog : UserControl
    {
        public NewFolderDialog()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty FolderNameProperty = DependencyProperty.Register(
            "FolderName", typeof(string), typeof(NewFolderDialog), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ReminderProperty = DependencyProperty.Register(
            "Reminder", typeof(string), typeof(NewFolderDialog), new PropertyMetadata(default(string)));

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
        public string FolderName
        {
            set
            {
                SetValue(FolderNameProperty, value);
            }
            get
            {
                return (string) GetValue(FolderNameProperty);
            }
        }

        public bool Complete
        {
            set;
            get;
        }
    }
}

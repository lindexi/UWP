using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Boleslav.View
{
    public sealed partial class KaranPage : UserControl
    {
        public KaranPage()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public event EventHandler<string> Carsen;

        private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            //Eadwulf.IsReadOnly = true;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            Eadwulf.IsReadOnly = false;
            Eadwulf.Focus(FocusState.Programmatic);
        }

        private void Carsen_OnClick(object sender, RoutedEventArgs e)
        {
            Carsen?.Invoke(this, Text);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(KaranPage), new PropertyMetadata(default(string)));
    }

    public class EadwulfVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool) value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }
}
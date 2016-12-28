using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Button.Control
{
    [TemplatePart(Name = "TextBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "Progress", Type = typeof(Windows.UI.Xaml.Controls.ProgressRing))]
    public class ProgressButton : Windows.UI.Xaml.Controls.Button
    {
        public ProgressButton()
        {
            DefaultStyleKey = typeof(ProgressButton);
            Click += ProgressButton_Click;
        }

        private void ProgressButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Complete = false;
        }

        private Windows.UI.Xaml.Controls.TextBlock _textBlock;

        private Windows.UI.Xaml.Controls.ProgressRing _proress;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ProgressButton), new PropertyMetadata("",
                (d, e) =>
                {
                    ProgressButton temp = d as ProgressButton;
                    if (temp == null)
                    {
                        return;
                    }
                    if(temp._textBlock!=null)
                    {
                        temp._textBlock.Text = (string) e.NewValue;
                    }
                }));

        

        public bool Complete
        {
            get { return (bool)GetValue(CompleteProperty); }
            set { SetValue(CompleteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Complete.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompleteProperty =
            DependencyProperty.Register("Complete", typeof(bool), typeof(ProgressButton), new PropertyMetadata(true,
                OnComplete));

        private static void OnComplete(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProgressButton button = d as ProgressButton;
            if (button == null)
            {
                return;
            }

            bool temp = (bool)e.NewValue;


            //button._textBlock.Visibility = temp ? Visibility.Visible : Visibility.Collapsed;
            button._proress.Visibility = temp ? Visibility.Collapsed : Visibility.Visible;
            button.IsEnabled = temp;
        }



        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _textBlock = GetTemplateChild("TextBlock") as Windows.UI.Xaml.Controls.TextBlock;
            _proress = GetTemplateChild("Progress") as Windows.UI.Xaml.Controls.ProgressRing;

            if (_textBlock != null)
            {
                _textBlock.Visibility = Visibility.Visible;
                _textBlock.Text = Text;
            }

            if (_proress != null)
            {
                _proress.Visibility=Visibility.Collapsed;
            }
        }
    }
}

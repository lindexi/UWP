using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OTAManager.Demo.WPF.View
{
    /// <summary>
    /// InputText.xaml 的交互逻辑
    /// </summary>
    public partial class InputText : UserControl
    {
        public InputText()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty AcceptsReturnProperty = DependencyProperty.Register(
            "AcceptsReturn", typeof(bool), typeof(InputText), new PropertyMetadata(default(bool)));

        public bool AcceptsReturn
        {
            get { return (bool) GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register(
            "TextWrapping", typeof(TextWrapping), typeof(InputText), new PropertyMetadata(default(TextWrapping)));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping) GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
            "HeaderText", typeof(string), typeof(InputText), new PropertyMetadata(default(string)));

        public string HeaderText
        {
            get { return (string) GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(InputText), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}

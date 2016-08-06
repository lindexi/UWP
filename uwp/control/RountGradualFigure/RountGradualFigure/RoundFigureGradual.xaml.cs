using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace RountGradualFigure
{
    public sealed partial class RoundFigureGradual : UserControl
    {
        public RoundFigureGradual()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty NProperty = DependencyProperty.Register(
            "N", typeof(int), typeof(RoundFigureGradual), new PropertyMetadata(default(int)));

        public int N
        {
            set
            {
                SetValue(NProperty, value);
            }
            get
            {
                return (int) GetValue(NProperty);
            }
        }
    }
}

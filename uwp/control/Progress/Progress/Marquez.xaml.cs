using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace lindexi.uwp.Progress
{
    /// <summary>
    ///     带文字自动移动进度条
    /// </summary>
    public sealed partial class Marquez : UserControl
    {
        /// <summary>
        ///     进度条
        /// </summary>
        public Marquez()
        {
            InitializeComponent();
            SizeChanged += (s, e) =>
            {
                Scrhrentran(scrohn,
                    e.NewSize.Width, Value, Maximum, Mcdon);
            };
        }


        /// <summary>
        ///     标识 <see cref="Maximum" /> 的依赖项属性。
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(Marquez), new PropertyMetadata(100d, (s, e) =>
            {
                var t = s as Marquez;
                if (t == null)
                {
                    return;
                }

                Scrhrentran(t.scrohn, t.ActualWidth, t.Value, (double) e.NewValue, t.Mcdon);
            }));


        /// <summary>
        ///     标识 <see cref="Minimum" /> 的依赖项属性。
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(Marquez), new PropertyMetadata(default(double), (s, e) =>
            {
            }));


        /// <summary>
        ///     标识 <see cref="Value" /> 的依赖项属性。
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(Marquez), new PropertyMetadata(20d, (s, e) =>
            {
                var t = s as Marquez;
                if (t == null)
                {
                    return;
                }

                Scrhrentran(t.scrohn, t.ActualWidth, (double) e.NewValue, t.Maximum, t.Mcdon);
            }));

        /// <summary>
        ///     获取或设置
        /// </summary>
        public double Value
        {
            get { return (double) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        ///     获取或设置最小值
        /// </summary>
        public double Minimum
        {
            get { return (double) GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        ///     获取或设置最大值
        /// </summary>
        public double Maximum
        {
            get { return (double) GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        ///     用于调节进度条
        /// </summary>
        /// <param name="scrohn"></param>
        /// <param name="w"></param>
        /// <param name="v"></param>
        /// <param name="t"></param>
        /// <param name="mcdon"></param>
        private static void Scrhrentran(TextBlock scrohn, double w, double v, double t, ProgressBar mcdon)
        {
            //显示的文字
            scrohn.Text = v.ToString("F") + "/" + t.ToString("F");

            //设置进度条
            mcdon.Value = v;
            mcdon.Maximum = t;

            var sc = scrohn.RenderTransform as TranslateTransform;
            if (v > t)
            {
                v = t;
            }
            if (sc != null)
            {
                sc.X = w / 2 * v / t - scrohn.ActualWidth / 2;
                if (sc.X < 0)
                {
                    sc.X = 0;
                }
            }
        }
    }
}

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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace lindexi.uwp.control.RountProgress.View
{
    public sealed partial class Marquez : UserControl
    {
        public Marquez()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// 标识 <see cref="Maximum"/> 的依赖项属性。
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(Marquez), new PropertyMetadata(100d));


        /// <summary>
        /// 标识 <see cref="Minimum"/> 的依赖项属性。
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(Marquez), new PropertyMetadata(default(double)));


        /// <summary>
        /// 标识 <see cref="Value"/> 的依赖项属性。
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(Marquez), new PropertyMetadata(100d, (s, e) =>
            {
                var t = s as Marquez;
                if (t == null)
                {
                    return;
                }

                t.Mcdon.Value = (double)e.NewValue;
                t.scrohn.Text = ((double)e.NewValue).ToString("F") + "/" + t.Maximum.ToString("f");
                var sc = t.scrohn.RenderTransform as TranslateTransform;
                if (sc != null)
                {
                    sc.X = t.ActualWidth / 2 * (double)e.NewValue / t.Maximum;
                }
            }));

        /// <summary>
        /// 获取或设置
        /// </summary>
        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// 获取或设置最小值
        /// </summary>
        public double Minimum
        {
            get
            {
                return (double)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        /// <summary>
        /// 获取或设置最大值
        /// </summary>
        public double Maximum
        {
            get
            {
                return (double)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }
    }
}

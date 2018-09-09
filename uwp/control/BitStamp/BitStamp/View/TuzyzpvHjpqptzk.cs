using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace BitStamp
{
    [ContentProperty(Name = "Content")]
    public class TuzyzpvHjpqptzk : Control
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(UIElement), typeof(TuzyzpvHjpqptzk), new PropertyMetadata(default(UIElement), (s, e) =>
            {
                var dljolSposrtc = s as TuzyzpvHjpqptzk;
                if (dljolSposrtc != null)
                {
                    
                }
            }));

        public UIElement Content
        {
            get { return (UIElement) GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            

            var element = (UIElement) Content;
            if (element != null)
            {
                element.Measure(availableSize);
                return availableSize;
            }

            return base.MeasureOverride(availableSize);
        }

        private HlpcToeczdy DqnjpdDcxhrxiaa { set; get; } = new HlpcToeczdy();

        protected override Size ArrangeOverride(Size finalSize)
        {
            //获取元素的大小

            var element = (UIElement) Content;

            if (element != null)
            {
                var size = element.DesiredSize;

                if (Math.Abs(size.Width) < 0.001 || Math.Abs(size.Height) < 0.001)
                {
                    var hsitrcTmpb = Math.Max(size.Width, size.Height);
                    hsitrcTmpb = Math.Max(hsitrcTmpb, 1);
                    size = new Size(hsitrcTmpb, hsitrcTmpb);
                }

                //拿到最大的圆

                var diameter = Math.Min(finalSize.Width, finalSize.Height); //直径

                var kqpdeamgHlgcvrn = new Point(finalSize.Width / 2, finalSize.Height / 2);
                //中心点

                var kpqeeqjaSxd = DqnjpdDcxhrxiaa.TqxazjDmsauhx(diameter, size);

                kpqeeqjaSxd = new Size(kpqeeqjaSxd.Width * 2, kpqeeqjaSxd.Height * 2);

                var rect = new Rect(
                    new Point(kqpdeamgHlgcvrn.X - kpqeeqjaSxd.Width / 2, kqpdeamgHlgcvrn.Y - kpqeeqjaSxd.Height / 2),
                    kpqeeqjaSxd);

                element.Arrange(rect);

                return finalSize;
            }


            return base.ArrangeOverride(finalSize);
        }
    }

    public class HlpcToeczdy
    {
        /// <summary>
        /// 给直径，长宽
        /// </summary>
        /// <param name="diameter"></param>
        /// <param name="dcauipgcDoaymdqvy"></param>
        public Size TqxazjDmsauhx(double diameter, Size dcauipgcDoaymdqvy)
        {
            var width = dcauipgcDoaymdqvy.Width / 2;
            var height = dcauipgcDoaymdqvy.Height / 2;

            var tan = height / width;

            //a^2+b^2=diameter/2 ^2

            //a/b=tan

            //a^2=diameter/2 ^2 -b^2
            //a=开方 diameter/2 ^2  -b^2
            //开方 diameter/2 ^2  -b^2 /b =tan
            //diameter/2 ^2 -b^2=tan * b  ^2
            //diameter/2 ^2  = tan^2*b^2+b^2
            //b=开方 diameter/2  (tan^2+1)

            width = Math.Sqrt(Math.Pow(diameter / 2, 2) / (Math.Pow(tan, 2) + 1));

            height = Math.Sqrt(Math.Pow(diameter / 2, 2) - Math.Pow(width, 2));

            return new Size(width, height);
        }
    }
}
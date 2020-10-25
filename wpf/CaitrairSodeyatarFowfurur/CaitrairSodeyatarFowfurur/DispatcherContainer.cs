using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace CaitrairSodeyatarFowfurur
{
    public class DispatcherContainer : UIElement
    {
        /// <inheritdoc />
        public DispatcherContainer()
        {
            var thread = new Thread(() =>
            {
                _visualTarget = new VisualTarget(_hostVisual);
                DrawingVisual drawingVisual = new DrawingVisual();
                var drawing = drawingVisual.RenderOpen();
                using (drawing)
                {
                    var text = new FormattedText("欢迎访问我博客 http://lindexi.gitee.io 里面有大量 UWP WPF 博客",
                        CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface(new FontFamily("微软雅黑"), new FontStyle(), FontWeight.FromOpenTypeWeight(1),
                            FontStretch.FromOpenTypeStretch(1)), 20, Brushes.DarkSlateBlue);

                    drawing.DrawText(text, new Point(100, 100));
                }

                var containerVisual = new ContainerVisual();

                containerVisual.Children.Add(drawingVisual);

                _visualTarget.RootVisual = containerVisual;

                System.Windows.Threading.Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        /// <inheritdoc />
        protected override Visual GetVisualChild(int index)
        {
            return _hostVisual;
        }

        /// <inheritdoc />
        protected override int VisualChildrenCount => 1;

        private readonly HostVisual _hostVisual = new HostVisual();
        private VisualTarget _visualTarget;
    }
}

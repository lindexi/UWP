using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using VarietyHiggstGushed.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VarietyHiggstGushed.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    [lindexi.uwp.Framework.ViewModel.ViewModel(typeof(TvrwgrnNnuModel))]
    public sealed partial class BpyaxxjwkQwknemobzPage : Page
    {
        public BpyaxxjwkQwknemobzPage()
        {
            this.InitializeComponent();
            Loaded += BpyaxxjwkQwknemobzPage_Loaded;
            Unloaded += BpyaxxjwkQwknemobzPage_Unloaded;
            SizeChanged += BpyaxxjwkQwknemobzPage_SizeChanged;

        }

        private void BpyaxxjwkQwknemobzPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!FireflyParticle.Any())
            {
                Rect bound = new Rect(0, 0, canvas.ActualWidth, canvas.ActualHeight);
                for (int i = 0; i < 100; i++)
                {
                    FireflyParticle.Add(new FireflyParticle(bound));
                }
            }
        }

        private void BpyaxxjwkQwknemobzPage_Unloaded(object sender, RoutedEventArgs e)
        {
            canvas.RemoveFromVisualTree();
            canvas = null;
        }

        private void BpyaxxjwkQwknemobzPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Rect bound = new Rect(0, 0, canvas.ActualWidth, canvas.ActualHeight);
            foreach (var temp in FireflyParticle)
            {
                temp.Bound = new Rect(0, 0, canvas.ActualWidth, canvas.ActualHeight);
            }
        }

        public TvrwgrnNnuModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (TvrwgrnNnuModel) e.Parameter;
            base.OnNavigatedTo(e);
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AdraqbqhUgtwg();
        }

        private void DxpoihQprdqbip_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.DxpoihQprdqbip();
        }

        private async void YxbrbfgEakhybi_OnClick(object sender, RoutedEventArgs e)
        {
            await new MessageDialog("开源游戏 https://github.com/lindexi/UWP/tree/master/uwp/src/VarietyHiggstGushed", "关于")
            {
                Options = MessageDialogOptions.None,
                CancelCommandIndex = 1,
                Commands =
                {
                    new UICommand("打开链接")
                    {
                        Invoked = async command =>
                        {
                            await Launcher.LaunchUriAsync(new Uri(
                                "https://github.com/lindexi/UWP/tree/master/uwp/src/VarietyHiggstGushed"));
                        }
                    },
                    new UICommand("关闭")
                }
            }.ShowAsync();
        }

        private void Canvas_OnUpdate(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            foreach (var temp in FireflyParticle)
            {
                temp.Time(args.Timing.ElapsedTime);
            }
        }

        private List<FireflyParticle> FireflyParticle { set; get; } = new List<FireflyParticle>();

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                foreach (var temp in FireflyParticle)
                {
                    using (var cl = new CanvasCommandList(session))
                    using (var ds = cl.CreateDrawingSession())
                    {
                        var c = temp.CenterColor;
                        c.A = (byte) (temp.OpColor * 255);
                        ds.FillCircle((float) temp.Point.X, (float) temp.Point.Y, (float) temp.Radius, c);
                        using (var glow = new GlowEffectGraph())
                        {
                            glow.Setup(cl, temp.Radius);
                            session.DrawImage(glow.Blur);
                        }
                    }
                }
            }
        }
    }
    class GlowEffectGraph : IDisposable
    {
        private MorphologyEffect morphology;
        public GlowEffectGraph()
        {
            Blur.BlurAmount = 10;
            Blur.BorderMode = EffectBorderMode.Soft;

            morphology = new MorphologyEffect()
            {
                Mode = MorphologyEffectMode.Dilate,
                Width = 10,
                Height = 10,
            };

            Blur.Source = morphology;

        }

        public GaussianBlurEffect Blur { get; set; } = new GaussianBlurEffect();

        public void Dispose()
        {
            Blur.Dispose();
            morphology.Dispose();
        }

        public void Setup(ICanvasImage canvas, double amount = 10)
        {
            morphology.Source = canvas;
            amount = Math.Min(amount / 2, 100);
            morphology.Width = (int) Math.Truncate(Math.Floor(amount));
            morphology.Height = (int) Math.Truncate(Math.Floor(amount));
            Blur.BlurAmount = (float) amount;
        }
    }

    class FireflyParticle
    {
        public FireflyParticle(Rect bound)
        {
            Point = new Point(ran.Next((int) bound.Width), ran.Next((int) bound.Height));
            _x = new Ran(Point.X, bound.Width, 0);
            _y = new Ran(Point.Y, bound.Height, 0);
            _radius = new Ran(ran.Next(2, 5), 5, 2)
            {
                Po = 0.71
            };
            Bound = bound;
        }

        public FireflyParticle()
        {
        }

        public void Time(TimeSpan time)
        {
            //var n = ran.Next(2) > 0 ? 1 : -1;
            //Radius += ran.NextDouble() * n;
            //if (Radius < 2)
            //{
            //    Radius = 2;
            //}

            //n = ran.Next(2) > 0 ? 1 : -1;
            //var x = Point.X;
            //var y = Point.Y;
            //x += ran.NextDouble() * n;
            //n = ran.Next(2) > 0 ? 1 : -1;
            //y += ran.NextDouble() * n;
            //Point = new Point(x, y);

            //n = ran.Next(2) > 0 ? 1 : -1;
            //OpColor += ran.NextDouble() / 10 * n;

            _radius.Time(time);
            _opColor.Time(time);
            _x.Time(time);
            _y.Time(time);

            Radius = _radius.Value;
            OpColor = _opColor.Value;
            Point = new Point(_x.Value, _y.Value);
        }

        public Point Point { get; set; }

        public Rect Bound
        {
            get { return _bound; }
            set
            {
                _bound = value;
                _x.Ma = value.Width;
                _y.Ma = value.Height;
            }
        }

        public double Radius { get; set; } = 10;
        public Color CenterColor { get; set; } = Color.FromArgb(255, 252, 203, 89);
        public double OpColor { set; get; } = 1;
        private static Random ran = new Random();

        private Ran _radius;
        private Ran _opColor = new Ran(1, 1, 0.001);

        private Ran _x;
        private Ran _y;
        private Rect _bound;
    }

    class Ran
    {
        public Ran(double value, double ma, double mi)
        {
            Value = value;
            Ma = ma;
            Mi = mi;
            To = ran.NextDouble() * (Ma - Mi) + Mi;
        }

        public double Value { get; set; }
        public double To { get; set; }
        public double Dalue { get; set; }

        public double Ma { get; set; }

        public double Mi { get; set; }

        /// <summary>
        /// 加速度
        /// </summary>
        public double Po { get; set; }

        public void Time(TimeSpan time)
        {
            if (Math.Abs(Dalue) < 0.000001)
            {
                if (Math.Abs(Po) < 0.0001)
                {
                    Dalue = Math.Abs(Value - To) / ran.Next(10, 300);
                }
                else
                {
                    Dalue = Po;
                }
            }
            int n = 1;
            if (Value > To)
            {
                n = n * -1;
            }
            Value += n * Dalue * time.TotalSeconds * 2;
            if (n > 0 && Value > To)
            {
                Value = To;
                To = ran.NextDouble() * (Ma - Mi) + Mi;
            }
            if (n < 0 && Value < To)
            {
                Value = To;
                To = ran.NextDouble() * (Ma - Mi) + Mi;
            }
        }


        private static Random ran = new Random();
    }

}

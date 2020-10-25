using System;
using System.Collections.Generic;
using System.Linq;
using lindexi.MVVM.Framework.ViewModel;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using VarietyHiggstGushed.ViewModel;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace VarietyHiggstGushed.View
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    [ViewModel(typeof(TvrwgrnNnuModel))]
    public sealed partial class BpyaxxjwkQwknemobzPage : Page
    {
        public BpyaxxjwkQwknemobzPage()
        {
            InitializeComponent();
            Loaded += BpyaxxjwkQwknemobzPage_Loaded;
            Unloaded += BpyaxxjwkQwknemobzPage_Unloaded;
            SizeChanged += BpyaxxjwkQwknemobzPage_SizeChanged;
        }

        public TvrwgrnNnuModel ViewModel { get; set; }

        private void BpyaxxjwkQwknemobzPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!FireflyParticle.Any())
            {
                var bound = new Rect(0, 0, canvas.ActualWidth, canvas.ActualHeight);
                for (var i = 0; i < 100; i++)
                {
                    FireflyParticle.Add(new FireflyParticle(bound));
                }
            }

            Window.Current.VisibilityChanged += VisibilityChanged;
            Window.Current.Activated += Activated;
        }

        private void Activated(object sender, WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == CoreWindowActivationState.CodeActivated)
            {
                canvas.Paused = false;
            }
            else if (e.WindowActivationState == CoreWindowActivationState.PointerActivated)
            {
                canvas.Paused = false;
            }
            else if (e.WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                canvas.Paused = true;
            }
        }

        private void VisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            canvas.Paused = !e.Visible;
        }

        private void BpyaxxjwkQwknemobzPage_Unloaded(object sender, RoutedEventArgs e)
        {
            canvas.RemoveFromVisualTree();
            canvas = null;
            Window.Current.VisibilityChanged -= VisibilityChanged;
            Window.Current.Activated -= Activated;
        }

        private void BpyaxxjwkQwknemobzPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var bound = new Rect(0, 0, canvas.ActualWidth, canvas.ActualHeight);
            foreach (var temp in FireflyParticle)
            {
                temp.Bound = bound;
            }
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
                                    "https://github.com/lindexi/UWP/tree/master/uwp/src/VarietyHiggstGushed"),
                                new LauncherOptions
                                {
                                    TreatAsUntrusted = true
                                });
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

        private List<FireflyParticle> FireflyParticle { get; } = new List<FireflyParticle>();

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = (TvrwgrnNnuModel) e.Parameter;
            base.OnNavigatedTo(e);
        }
    }

    internal class GlowEffectGraph : IDisposable
    {
        public GlowEffectGraph()
        {
            Blur.BlurAmount = 10;
            Blur.BorderMode = EffectBorderMode.Soft;

            morphology = new MorphologyEffect
            {
                Mode = MorphologyEffectMode.Dilate,
                Width = 10,
                Height = 10
            };

            Blur.Source = morphology;
        }

        public GaussianBlurEffect Blur { get; set; } = new GaussianBlurEffect();

        public void Setup(ICanvasImage canvas, double amount = 10)
        {
            morphology.Source = canvas;
            amount = Math.Min(amount / 2, 100);
            morphology.Width = (int) Math.Truncate(Math.Floor(amount));
            morphology.Height = (int) Math.Truncate(Math.Floor(amount));
            Blur.BlurAmount = (float) amount;
        }

        public void Dispose()
        {
            Blur.Dispose();
            morphology.Dispose();
        }

        private readonly MorphologyEffect morphology;
    }

    internal class FireflyParticle
    {
        public FireflyParticle(Rect bound)
        {
            Point = new Point(ran.Next((int) bound.Width), ran.Next((int) bound.Height));
            _x = new Ran(Point.X, bound.Width, 0)
            {
                EasingFunction = true
            };
            _y = new Ran(Point.Y, bound.Height, 0)
            {
                EasingFunction = true
            };
            _radius = new Ran(ran.Next(2, 5), 5, 2)
            {
                Po = 0.71
            };
            Bound = bound;
        }

        public FireflyParticle()
        {
        }

        private static readonly Random ran = new Random();

        public Point Point { get; set; }

        public Rect Bound
        {
            get => _bound;
            set
            {
                _bound = value;
                _x.Ma = value.Width - 10;
                _y.Ma = value.Height - 10;
            }
        }

        public double Radius { get; set; } = 10;
        public Color CenterColor { get; set; } = Color.FromArgb(255, 252, 203, 89);
        public double OpColor { set; get; } = 1;

        public void Time(TimeSpan time)
        {
            _radius.Time(time);
            _opColor.Time(time);
            _x.Time(time);
            _y.Time(time);

            Radius = _radius.Value;
            OpColor = _opColor.Value;
            Point = new Point(_x.Value, _y.Value);
        }

        private Rect _bound;
        private readonly Ran _opColor = new Ran(1, 1, 0.001);

        private readonly Ran _radius;

        private readonly Ran _x;
        private readonly Ran _y;
    }

    internal class Ran
    {
        public Ran(double value, double ma, double mi)
        {
            Value = value;
            Ma = ma;
            Mi = mi;
            To = ran.NextDouble() * (Ma - Mi) + Mi;
        }


        private static readonly Random ran = new Random();

        public double Value { get; set; }
        public double To { get; set; }
        public double Dalue { get; set; }

        public double Ma { get; set; }

        public double Mi { get; set; }

        public bool EasingFunction { get; set; }

        /// <summary>
        ///     加速度
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

            //减数
            if (EasingFunction && Math.Abs(Value - To) < Dalue * 10 /*如果接近*/)
            {
                Dalue /= 2;
                if (Math.Abs(Dalue) < 1)
                {
                    Dalue = 1;
                }
            }

            var n = 1;
            if (Value > To)
            {
                n = n * -1;
            }

            Value += n * Dalue * time.TotalSeconds * 2;
            if (n > 0 && Value >= To)
            {
                Value = To;
                To = ran.NextDouble() * (Ma - Mi) + Mi;
                Dalue = 0;
            }

            if (n < 0 && Value <= To)
            {
                Value = To;
                To = ran.NextDouble() * (Ma - Mi) + Mi;
                Dalue = 0;
            }
        }
    }
}

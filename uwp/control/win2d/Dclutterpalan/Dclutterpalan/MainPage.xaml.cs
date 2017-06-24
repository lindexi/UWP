using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Dclutterpalan
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            //SizeChanged += MainPage_SizeChanged;

            _stone = new Dictionary<string, Stone>()
            {
                {
                    "星", new Stone()
                    {
                        MinCorners = 3,
                        MaxCorners = 20,
                        GetCenter = (perX, perY, i, j) => new Point((int) (perX * j), (int) (perY * i)),
                        GetouterRadius = perX => (int) (perX * 0.18f),
                        GetinnerRadius = perX => (int) (perX * 0.06f)
                    }
                },
                {
                    "竹叶", new Stone()
                    {
                        MinCorners = 3,
                        MaxCorners = 4,
                        GetCenter = (perX, perY, i, j) => new Point((int) (perX * j), (int) (perY * i)),
                        GetouterRadius = perX => (int) (perX * 1.4f),
                        GetinnerRadius = perX => (int) (perX * 0.009f),
                    }
                },
                {
                    "草", new Stone()
                    {
                        MinCorners = 5,
                        MaxCorners = 38,
                        GetCenter = (perX, perY, i, j) => new Point((int) (perX * j), (int) (perY * i)),
                        GetouterRadius = perX => (int) (perX * 0.88f),
                        GetinnerRadius = perX => (int) (perX * 0.01f),
                    }
                }
            };
        }


        private List<Point[]> _points = new List<Point[]>();

        private Dictionary<string, Stone> _stone;


        //private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    if (e.NewSize.Width < MinWidht)
        //    {
        //        if (_grid)
        //        {
        //            _grid = false;

        //            Grid.Visibility = Visibility.Collapsed;
        //            var children = Grid.Children.ToList();
        //            Grid.Children.Clear();

        //            GridView.ItemsSource = children.ToList();
        //            GridView.Visibility = Visibility.Visible;
        //            Grid.Children.Clear();
        //        }
        //    }
        //    else
        //    {
        //        if (!_grid)
        //        {
        //            // change to GridView to Grid
        //        }
        //    }
        //}

        //private const double MinWidht = 700;

        //private bool _grid = true;


        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            string str = ((Button) sender).Content?.ToString();

            _points = new List<Point[]>();

            int width = 500;
            int height = 500;
            int numX = 10;
            int numY = 10;
            float perX = width * 1f / numX;
            float perY = height * 1f / numY;

            int minCorners = _stone[str].MinCorners;
            int maxCorners = _stone[str].MaxCorners;

            int lastCorners = minCorners;
            Random random = new Random();
            for (int i = 0; i < numX; i++)
            {
                for (int j = 0; j < numY; j++)
                {
                    int corners = random.Next(minCorners, maxCorners);
                    if (Math.Abs(corners - lastCorners) < (maxCorners - minCorners) / 2)
                        corners = RetrievRandomCorners(minCorners, maxCorners);
                    lastCorners = corners;
                    Point point = _stone[str].GetCenter(perX, perY, i, j);
                    int outerRadius = _stone[str].GetouterRadius(perX);
                    int innerRadius = _stone[str].GetinnerRadius(perX);
                    var points = Stone.CreateStone(point, outerRadius,
                        innerRadius, corners);
                    _points.Add(points);
                }
            }

            canvas.Invalidate();
        }


        private int RetrievRandomCorners(int minCorners, int maxCorners)
        {
            return new Random(Guid.NewGuid().GetHashCode()).Next(minCorners, maxCorners);
        }


        private void Canvas_OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var draw = args.DrawingSession;
            foreach (var point in _points)
            {
                var canvasGeometry = CanvasGeometry.CreatePolygon(draw.Device,
                    point.Select(temp => new Vector2((float) temp.X, (float) temp.Y)).ToArray());
                draw.DrawGeometry(canvasGeometry, Color.FromArgb(255, 100, 100, 100));
                //尝试使用 FillGeometry ，可以看到很好看的效果
            }
        }
    }


    public class Stone
    {
        public static Point[] CreateStone(Point center, int outerRadius, int innerRadius, int arms)
        {
            int centerX = (int) center.X;
            int centerY = (int) center.Y;
            Point[] points = new Point[arms * 2];
            double offset = Math.PI / 2;
            double arc = 2 * Math.PI / arms;
            double half = arc / 2;
            for (int i = 0; i < arms; i++)
            {
                Random randomOuter = new Random((int) DateTime.Now.Ticks);
                outerRadius = outerRadius -
                              randomOuter.Next((int) (innerRadius * 0.06 * new Random().Next(-20, 20) / 30d),
                                  (int) (innerRadius * 0.08));
                Random randomInner = new Random(Guid.NewGuid().GetHashCode());
                innerRadius = innerRadius +
                              randomInner.Next((int) (innerRadius * 0.02 * new Random().Next(-100, 100) / 150d),
                                  (int) (innerRadius * 0.08));
                if (innerRadius > outerRadius)
                {
                    int temp = outerRadius;
                    outerRadius = innerRadius;
                    innerRadius = temp;
                }
                double angleTemp = arc * randomInner.Next(-5, 5) / 10d;
                var angle = i * arc;
                angle += angleTemp;
                points[i * 2].X = (float) (centerX + Math.Cos(angle - offset) * outerRadius) + 20;
                points[i * 2].Y = (float) (centerY + Math.Sin(angle - offset) * outerRadius) + 20;
                points[i * 2 + 1].X = (float) (centerX + Math.Cos(angle + half - offset) * innerRadius) + 20;
                points[i * 2 + 1].Y = (float) (centerY + Math.Sin(angle + half - offset) * innerRadius) + 20;
            }

            return points;
        }

        public Func<float, float, int, int, Point> GetCenter { set; get; }
        public Func<float, int> GetinnerRadius { set; get; }
        public Func<float, int> GetouterRadius { set; get; }
        public int MaxCorners { set; get; } = 18;
        public int MinCorners { set; get; } = 3;
    }
}
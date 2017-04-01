using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Microsoft.Graphics.Canvas;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Simulationq
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            canvas.Invalidate();
            ViewModel.NextUmShi += WinNextUm;
            FillSolidColor = new SolidColorBrush(Colors.Gray);

            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var applicationView = ApplicationView.GetForCurrentView();
                applicationView.SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundColor = Colors.Beige;
                statusbar.BackgroundOpacity = 0.2;
                statusbar.ForegroundColor = Colors.Black;
                applicationView.TryEnterFullScreenMode();
            }

            var s = ViewModel.Solid;
            var c = 0;
            var r = 0;
            var w = ViewModel.Width;
            foreach (var temp in s)
            {
                Rectangle.Add(new Rect()
                {
                    X = w * c,
                    Y = w * r,
                    Width = w,
                    Height = w,
                });
                c++;
                if (c == ViewModel.Col)
                {
                    c = 0;
                    r++;
                }
            }
        }

        private Random Ran { set; get; } = new Random();

        CanvasBitmap img;

        private List<Rect> Rectangle { set; get; } = new List<Rect>();

        private void Canvas_OnDraw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            var draw = args.DrawingSession;

            var s = ViewModel.Solid;
            //var c = 0;
            //var r = 0;
            //var w = ViewModel.Width;

            for (var i = 0; i < s.Length; i++)
            {
                var temp = s[i];
                draw.FillRectangle(Rectangle[i], temp.WeizCsefsimile ? FillSolidColor.Color : Colors.Transparent);
                draw.DrawRectangle(Rectangle[i], temp.WeizCsefsimile ? BoundColor : Colors.Transparent);
                //c++;
                //if (c == ViewModel.Col)
                //{
                //    c = 0;
                //    r++;
                //}
            }

            //draw.DrawText("lindexi", Ran.Next(10, 100), Ran.Next(10, 100), 500, 50, r(), new CanvasTextFormat()
            //{
            //    FontSize = 100
            //});

            //for (int i = 0; i < 10; i++)
            //{
            //    draw.DrawLine(Ran.Next(10, 100), Ran.Next(10, 100), Ran.Next(100, 1000), Ran.Next(100, 1000), r());
            //}
            //if (img != null)
            //{
            //    draw.DrawImage(img, Ran.Next(10, 1000), rc());
            //}
            //else
            //{
            //    Img().Wait();
            //}

            //Color r()
            //{
            //    return Color.FromArgb(0xFF, rc(), rc(), rc());
            //}

            //byte rc()
            //{
            //    return (byte)(Ran.NextDouble() * 255);
            //}

            //async Task Img()
            //{
            //    img = await CanvasBitmap.LoadAsync(canvas, new Uri("ms-appx:///Assets/SplashScreen.png"));
            //}
        }

        private void Page_OnUnloaded(object sender, RoutedEventArgs e)
        {
            canvas.RemoveFromVisualTree();
            canvas = null;
        }

        private Color BoundColor { set; get; } = Color.FromArgb(255, 0x56, 0x56, 0x56);

        public ViewModel.ViewModel ViewModel { get; set; } = new ViewModel.ViewModel();

        private async void WinNextUm(object sender, object e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   canvas.Invalidate();
               });
        }


        public SolidColorBrush FillSolidColor
        {
            set; get;
        }

        public SolidColorBrush StrokeSolidColor
        {
            set; get;
        }


        //private void NewRectangle()
        //{
        //    _rectangle=new Rectangle[View.Row,View.Col];

        //    Canvas.Width = View.Width*View.Col;
        //    Canvas.Height = View.Height*View.Row;
        //    Canvas.Background=new SolidColorBrush(Colors.White);

        //    double width;
        //    double height;

        //    width = View.Width;
        //    height = View.Height;

        //    Button[,] _button=new Button[View.Row, View.Col];


        //    for (int i = 0; i < View.Row; i++)
        //    {
        //        for (int j = 0; j < View.Col; j++)
        //        {
        //            //_rectangle[i,j]=new Rectangle()
        //            //{
        //            //    Width = width,
        //            //    Height = height,
        //            //    Fill = View.FillSolidColor,
        //            //    Stroke = View.StrokeSolidColor,
        //            //    Margin = new Thickness(width*j, height * i, 0,0)
        //            //};
        //            //Binding bind = new Binding()
        //            //{
        //            //    Path = new PropertyPath("Solid[" + (i * View.Col + j).ToString() + "].SolidColor"),
        //            //    Mode = BindingMode.OneWay
        //            //};
        //            //_rectangle[i, j].DataContext = View;
        //            //_rectangle[i, j].SetBinding(Shape.FillProperty, bind);
        //            //Canvas.Children.Add(_rectangle[i, j]);

        //            _button[i,j]=new Button()
        //            {
        //                Width = width,
        //                Height = height,
        //                Background =  View.FillSolidColor,
        //                Margin = new Thickness(width * j, height * i, 0, 0)
        //            };
        //            Binding bind = new Binding()
        //            {
        //                Path = new PropertyPath("Solid[" + (i * View.Col + j).ToString() + "].SolidColor"),
        //                Mode = BindingMode.OneWay
        //            };
        //            Canvas.Children.Add(_button[i, j]);
        //            _button[i, j].DataContext = View;
        //            _button[i,j].SetBinding(Button.BackgroundProperty,bind);
        //            var i1 = i;
        //            var j1 = j;
        //            _button[i, j].Click += (s, e) =>
        //            {
        //                View.Solid[i1 * View.Col+j1].Rsolid();
        //            };
        //        }
        //    }
        //}

        private Rectangle[,] _rectangle;




        private ViewModel.ViewModel View
        {
            set;
            get;
        }
    }
}

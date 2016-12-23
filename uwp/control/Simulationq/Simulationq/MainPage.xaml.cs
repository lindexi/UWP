using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

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
            View=new ViewModel.ViewModel();
            this.InitializeComponent();
            DataContext = View;
            NewRectangle();
            View.Bjie += async () =>
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    async () =>
                    {
                        var bitmap = new RenderTargetBitmap();
                        DateTime time = DateTime.Now;
                        Random ran = new Random();
                        string str = time.Year + time.Month + time.Day + time.Hour.ToString() + time.Minute + ran.Next(1000);
                        StorageFile file = await KnownFolders.PicturesLibrary.CreateFileAsync(str + ".jpg",
                            CreationCollisionOption.GenerateUniqueName);
                        await bitmap.RenderAsync(Canvas);
                        var buffer = await bitmap.GetPixelsAsync();
                        using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                        {
                            var encod = await BitmapEncoder.CreateAsync(
                                BitmapEncoder.JpegEncoderId, stream);
                            encod.SetPixelData(BitmapPixelFormat.Bgra8,
                                BitmapAlphaMode.Ignore,
                                (uint)bitmap.PixelWidth,
                                (uint)bitmap.PixelHeight,
                                DisplayInformation.GetForCurrentView().LogicalDpi,
                                DisplayInformation.GetForCurrentView().LogicalDpi,
                                buffer.ToArray()
                               );
                            await encod.FlushAsync();
                        }
                    });

               
            };
        }

        private void NewRectangle()
        {
            _rectangle=new Rectangle[View.Row,View.Col];

            Canvas.Width = View.Width*View.Col;
            Canvas.Height = View.Height*View.Row;
            Canvas.Background=new SolidColorBrush(Colors.White);

            double width;
            double height;

            width = View.Width;
            height = View.Height;

            Button[,] _button=new Button[View.Row, View.Col];


            for (int i = 0; i < View.Row; i++)
            {
                for (int j = 0; j < View.Col; j++)
                {
                    //_rectangle[i,j]=new Rectangle()
                    //{
                    //    Width = width,
                    //    Height = height,
                    //    Fill = View.FillSolidColor,
                    //    Stroke = View.StrokeSolidColor,
                    //    Margin = new Thickness(width*j, height * i, 0,0)
                    //};
                    //Binding bind = new Binding()
                    //{
                    //    Path = new PropertyPath("Solid[" + (i * View.Col + j).ToString() + "].SolidColor"),
                    //    Mode = BindingMode.OneWay
                    //};
                    //_rectangle[i, j].DataContext = View;
                    //_rectangle[i, j].SetBinding(Shape.FillProperty, bind);
                    //Canvas.Children.Add(_rectangle[i, j]);

                    _button[i,j]=new Button()
                    {
                        Width = width,
                        Height = height,
                        Background =  View.FillSolidColor,
                        Margin = new Thickness(width * j, height * i, 0, 0)
                    };
                    Binding bind = new Binding()
                    {
                        Path = new PropertyPath("Solid[" + (i * View.Col + j).ToString() + "].SolidColor"),
                        Mode = BindingMode.OneWay
                    };
                    Canvas.Children.Add(_button[i, j]);
                    _button[i, j].DataContext = View;
                    _button[i,j].SetBinding(Button.BackgroundProperty,bind);
                    var i1 = i;
                    var j1 = j;
                    _button[i, j].Click += (s, e) =>
                    {
                        View.Solid[i1 * View.Col+j1].Rsolid();
                    };
                }
            }
        }

        private Rectangle[,] _rectangle;


        //public List<string> Str=new List<string>()
        //{
        //    "林德熙",
        //    "CSDN博客",
        //    "九幽"
        //};

        private ViewModel.ViewModel View
        {
            set;
            get;
        }
    }
}

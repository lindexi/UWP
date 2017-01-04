using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace ImageMoseClick
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            View = (ViewModel.ViewModel) DataContext;
            View.Content = Frame;
            View.Read();
        }

        private ViewModel.ViewModel View
        {
            set;
            get;
        }

        private async void Image_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            
            var position = e.GetPosition(sender as UIElement);
            //if (View.Image.UriSource == null)
            //{
                
            //}
            WriteableBitmap image = await BitmapFactory.New(1, 1).FromContent((View.Image).UriSource);
            Color temp;
            //for (int i = 10; i < image.PixelHeight; i++)
            //{
            //    for (int j = 0; j < image.PixelWidth; j++)
            //    {
            //        temp = image.GetPixel(j, i);
            //        if (temp.R != 255)
            //        {

            //        }
            //        temp = image.GetPixel(i, j);
            //        if (temp.R != 255)
            //        {

            //        }
            //    }
            //}

            temp = image.GetPixel((int) position.Y, (int) position.X);
            //temp = image.GetPixel(4, 4);

            Text.Text = $"R: {temp.R} G: {temp.G} B: {temp.B} ";
        }
    }
}

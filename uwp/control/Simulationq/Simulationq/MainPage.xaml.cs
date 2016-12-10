using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        }

        private void NewRectangle()
        {
            _rectangle=new Rectangle[View.Row,View.Col];

            Canvas.Width = View.Width*View.Col;
            Canvas.Height = View.Height*View.Row;

            double width = Canvas.ActualWidth/View.Col;
            double height = Canvas.ActualHeight/View.Row;

            width = View.Width;
            height = View.Height;

            for (int i = 0; i < View.Row; i++)
            {
                for (int j = 0; j < View.Col; j++)
                {
                    _rectangle[i,j]=new Rectangle()
                    {
                        Width = width,
                        Height = height,
                        Fill = View.FillSolidColor,
                        Stroke = View.StrokeSolidColor,
                        Margin = new Thickness(width*j, height * i, 0,0)
                    };
                    Binding bind = new Binding()
                    {
                        Path = new PropertyPath("Solid[" + i + "," + j + "].SolidColor"),
                        Mode = BindingMode.OneWay
                    };
                    _rectangle[i, j].DataContext = View;
                    _rectangle[i, j].SetBinding(Shape.FillProperty, bind);
                    Canvas.Children.Add(_rectangle[i, j]);
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

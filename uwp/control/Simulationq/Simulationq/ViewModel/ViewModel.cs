using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Simulationq.ViewModel
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {
            FillSolidColor = new SolidColorBrush(Colors.Gray);
            StrokeSolidColor = new SolidColorBrush(Colors.LightCoral);
           
            Width = 20;
            Height = 20;

            Col= (int) (Window.Current.Bounds.Width/Width) -10;
            Row = (int) (Window.Current.Bounds.Height/Height) -10;
           // Col = 50;
//            Row = 60;

            Solid =new Solid[Row*Col];
            for (int i = 0; i < Row*Col; i++)
            {
                Solid[i] = new Solid();
                //for (int j = 0; j < Col; j++)
                //{
                //    Solid[i,j]=new Solid();
                //}
            }
            foreach (var temp in Solid)
            {
                temp.SolidColor = FillSolidColor;
                  // = new SolidColorBrush(Colors.Gray);
            }

            Solid[15].SolidColor = StrokeSolidColor;
        }

        public int Width
        {
            set
            {
                _width = value;
                OnPropertyChanged();
            }
            get
            {
                return _width;
            }
        }

        public int Height
        {
            set
            {
                _height = value;
                OnPropertyChanged();
            }
            get
            {
                return _height;
            }
        }



        public SolidColorBrush FillSolidColor
        {
            set
            {
                _fillSolidColor = value;
                OnPropertyChanged();
            }
            get
            {
                return _fillSolidColor;
            }
        }

        public SolidColorBrush StrokeSolidColor
        {
            set
            {
                _strokeSolidColor = value;
                OnPropertyChanged();
            }
            get
            {
                return _strokeSolidColor;
            }
        }

        private int _height;

        private int _width;

        public int Col
        {
            set
            {
                _col = value;
                OnPropertyChanged();
            }
            get
            {
                return _col;
            }
        }

        public int Row
        {
            set
            {
                _row = value;
                OnPropertyChanged();
            }
            get
            {
                return _row;
            }
        }

        public Solid[] Solid
        {
            set;
            get;
        }



        private SolidColorBrush _strokeSolidColor;

        private SolidColorBrush _fillSolidColor;

        private int _row;

        private int _col;
    }

    public class Solid:NotifyProperty
    {
        public SolidColorBrush SolidColor
        {
            set
            {
                _solidColor = value;
                OnPropertyChanged();
            }
            get
            {
                return _solidColor;
            }
        }

        

        private SolidColorBrush _solidColor;
    }
}

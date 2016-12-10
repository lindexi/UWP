using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Simulationq.ViewModel
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {
            FillSolidColor = new SolidColorBrush(Colors.Gray);
            StrokeSolidColor = new SolidColorBrush(Colors.LightCoral);
            Col = 10;
            Row = 20;
            Width = 10;
            Height = 10;
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





        private SolidColorBrush _strokeSolidColor;

        private SolidColorBrush _fillSolidColor;

        private int _row;

        private int _col;
    }
}

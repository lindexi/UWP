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
        //下一步
        //自动

        public ViewModel()
        {
            FillSolidColor = new SolidColorBrush(Colors.Gray);
            StrokeSolidColor = new SolidColorBrush(Colors.LightCoral);

            Width = 5;
            Height = 5;

            Col = (int) (Window.Current.Bounds.Width / Width) - 10;
            Row = (int) (Window.Current.Bounds.Height / Height) - 10;


            Solid = new Solid[Row * Col];

            solid = new SolidColorBrush(new Color()
            {
                A = 255,
                R = 5,
                B = 3,
                G = 6
            });


            for (int i = 0; i < Row * Col; i++)
            {
                Solid[i] = new Solid();
            }

            //foreach (var temp in Solid)
            //{
            //    temp.SolidColor = FillSolidColor;
            //}



            new Task(async () =>
            {
                while (true)
                {
                    Next();

                    await Task.Delay(1);
                }
            }).Start();
        }

        private void Next()
        {
            //bool _true = false;




            Parallel.For(0, Row * Col, (i, e) =>
            {
                var n = ZhenDev(i);
                if (!Solid[i].WeizCsefsimile)
                {
                    if (n == 3)
                    {
                        Solid[i].Nextcun = true;
                        //_true = true;
                    }
                }
                else
                {
                    if (n > 3 || n < 2)
                    {
                        Solid[i].Nextcun = false;
                        //_true = true;
                    }
                }
            });
            //for (int i = 0; i < Row * Col; i++)
            //{
            //    n = ZhenDev(i);
            //    if (!Solid[i].WeizCsefsimile)
            //    {
            //        if (n == 3)
            //        {
            //            Solid[i].Nextcun = true;
            //            _true = true;
            //        }
            //    }
            //    else
            //    {
            //        if (n > 3 || n < 2)
            //        {
            //            Solid[i].Nextcun = false;
            //            _true = true;
            //        }
            //    }
            //}

            //if (!_true)
            //{
            //    Ealiang();
            //}

            foreach (var temp in Solid)
            {
                temp.WeizCsefsimile = temp.Nextcun;
            }

            //foreach (var temp in Solid)
            //{
            //    if (temp.WeizCsefsimile && temp.SolidColor != solid)
            //    {
            //        temp.SolidColor = solid;
            //    }
            //    else if (temp.WeizCsefsimile == false && temp.SolidColor != FillSolidColor)
            //    {
            //        temp.SolidColor = FillSolidColor;
            //    }
            //}

            NextUmShi?.Invoke(this, null);
            //return n;
        }

        private SolidColorBrush solid
        {
            get; set;
        }

        public void Ealiang()
        {
            var n = Row * Col / 2 - Solid.Count(temp => temp.WeizCsefsimile);
            n = _ran.Next(n / 10);

            for (int i = 0; i < n; i++)
            {
                var temp = Solid[_ran.Next(Row) * Col + _ran.Next(Col / 2)];
                if (temp.WeizCsefsimile)
                {
                    //i--;
                }
                else
                {
                    //temp.SolidColor = solid;
                    temp.WeizCsefsimile = true;
                    temp.Nextcun = true;
                }
            }
        }

        public EventHandler NextUmShi;

        public Action Bjie
        {
            set;
            get;
        }

        private int ZhenDev(int n)
        {
            int row = n / Col;
            int col = n - row * Col;

            //1 2 3
            //4 5 6
            //7 8 9
            int s = 0;
            //1
            if (row - 1 >= 0 && col - 1 >= 0)
            {
                n = (row - 1) * Col + (col - 1);
                if (Solid[n].WeizCsefsimile)
                {
                    s++;
                }
            }

            //2
            if (row - 1 >= 0)
            {
                n = (row - 1) * Col + col;
                if (Solid[n].WeizCsefsimile)
                {
                    s++;
                }
            }

            //3
            if (row - 1 >= 0 && col + 1 < Col)
            {
                n = (row - 1) * Col + (col + 1);
                if (Solid[n].WeizCsefsimile)
                {
                    s++;
                }
            }

            //4
            if (row >= 0 && col - 1 >= 0)
            {
                n = (row) * Col + (col - 1);
                if (Solid[n].WeizCsefsimile)
                {
                    s++;
                }
            }

            //6
            if (row >= 0 && col + 1 < Col)
            {
                n = (row) * Col + (col + 1);
                if (Solid[n].WeizCsefsimile)
                {
                    s++;
                }
            }

            //7
            if (row + 1 < Row && col - 1 >= 0)
            {
                n = (row + 1) * Col + (col - 1);
                if (Solid[n].WeizCsefsimile)
                {
                    s++;
                }
            }

            //8
            if (row + 1 < Row && col >= 0)
            {
                n = (row + 1) * Col + (col);
                if (Solid[n].WeizCsefsimile)
                {
                    s++;
                }
            }

            //9
            if (row + 1 < Row && col + 1 < Col)
            {
                n = (row + 1) * Col + (col + 1);
                if (Solid[n].WeizCsefsimile)
                {
                    s++;
                }
            }

            return s;
        }

        private Random _ran = new Random();

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

        //环境时间

        private SolidColorBrush _strokeSolidColor;

        private SolidColorBrush _fillSolidColor;

        private int _row;

        private int _col;
    }

    public class Solid : NotifyProperty
    {
        //public SolidColorBrush SolidColor
        //{
        //    set
        //    {
        //        _solidColor = value;
        //        OnPropertyChanged();
        //    }
        //    get
        //    {
        //        return _solidColor;
        //    }
        //}

        public bool WeizCsefsimile
        {
            set; get;
        }

        public bool Nextcun
        {
            set; get;
        }

        //public void Rsolid()
        //{
        //    WeizCsefsimile = !WeizCsefsimile;
        //    SolidColor = WeizCsefsimile ? UsolidColorBrush : FsolidColorBrush;
        //}

        //public SolidColorBrush UsolidColorBrush
        //{
        //    set;
        //    get;
        //}

        //public SolidColorBrush FsolidColorBrush
        //{
        //    set;
        //    get;
        //}


        //private SolidColorBrush _solidColor;
    }



    public class Account
    {
        public Account()
        {

        }

        //善意

        //恶意

        //扫描

    }
}

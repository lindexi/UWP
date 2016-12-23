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
            bool d = true;
            d = false;
            FillSolidColor = new SolidColorBrush(Colors.Gray);
            StrokeSolidColor = new SolidColorBrush(Colors.LightCoral);

            Width = 10;
            Height = 10;

            Col = (int)(Window.Current.Bounds.Width / Width) - 10;
            Row = (int)(Window.Current.Bounds.Height / Height) - 10;

            //if (d)
            //{
            //    Col = 6;
            //    Row = 4;
            //}

            Solid = new Solid[Row * Col];

            SolidColorBrush solid = new SolidColorBrush(new Color()
            {
                A = 255,
                R = 5,
                B = 3,
                G = 6
            });


            for (int i = 0; i < Row * Col; i++)
            {
                Solid[i] = new Solid()
                {
                    UsolidColorBrush = solid,
                    FsolidColorBrush = FillSolidColor
                };
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

            //Solid[15].SolidColor = StrokeSolidColor;



            if (d)
            {
                // 1 1 0
                // 0 0 0
                // 1 1 0 
                //List<int> temp=new List<int>()
                //{
                //    1,2,7,8
                //};
                foreach (var temp in new List<int>()
                {
                    //9,10,14,17,21,22

                    //2 *Col+3,2*Col+4,
                    //3*Col+2,3*Col+5,
                    //4*Col+3,4*Col+4

                    3 *Col+2,3*Col+3,3*Col+4

                })
                {
                    Solid[temp - 1].SolidColor = solid;
                }
            }

            //         return;


            new Task(async () =>
            {
                //await Task.Delay(5000);

                //Random ran = new Random();
                var n = 0;
                while (true)
                {
                    bool _true = false;
                    //Parallel.For(0, Row * Col, ((i, state) =>
                    //  {
                    //      if (i == 2 * Col)
                    //      {

                    //      }
                    //      n = ZhenDev(i, solid);
                    //      if (Solid[i].SolidColor != solid)
                    //      {
                    //          if (n == 3)
                    //          {
                    //              Solid[i].WeizCsefsimile = true;
                    //              //_true = true;
                    //          }
                    //      }
                    //      else
                    //      {
                    //          if (n > 3 || n < 2)
                    //          {
                    //              Solid[i].WeizCsefsimile = false;
                    //             // _true = true;
                    //          }
                    //      }
                    //  }));

                    for (int i = 0; i < Row * Col; i++)
                    {
                        if (i == 2 * Col)
                        {

                        }
                        n = ZhenDev(i, solid);
                        if (Solid[i].SolidColor != solid)
                        {
                            //if (n == 2 || n == 3)
                            if (n == 3)
                            {
                                //if (_ran.Next(2) == 0)
                                {
                                    //Solid[i].SolidColor = solid;
                                    Solid[i].WeizCsefsimile = true;
                                }
                                _true = true;
                            }
                        }
                        else
                        {
                            //if (n != 2 || n!=3)
                            if (n > 3 || n < 2)
                            {
                                //Solid[i].SolidColor = FillSolidColor;
                                Solid[i].WeizCsefsimile = false;
                                _true = true;
                            }
                            //if (n == 3)
                            //{
                            //    if (_ran.Next(2) == 0)
                            //    {
                            //        Solid[i].SolidColor = FillSolidColor;
                            //    }
                            //    _true = true;
                            //}
                        }
                    }

                    if (!_true )
                    {
                        _true = true;
                        if (Solid.All(temp => temp.SolidColor != solid))
                        {

                        }
                        else
                        {
                            Bjie?.Invoke();
                            await Task.Delay(3000);
                        }

                        //for (int i = 0; i < Row * Col; i++)
                        //{
                        //    Solid[i].SolidColor = FillSolidColor;
                        //}
                        n = Row * Col / 2 - Solid.Count(temp => temp.SolidColor == solid);
                        n = _ran.Next(n / 10);

                        for (int i = 0; i < n; i++)
                        {
                            var temp = Solid[_ran.Next(Row) * Col + _ran.Next(Col / 2)];
                            if (temp.SolidColor == solid)
                            {
                                //i--;
                            }
                            else
                            {
                                //temp.SolidColor = solid;
                                temp.WeizCsefsimile = true;
                            }
                        }
                    }

                    foreach (var temp in Solid)
                    {
                        if (temp.WeizCsefsimile && temp.SolidColor != solid)
                        {
                            temp.SolidColor = solid;
                        }
                        else if (temp.WeizCsefsimile == false && temp.SolidColor != FillSolidColor)
                        {
                            temp.SolidColor = FillSolidColor;
                        }
                        //temp.SolidColor = temp.WeizCsefsimile ? solid : FillSolidColor;
                        //temp.WeizCsefsimile = false;
                    }

                    //int n = Solid.Count(temp => temp.SolidColor == solid);

                    //for (int i = 0; i < 2; i++)
                    //{
                    //    var temp = Solid[_ran.Next(Row * Col)];
                    //    if (temp.SolidColor == solid)
                    //    {
                    //        //i--;
                    //    }
                    //    else
                    //    {
                    //        temp.SolidColor = solid;
                    //    }
                    //}

                    await Task.Delay(100);
                }
            }).Start();
        }

        public Action Bjie
        {
            set;
            get;
        }

        private int ZhenDev(int n, SolidColorBrush solid)
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
                if (Solid[n].SolidColor == solid)
                {
                    s++;
                }
            }

            //2
            if (row - 1 >= 0)
            {
                n = (row - 1) * Col + col;
                if (Solid[n].SolidColor == solid)
                {
                    s++;
                }
            }

            //3
            if (row - 1 >= 0 && col + 1 < Col)
            {
                n = (row - 1) * Col + (col + 1);
                if (Solid[n].SolidColor == solid)
                {
                    s++;
                }
            }

            //4
            if (row >= 0 && col - 1 >= 0)
            {
                n = (row) * Col + (col - 1);
                if (Solid[n].SolidColor == solid)
                {
                    s++;
                }
            }

            //6
            if (row >= 0 && col + 1 < Col)
            {
                n = (row) * Col + (col + 1);
                if (Solid[n].SolidColor == solid)
                {
                    s++;
                }
            }

            //7
            if (row + 1 < Row && col - 1 >= 0)
            {
                n = (row + 1) * Col + (col - 1);
                if (Solid[n].SolidColor == solid)
                {
                    s++;
                }
            }

            //8
            if (row + 1 < Row && col >= 0)
            {
                n = (row + 1) * Col + (col);
                if (Solid[n].SolidColor == solid)
                {
                    s++;
                }
            }

            //9
            if (row + 1 < Row && col + 1 < Col)
            {
                n = (row + 1) * Col + (col + 1);
                if (Solid[n].SolidColor == solid)
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

        public bool WeizCsefsimile
        {
            set
            {
                _weizCsefsimile = value;
                //OnPropertyChanged();
            }
            get
            {
                return _weizCsefsimile;
            }
        }

        public void Rsolid()
        {
            WeizCsefsimile = !WeizCsefsimile;
            SolidColor = WeizCsefsimile ? UsolidColorBrush : FsolidColorBrush;
        }

        public SolidColorBrush UsolidColorBrush
        {
            set;
            get;
        }

        public SolidColorBrush FsolidColorBrush
        {
            set;
            get;
        }

        private bool _weizCsefsimile;


        private SolidColorBrush _solidColor;
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

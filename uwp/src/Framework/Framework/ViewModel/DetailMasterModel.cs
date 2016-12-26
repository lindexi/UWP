using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Framework.ViewModel
{
    public class DetailMasterModel : NotifyProperty
    {
        public DetailMasterModel()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
            Narrow();
        }

        public int GridInt
        {
            set
            {
                _gridInt = value;
                OnPropertyChanged();
            }
            get
            {
                return _gridInt;
            }
        }

        public int ZFrame
        {
            set
            {
                _zFrame = value;
                OnPropertyChanged();
            }
            get
            {
                return _zFrame;
            }
        }

        public GridLength MasterGrid
        {
            set
            {
                _masterGrid = value;
                OnPropertyChanged();
            }
            get
            {
                return _masterGrid;
            }
        }

        public GridLength DetailGrid
        {
            set
            {
                _detailGrid = value;
                OnPropertyChanged();
            }
            get
            {
                return _detailGrid;
            }
        }

        public int ZListView
        {
            set
            {
                _zListView = value;
                OnPropertyChanged();
            }
            get
            {
                return _zListView;
            }
        }

        public bool HasFrame { set; get; }

        public Visibility Visibility
        {
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
            get
            {
                return _visibility;
            }
        }

        //public void MasterClick(object o, ItemClickEventArgs e)
        //{
        //    //AddressBook temp = e.ClickedItem as AddressBook;
        //    //if (temp == null)
        //    //{
        //    //    return;
        //    //}
        //    HasFrame = true;
        //    Visibility=Visibility.Visible;
        //    //Detail.Navigate(typeof(DetailPage), temp.Str);
        //    Narrow();
        //}

        public void MasterClick()
        {
            HasFrame = true;
            Visibility = Visibility.Visible;
            Narrow();
        }

        private Visibility _visibility = Visibility.Collapsed;

        private int _zListView;

        private GridLength _detailGrid;

        private GridLength _masterGrid;

        private int _zFrame;

        private int _gridInt;

        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            HasFrame = false;
            Visibility = Visibility.Collapsed;
            Narrow();
        }

        public void Narrow()
        {
            if (Window.Current.Bounds.Width < 720)
            {
                MasterGrid = new GridLength(1, GridUnitType.Star);
                DetailGrid = GridLength.Auto;
                GridInt = 0;
                if (HasFrame)
                {
                    ZListView = 0;
                }
                else
                {
                    ZListView = 2;
                }
            }
            else
            {
                MasterGrid = GridLength.Auto;
                DetailGrid = new GridLength(1, GridUnitType.Star);
                GridInt = 1;
            }
        }
    }
}
#if  WINDOWS_UWP
using Windows.UI.Core;
using Windows.UI.Xaml;
#elif wpf 
using System.Windows;
#endif



namespace lindexi.uwp.Framework.ViewModel.MasterDetail
{
    public class DetailMasterModel : NotifyProperty
    {
        private GridLength _detailGrid;

        private int _gridInt;

        private GridLength _masterGrid;

        private Visibility _visibility = Visibility.Collapsed;

        private int _zFrame;

        private int _zListView;

        public DetailMasterModel()
        {
#if WINDOWS_UWP
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
#endif
            Narrow();
        }

        public int GridInt
        {
            set
            {
                _gridInt = value;
                OnPropertyChanged();
            }
            get { return _gridInt; }
        }

        public int ZFrame
        {
            set
            {
                _zFrame = value;
                OnPropertyChanged();
            }
            get { return _zFrame; }
        }

        public GridLength MasterGrid
        {
            set
            {
                _masterGrid = value;
                OnPropertyChanged();
            }
            get { return _masterGrid; }
        }

        public GridLength DetailGrid
        {
            set
            {
                _detailGrid = value;
                OnPropertyChanged();
            }
            get { return _detailGrid; }
        }

        public int ZListView
        {
            set
            {
                _zListView = value;
                OnPropertyChanged();
            }
            get { return _zListView; }
        }

        public bool HasFrame { set; get; }

        public Visibility Visibility
        {
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
            get { return _visibility; }
        }

        public void MasterClick()
        {
            HasFrame = true;
            Visibility = Visibility.Visible;
            Narrow();
        }

        public void Narrow()
        {
#if WINDOWS_UWP
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
#endif

        }

#if WINDOWS_UWP
        private void BackRequested(object sender, BackRequestedEventArgs e)
        {
            HasFrame = false;
            Visibility = Visibility.Collapsed;
            Narrow();
        } 
#endif

    }
}
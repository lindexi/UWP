// lindexi

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageMoseClick.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            View = this;
        }

        public Visibility FrameVisibility
        {
            set
            {
                _frameVisibility = value;
                OnPropertyChanged();
            }
            get
            {
                return _frameVisibility;
            }
        }

        public ViewModel View
        {
            set;
            get;
        }

        public async void Read()
        {
            FrameVisibility = Visibility.Collapsed;
#if NOGUI
#else
            // Content.Navigate(typeof(SplashPage));
#endif
            //ViewModel

            Image = new BitmapImage(new Uri("ms-appx:///assets/1.png"));
            //FileOpenPicker pick=new FileOpenPicker();
            //pick.FileTypeFilter.Add(".png");

            //StorageFile image = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///assets/1.png"));
            //image = await pick.PickSingleFileAsync();
            
            //await Image.SetSourceAsync(await image.OpenAsync(FileAccessMode.Read));
        }

        public BitmapImage Image
        {
            set
            {
                _image = value;
                OnPropertyChanged();
            }
            get
            {
                return _image;
            }
        }

        

        private BitmapImage _image;

        public void NavigateToInfo()
        {
        }

        public void NavigateToAccount()
        {
        }

        public override void OnNavigatedFrom(object obj)
        {
            throw new NotImplementedException();
        }

        public override void OnNavigatedTo(object obj)
        {
            throw new NotImplementedException();
        }


        private Visibility _frameVisibility;
    }
}
// lindexi

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PivoHeader.ViewModel
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

        public void Read()
        {
            FrameVisibility = Visibility.Collapsed;
#if NOGUI
#else
            // Content.Navigate(typeof(SplashPage));
#endif
            //ViewModel
        }

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
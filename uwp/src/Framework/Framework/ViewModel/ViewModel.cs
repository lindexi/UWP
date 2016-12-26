// lindexi

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Framework.View;

namespace Framework.ViewModel
{
    public class AModel : ViewModelBase
    {
        public override void OnNavigatedFrom(object obj)
        {
            throw new NotImplementedException();
        }

        public override void OnNavigatedTo(object obj)
        {
            throw new NotImplementedException();
        }
    }

    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            View = this;


        }

        public AModel AModel
        {
            set;
            get;
        }

        public CodeStorageModel CodeStorageModel
        {
            set;
            get;
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
            CodeStorageModel = new CodeStorageModel();
            ViewModel.Add(new ViewModelPage(CodeStorageModel, typeof(MasterDetailPage))
            {


            });
            FrameVisibility = Visibility.Visible;
        }

        public void NavigateToList()
        {
            Navigateto(typeof(CodeStorageModel), null);
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
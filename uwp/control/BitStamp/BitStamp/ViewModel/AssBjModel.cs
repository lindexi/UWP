using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using lindexi.MVVM.Framework.ViewModel;

namespace BitStamp.ViewModel
{
    public class AssBjModel : NotifyProperty
    {
        public AssBjModel()
        {
            Account = AccoutGoverment.AccountModel;
        }

        public void NavigateStamp()
        {
            //StampPage 是用户控件
            //FrameAccount.Navigate(typeof(View.StampPage));
            FrameVisibility = Visibility.Visible;
        }

        public AccoutGoverment Account
        {
            set;
            get;
        }

        public void NavigateAccount()
        {
            FrameVisibility = Visibility.Collapsed;
        }

        public Frame FrameAccount
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
        private Visibility _frameVisibility;
    }
}

// lindexi
// 16:03

using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using BitStamp.Model;

namespace BitStamp.ViewModel
{
    public class Stamp : NotifyProperty
    {
        public Stamp()
        {
            Image = new BitmapImage(new Uri("ms-appx:///assets/QQ截图20160926151822.png"));
            Str = "blog.csdn.net/lindexi_gd";
            Visibility = Visibility.Collapsed;
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

        public StorageFile File
        {
            set;
            get;
        }

        public string Address
        {
            set
            {
                _address = value;
                OnPropertyChanged();
            }
            get
            {
                return _address;
            }
        }

        public string LinkReminder
        {
            set
            {
                _linkReminder = value;
                OnPropertyChanged();
            }
            get
            {
                return _linkReminder;
            }
        }

        public string Str
        {
            set
            {
                _str = value;
                OnPropertyChanged();
            }
            get
            {
                return _str;
            }
        }

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

        public void Jcloud()
        {
            JyUploadImage jyUploadImage = new JyUploadImage(File);
            jyUploadImage.OnUploaded += (s, e) =>
            {
                UploadImageTask uploadImage = s as UploadImageTask;
                Visibility = Visibility.Collapsed;
                Address = "上传" + File.Name;
                if (uploadImage == null)
                {
                    return;
                }
                if (e)
                {
                    LinkReminder = "![](" +
                                   uploadImage.Url + ")";
                }
                else
                {
                    Address += "失败";
                }
            };
            Visibility = Visibility.Visible;
            jyUploadImage.UploadImage();
        }

        private string _address;
        private BitmapImage _image;

        private string _linkReminder;

        private string _str;

        private Visibility _visibility;
    }
}
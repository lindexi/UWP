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
            //Image = new BitmapImage(new Uri("ms-appx:///assets/QQ截图20160926151822.png"));
            //#if DEBUG
            //            Str = "blog.csdn.net/lindexi_gd";
            //#endif

            Account = AccoutGoverment.AccountModel;

            //Str = Account.Account.Str;

            Visibility = Visibility.Collapsed;

         
        }

        public AccoutGoverment Account
        {
            set;
            get;
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
                AccoutGoverment.AccountModel.Account.Str = value;
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

        private UploadImageTask NewUploadImageTask(ImageShackEnum imageShack, StorageFile file)
        {
            switch (imageShack)
            {
                case ImageShackEnum.Jiuyou:
                    return new JyUploadImage(file);
                case ImageShackEnum.Smms:
                    return new SmmsUploadImage(file);
                case ImageShackEnum.Qin:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(imageShack), imageShack, null);
            }
            return new JyUploadImage(file);
        }

        public void Jcloud()
        {
            UploadImageTask uploadImageTask = NewUploadImageTask(
                AccoutGoverment.AccountModel.Account.ImageShack, File);
            uploadImageTask.OnUploaded += (s, e) =>
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
            uploadImageTask.UploadImage();
        }

        private string _address;
        private BitmapImage _image;

        private string _linkReminder;

        private string _str;

        private Visibility _visibility;
    }
}
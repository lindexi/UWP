// lindexi
// 16:03

using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using BitStamp.Model;
using BitStamp.Model.Cimage;
using lindexi.uwp.ImageShack.Model;

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
                    return new QnUploadImage(file)
                    {
                        Accound = Account.Account.CloundAccound
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(imageShack), imageShack, null);
            }
            //return new JyUploadImage(file);
        }

        public async Task Jcloud()
        {
            
            Cimage image=new Cimage(File);
            image.UploadImage();
            return;

            ImageShackEnum imageShack = AccoutGoverment.AccountModel.Account.ImageShack;
            if (File.FileType == ".gif" && imageShack == ImageShackEnum.Jiuyou)
            {
                imageShack = ImageShackEnum.Qin;
            }
            var size= (await File.GetBasicPropertiesAsync()).Size;
            //1M
            //1024k
            //‪125000‬
            if (size > 125000)
            {
                imageShack = ImageShackEnum.Smms;
            }
            //4326  24,447 

            UploadImageTask uploadImageTask = NewUploadImageTask(
               imageShack, File);
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
                    //LinkReminder = "![](" +
                    //               uploadImage.Url + ")";
                    //Bcode = $"[img]{uploadImage.Url}[/img]";
                    Url = uploadImage.Url;
                }
                else
                {
                    Address += "失败";
                }
            };
            Visibility = Visibility.Visible;
            uploadImageTask.UploadImage();
        }

        public string Bcode
        {
            set
            {
                _bcode = value;
                OnPropertyChanged();
            }
            get
            {
                return _bcode;
            }
        }

        public string Url
        {
            set
            {
                _url = value;
                OnPropertyChanged();
                LinkReminder = "![](" +
                                  value + ")";
                Bcode = $"[img]{value}[/img]";
            }
            get
            {
                return _url;
            }
        }
        private string _url;
        private string _bcode;

        private string _address;
        private BitmapImage _image;

        private string _linkReminder;

        private string _str;

        private Visibility _visibility;
    }
}
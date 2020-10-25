// lindexi
// 16:26

using System;
using System.Threading.Tasks;
using lindexi.MVVM.Framework.ViewModel;
using lindexi.uwp.ImageShack.Model;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

namespace BitStamp.ViewModel
{
    public class Account : NotifyProperty
    {
        public Account()
        {

        }



        public ElementTheme Theme
        {
            set
            {
                _theme = value;
                OnPropertyChanged();
            }
            get
            {
                return _theme;
            }
        }

        public bool ThemeDay
        {
            set
            {
                _day = value;
                Theme = value ? ElementTheme.Light : ElementTheme.Dark;
                OnPropertyChanged();
            }
            get
            {
                return _day;
            }
        }

        [JsonIgnore]
        public StorageFolder Folder
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

        public string Token
        {
            set;
            get;
        }

        public ImageShackEnum ImageShack
        {
            set
            {
                if (value == ImageShackEnum.NoShack)
                {
                    return;
                }

                _imageShack = value;
                OnPropertyChanged();
            }
            get => _imageShack;
        }

        public bool? SmmsImageShack
        {
            set
            {
                if (_smmsImageShack == value)
                {
                    return;
                }

                if (value == true)
                {
                    ImageShack = ImageShackEnum.Smms;
                }
                OnPropertyChanged();
            }
            get
            {
                return _smmsImageShack = ImageShack == ImageShackEnum.Smms;
            }
        }

        public bool? QinImageShack
        {
            set
            {
                if (_qinImageShack == value)
                {
                    return;
                }

                _qinImageShack = value;
                if (value == true)
                {
                    ImageShack = ImageShackEnum.Qin;
                }
                QinImageShackVisibility = value == true ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged();
                //OnPropertyChanged(nameof(QinImageShackVisibility));
            }
            get
            {
                return _qinImageShack = ImageShack == ImageShackEnum.Qin;
            }
        }

        public Visibility QinImageShackVisibility
        {
            set
            {
                _qinImageShackVisibility = value;
                OnPropertyChanged();
            }
            get
            {
                return _qinImageShackVisibility;
            }
        }

        private Visibility _qinImageShackVisibility;
        public bool? JiuYouImageShack
        {
            set
            {
                if (_jiuYouImageShack == value)
                {
                    return;
                }

                _jiuYouImageShack = value;
                if (value == true)
                {
                    ImageShack = ImageShackEnum.Jiuyou;
                }
                JiuYouImageShackVisibility = value == true ? Visibility.Visible :
                Visibility.Collapsed;
                OnPropertyChanged();
            }
            get
            {
                return _jiuYouImageShack = ImageShack == ImageShackEnum.Jiuyou;
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

        private Visibility _jiuYouImageShackVisibility;

        public async void OpenPicesrFolder()
        {
            FolderPicker pick = new FolderPicker();
            pick.FileTypeFilter.Add(".png");
            var folder = await pick.PickSingleFolderAsync();
            if (folder != null)
            {
                Folder = folder;
                Address = folder.Path;
                Token = StorageApplicationPermissions.FutureAccessList.Add(folder);
            }
        }

        public string JiuYouId
        {
            set;
            get;
        }

        public string JiuYouSecretId
        {
            set;
            get;
        }

        public Visibility JiuYouImageShackVisibility
        {
            set
            {
                _jiuYouImageShackVisibility = value;
                OnPropertyChanged();
            }
            get
            {
                return _jiuYouImageShackVisibility;
            }
        }

        public CloundesAccound CloundAccound
        {
            set;
            get;
        } = new CloundesAccound();

        private string _address;

        private bool _day;

        private bool? _jiuYouImageShack;
        private bool? _qinImageShack;
        private bool? _smmsImageShack;
        private string _str;

        private ElementTheme _theme = ElementTheme.Light;
        private ImageShackEnum _imageShack;
    }

    public enum ImageShackEnum
    {
        Jiuyou,
        Smms,
        Qin,
        Cimage,
        NoShack
    }
}

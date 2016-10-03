// lindexi
// 16:26

using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Newtonsoft.Json;

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
            set;
            get;
        }

        public bool? SmmsImageShack
        {
            set
            {
                _smmsImageShack = value;
                if (value == true)
                {
                    ImageShack = ImageShackEnum.Smms;
                }
                OnPropertyChanged();
            }
            get
            {
                return _smmsImageShack;
            }
        }

        public bool? QinImageShack
        {
            set
            {
                _qinImageShack = value;
                if (value == true)
                {
                    ImageShack = ImageShackEnum.Qin;
                }
                OnPropertyChanged();
            }
            get
            {
                return _qinImageShack;
            }
        }

        public bool? JiuYouImageShack
        {
            set
            {
                _jiuYouImageShack = value;
                if (value == true)
                {
                    ImageShack = ImageShackEnum.Jiuyou;
                }
                OnPropertyChanged();
            }
            get
            {
                return _jiuYouImageShack;
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

        public async void OpenPicesrFolder()
        {
            FolderPicker pick=new FolderPicker();
            pick.FileTypeFilter.Add(".png");
            var folder =await pick.PickSingleFolderAsync();
            if (folder != null)
            {
                Folder = folder;
                Address = folder.Path;
                Token=StorageApplicationPermissions.FutureAccessList.Add(folder);
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

        private string _address;

        private bool _day;

        private bool? _jiuYouImageShack;
        private bool? _qinImageShack;
        private bool? _smmsImageShack;
        private string _str;

        private ElementTheme _theme = ElementTheme.Light;
    }

    public enum ImageShackEnum
    {
        Jiuyou,
        Smms,
        Qin
    }
}
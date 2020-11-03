using EncryptionSyncFolder.ViewModel;
using Windows.UI.Xaml.Media.Imaging;

namespace EncryptionSyncFolder.Model
{
    public abstract class VirtualStorage : NotifyProperty
    {
        public VirtualStorage()
        {

        }

        private bool _check;

        public bool Check
        {
            set
            {
                _check = value;
                OnPropertyChanged();
            }
            get
            {
                return _check;
            }
        }

        private BitmapImage _bitmap;

        public BitmapImage Bitmap
        {
            set
            {
                _bitmap = value;
                OnPropertyChanged();
            }
            get
            {
                return _bitmap;
            }
        }

        public abstract void Rename();
        public abstract void ToFolder();


        /// <summary>
        ///     文件名
        /// </summary>
        public string Name
        {
            set
            {
                _name = value;
                OnPropertyChanged();
            }
            get
            {
                return _name;
            }
        }

        /// <summary>
        ///     文件路径
        /// </summary>
        public string Path
        {
            set
            {
                _path = value;
                OnPropertyChanged();
            }
            get
            {
                return _path;
            }
        }

        /// <summary>
        ///     文件大小
        /// </summary>
        public string Size
        {
            set
            {
                _size = value;
                OnPropertyChanged();
            }
            get
            {
                return _size;
            }
        }

        /// <summary>
        ///     创建时间
        /// </summary>
        public string NewTime
        {
            set
            {
                _newTime = value;
                OnPropertyChanged();
            }
            get
            {
                return _newTime;
            }
        }

        public VirtualFileFolderEnum VirtualFileFolder
        {
            set
            {
                _virtualFileFolder = value;
                OnPropertyChanged();
            }
            get
            {
                return _virtualFileFolder;
            }
        }
        private VirtualFileFolderEnum _virtualFileFolder;
        public enum VirtualFileFolderEnum
        {
            File,
            Folder
        }
        private string _name;
        private string _newTime;
        private string _path;
        private string _size;
    }
}

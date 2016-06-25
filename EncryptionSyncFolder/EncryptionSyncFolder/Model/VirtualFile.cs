// lindexi
// 21:07

using Windows.Storage;
using EncryptionSyncFolder.ViewModel;

namespace EncryptionSyncFolder.Model
{
    /// <summary>
    ///     虚拟文件
    /// </summary>
    public class VirtualFile : NotifyProperty
    {
        public VirtualFile()
        {
        }

        public StorageFile File
        {
            set
            {
                _file = value;
                OnPropertyChanged();
            }
            get
            {
                return _file;
            }
        }

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

        private StorageFile _file;
        private string _name;
        private string _newTime;
        private string _path;
        private string _size;
    }
}
// lindexi
// 21:07

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using EncryptionSyncFolder.ViewModel;

namespace EncryptionSyncFolder.Model
{
    /// <summary>
    ///     虚拟文件夹
    /// </summary>
    public class VirtualFolder : NotifyProperty
    {
        public VirtualFolder()
        {

        }

        public StorageFolder FolderStorage
        {
            set
            {
                _folderStorage = value;
                OnPropertyChanged();
            }
            get
            {
                return _folderStorage;
            }
        }

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

        public List<VirtualFile> File
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

        public List<VirtualFolder> Folder
        {
            set
            {
                _folder = value;
                OnPropertyChanged();
            }
            get
            {
                return _folder;
            }
        }
        /// <summary>
        /// 新建文件
        /// </summary>
        public void NewFile()
        {
            //文件存在

        }

        public void NewFolder()
        {
            
        }

        public void RenameFile()
        {
            
        }

        public void RenameFolder()
        {
            
        }

        public void DeleteFile()
        {
            
        }

        public void DeleteFolder()
        {
            
        }

        private List<VirtualFile> _file = new List<VirtualFile>();
        private List<VirtualFolder> _folder = new List<VirtualFolder>();
        private StorageFolder _folderStorage;
        private string _name;
        private string _path;
    }
}
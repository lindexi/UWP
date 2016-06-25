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
    public class VirtualFolder : VirtualStorage
    {
        public VirtualFolder()
        {
            VirtualFileFolder = VirtualFileFolderEnum.Folder;
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
        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
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
    }
}
// lindexi
// 21:07

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            Folder=new List<VirtualFolder>();
            File=new List<VirtualFile>();
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
        public async Task NewFile(VirtualFile file)
        {
            //文件存在
            string str= ".file";
            if (!file.Name.EndsWith(str))
            {
                file.Name += str;
            }

            //foreach (var temp in File)
            //{
            //    if (temp.Name == file.Name)
            //    {
            //        throw new Exception("文件存在");
            //    }
            //}

            if (File.Any(temp => temp.Name == file.Name))
            {
                throw new Exception("文件存在");
            }

            file.Path = Path + "/" + file.Name;
            file.NewTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            file.File = await FolderStorage.CreateFileAsync(file.Name);

            File.Add(file);
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
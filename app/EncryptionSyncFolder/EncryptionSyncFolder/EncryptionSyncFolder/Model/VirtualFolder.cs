// lindexi
// 21:07

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EncryptionSyncFolder.ViewModel;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

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
            Bitmap = FolderBitmap;
            Folder = new List<VirtualFolder>();
            File = new List<VirtualFile>();
        }

        public override void Rename()
        {

        }


        public override void ToFolder()
        {

        }

        private static BitmapImage FolderBitmap
        {
            set;
            get;
        } = new BitmapImage(new Uri("ms-appx:///Assets/folder_178px_1201076_easyicon.net.png"));
        [JsonIgnore]
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
        /// 列出所有文件、文件夹
        /// </summary>
        public async void AllFileFolder()
        {
            var fileFolder = await FolderStorage.GetItemsAsync();
            foreach (var temp in fileFolder)
            {
                if (temp.IsOfType(StorageItemTypes.File))
                {

                }
                else if (temp.IsOfType(StorageItemTypes.Folder))
                {

                }
            }
        }

        /// <summary>
        /// 新建文件
        /// </summary>
        public void NewFile(VirtualFile file)
        {
            //文件存在
            string str = ".file";
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
            //file.File = await FolderStorage.CreateFileAsync(file.Name);

            File.Add(file);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">文件夹名</param>
        public void NewFolder(string str)
        {
            //存在
            string name = str.Trim();
            if (Folder.Any(temp => temp.Name == str))
            {
                throw new Exception("文件夹存在");
            }
            VirtualFolder folder = new VirtualFolder()
            {
                Name = name,
                NewTime = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                Path = Path + "/" + name
            };
            Folder.Add(folder);
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

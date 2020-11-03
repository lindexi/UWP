// lindexi
// 21:07

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

namespace EncryptionSyncFolder.Model
{
    /// <summary>
    ///     虚拟文件
    /// </summary>
    public class VirtualFile : VirtualStorage
    {
        public VirtualFile()
        {
            VirtualFileFolder = VirtualFileFolderEnum.File;
            Bitmap = FileBitmap;
        }

        public override void Rename()
        {
            
        }

        public override void ToFolder()
        {
            
        }

        ///// <summary>
        ///// 从StorageFile转VirtualFile
        ///// </summary>
        ///// <param name="file"></param>
        //private void StorageFileVirtualFile(StorageFile file)
        //{

        //}

        [JsonIgnore]
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
        /// 字符串可以文件名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool AreFileName(string str)
        {
            List<string> notStr = new List<string>()
            {
                "\\",
                "/",
                ":",
                "?",
                "\"",
                "<",
                ">",
                "|"
            };

            //foreach (var temp in notStr)
            //{
            //    if (str.IndexOf(temp) > -1)
            //    {
            //        return false;
            //    }
            //}
            //return true;
            return notStr.All(temp => str.IndexOf(temp) <= -1);
        }
        private static BitmapImage FileBitmap
        {
            set;
            get;
        } = new BitmapImage(new Uri("ms-appx:///Assets/file_140px_1201060_easyicon.net.png"));
        private StorageFile _file;
    }
}
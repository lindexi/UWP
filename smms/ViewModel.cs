using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using smms.Model;

namespace smms
{
    public class ViewModel:NotifyProperty
    {
        public ViewModel()
        {

        }

        public string Reminder
        {
            set
            {
                _reminder = value;
                OnPropertyChanged();
            }
            get
            {
                return _reminder;
            }
        }

        private string _reminder;
        public StorageFile File
        {
            set;
            get;
        }
        public async void UpLoad()
        {
            //判断文件不存在
            //string name = "file.txt";
            //StorageFile file;
            //if (!await exist(name))
            //{
            //    int n = 100;
            //    string str = RanStr(n);
            //    file = await Write(name, str);
            //}
            //else
            //{
            //    file= await ApplicationData.Current.
            //        LocalFolder.GetFileAsync(name);
            //}

            if (File == null)
            {
                FileOpenPicker picker = new FileOpenPicker()
                {
                    FileTypeFilter = { ".png", ".jpg" }
                };

                File =await picker.PickSingleFileAsync();
            }

            Imageshack imageshack = new Imageshack()
            {
                File=File,
            };
            imageshack.OnUploadedEventHandler += (sender, str) => Reminder = str.Replace("\\/","/");
            imageshack.UpLoad();
        }
        /// <summary>
        /// 文件存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true 文件不存在 False文件存在</returns>
        private async Task<bool> exist(string name)
        {
            try
            {
                var file = await ApplicationData.Current.
                    LocalFolder.GetFileAsync(name);
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            return true;
        }
        private async Task<StorageFile> Write(string name, string str)
        {
            StorageFile file = await ApplicationData.Current.
                LocalFolder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists);
            using (Stream stream = await file.OpenStreamForWriteAsync())
            {
                byte[] buffer = Encoding.ASCII.GetBytes(str);
                stream.Write(buffer, 0, buffer.Length);
            }
            return file;
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private string RanStr(int n)
        {
            Random ran = new Random();
            return RanStr(n, ran);
        }

        private string RanStr(int n, Random ran)
        {
            StringBuilder str = new StringBuilder();
            int[] chinesecharacters = new int[2] { 19968, 40895 };
            for (int i = 0; i < n; i++)
            {
                str.Append(Convert.ToChar(ran.Next(chinesecharacters[0], chinesecharacters[1])));
            }
            return str.ToString();
        }
    }
}

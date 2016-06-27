// lindexi
// 17:07

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using EncryptionSyncFolder.ViewModel;
using Newtonsoft.Json;

namespace EncryptionSyncFolder.Model
{
    public class Account : NotifyProperty
    {
        public Account()
        {
        }

        /// <summary>
        ///     文件账户
        /// </summary>
        public List<Account> FileVirtualAccount
        {
            set;
            get;
        } = new List<Account>();

        /// <summary>
        ///     用户名
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
        ///     账户登录
        /// </summary>
        public bool AreAccountConfirm
        {
            set
            {
                _areAccountConfirm = value;
                OnPropertyChanged();
            }
            get
            {
                return _areAccountConfirm;
            }
        }

        /// <summary>
        ///     密码
        /// </summary>
        public string Key
        {
            set
            {
                _key = value;
                OnPropertyChanged();
            }
            get
            {
                return _key;
            }
        }

        public VirtualFolder Folder
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

        public EventHandler<NewAccountEnum> OnNewAccountEventHandler
        {
            set;
            get;
        }

        public EventHandler<ConfirmEnum> OnConfirmEventHandler
        {
            set;
            get;
        }

        public static Account AccountVirtual
        {
            set
            {
                _accountVirtual = value;
            }
            get
            {
                return _accountVirtual ?? (_accountVirtual = new Account());
            }
        }

        public enum ConfirmEnum
        {
            Success,
            AccountNotExist,
            KeyError
        }

        public enum NewAccountEnum
        {
            Success,
            AccountExist,
            AccountAreNull,
        }

        /// <summary>
        ///     创建账户
        /// </summary>
        public async void NewAccount()
        {
            if (string.IsNullOrEmpty(Name))
            {
                OnNewAccountEventHandler?.Invoke(this, NewAccountEnum.AccountAreNull);
                return;
            }

            var folder =
                await
                    ApplicationData.Current.LocalFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);
            //如果不存在文件夹
            try
            {
                folder = await folder.CreateFolderAsync(Name, CreationCollisionOption.FailIfExists);
                await folder.CreateFolderAsync("folder");
                var file = await folder.CreateFileAsync("account");
                Write(file, ToString());
                Folder = new VirtualFolder()
                {
                    Name = Name,
                    Path = "~",
                    FolderStorage = folder
                };
                AreAccountConfirm = true;
                //await Read();
                OnNewAccountEventHandler?.Invoke(this, NewAccountEnum.Success);
            }
            catch (Exception)
            {
                OnNewAccountEventHandler?.Invoke(this, NewAccountEnum.AccountExist);
                try
                {
                    //folder =
                    //    await
                    //        ApplicationData.Current.LocalFolder.CreateFolderAsync("data",
                    //            CreationCollisionOption.OpenIfExists);
                    //folder = await folder.CreateFolderAsync(Name, CreationCollisionOption.OpenIfExists);
                    //await folder.DeleteAsync();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        ///     保存用户文件和文件夹
        /// </summary>
        public async void Storage()
        {
            JsonSerializer json = JsonSerializer.Create();
            var file = await Folder.FolderStorage.CreateFileAsync("file1",
                CreationCollisionOption.ReplaceExisting);
            using (TextWriter stream = new StreamWriter(await file.OpenStreamForWriteAsync()))
            {
                json.Serialize(stream, Folder);
            }

            try
            {
                var last = await Folder.FolderStorage.GetFileAsync("file");
                await last.MoveAsync(Folder.FolderStorage, "last",
                    NameCollisionOption.ReplaceExisting);
            }
            catch (FileNotFoundException)
            {
            }

            await file.MoveAsync(Folder.FolderStorage, "file",
                NameCollisionOption.ReplaceExisting);
        }

        public async void Confirm()
        {
            if (string.IsNullOrEmpty(Name))
            {
                OnConfirmEventHandler?.Invoke(this, ConfirmEnum.AccountNotExist);
                return;
            }

            //用户存在
            var folder =
                await
                    ApplicationData.Current.LocalFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);
            try
            {
                folder = await folder.GetFolderAsync(Name);
            }
            catch (DirectoryNotFoundException)
            {
                OnConfirmEventHandler?.Invoke(this, ConfirmEnum.AccountNotExist);
                return;
            }
            catch (FileNotFoundException)
            {
                OnConfirmEventHandler?.Invoke(this, ConfirmEnum.AccountNotExist);
                return;
            }

            try
            {
                var file = await folder.GetFileAsync("account");
                //用户账户是否一样，一般都是一样
                string str;
                using (StreamReader stream =
                    new StreamReader(await file.OpenStreamForReadAsync()))
                {
                    str = stream.ReadLine();
                    if (Name != str)
                    {
                        //不会
                    }

                    str = stream.ReadLine();
                }
                if (Md5(Key) != str)
                {
                    OnConfirmEventHandler?.Invoke(this, ConfirmEnum.KeyError);
                    return;
                }
                AreAccountConfirm = true;
                Folder = new VirtualFolder()
                {
                    Name = Name,
                    Path = "~",
                    FolderStorage = folder
                };
                await Read();
                OnConfirmEventHandler?.Invoke(this, ConfirmEnum.Success);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("验证错误，系统错误，没有用户文件");
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(Name + "\n");
            str.Append(Md5(Key));
            return str.ToString();
        }

        private bool _areAccountConfirm;

        private VirtualFolder _folder;

        private string _key;

        private string _name;

        private async Task Read()
        {
            try
            {
                var file = await Folder.FolderStorage.GetFileAsync("file");
                var json = JsonSerializer.Create();
                var folder = Folder.FolderStorage;
                Folder = json.Deserialize<VirtualFolder>(
                    new JsonTextReader(
                        new StreamReader(await file.OpenStreamForReadAsync())));
                Folder.FolderStorage = folder;
            }
            catch (FileNotFoundException)
            {
            }
        }

        private async void Write(StorageFile file, string str)
        {
            using (TextWriter stream = new StreamWriter(await
                file.OpenStreamForWriteAsync()))
            {
                stream.Write(str);
            }
        }

        private string Md5(string str)
        {
            if (str == null)
            {
                str = "";
            }

            CryptographicHash cryptographic =
                HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5).CreateHash();
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf16BE);
            cryptographic.Append(buffer);
            buffer = cryptographic.GetValueAndReset();
            return CryptographicBuffer.EncodeToBase64String(buffer);
        }

        private static Account _accountVirtual;
    }
}
// lindexi
// 17:02

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Data.Xml.Dom;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using EncryptionSyncFolder.Model;
using EncryptionSyncFolder.View;

namespace EncryptionSyncFolder.ViewModel
{
    public class FileVirtualModel : NotifyProperty
    {
        public FileVirtualModel()
        {
            _accountVirtual = Account.AccountVirtual;
            Folder = _accountVirtual.AreAccountConfirm
                ? _accountVirtual.Folder
                : new VirtualFolder();

            ListVirtualStorage();
        }

        public Account AccountVirtual
        {
            set
            {
                _accountVirtual = value;
                OnPropertyChanged();
            }
            get
            {
                return _accountVirtual;
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

        public ObservableCollection<VirtualStorage> FileFolder
        {
            set;
            get;
        } = new ObservableCollection<VirtualStorage>();

        public VirtualStorage FileFolderVirtualStorage
        {
            set
            {
                _fileFolderVirtualStorage = value;
                OnPropertyChanged();
            }
            get
            {
                return _fileFolderVirtualStorage;
            }
        }

        //public void WriteFile()
        //{

        //}

        /// <summary>
        ///     进入文件夹
        /// </summary>
        public void ToFolder()
        {
            if (FileFolderVirtualStorage == null)
            {
                ToastText("没有选择文件夹");
                return;
            }
            if (!(FileFolderVirtualStorage is VirtualFolder))
            {
                ToastText("没有选择文件夹");
                return;
            }

            //Folder.ToFolder();
            BackFolder.Push(Folder);
            Folder = (VirtualFolder)FileFolderVirtualStorage;
            ListVirtualStorage();
        }

        public void BackVirtualFolder()
        {
            if (BackFolder.Count == 0)
            {
                ToastText("没有进入文件夹");
                return;
            }
            Folder = BackFolder.Pop();
            ListVirtualStorage();
        }

        public void Des()
        {
            //测试
            //md5
            //string str = "lindexi";
            //str = Cryptography.Md5ToBase64String(str);
            //    //+"\r\n"+
            ////    cryptography.Md5ToHexString(str);
            //str = "md5\r\n" + str;
            //ToastText(str);

            //str = "lindexi";
            //str = "md5\r\n" + Cryptography.Md5ToHexString(str);
            //ToastText(str);

            //des
            string str = "lindexi";
            string key = "qq123456";
            string plaintext = Cryptography.DesEncryption(key, str);
            plaintext = Cryptography.DesDecryption(key, plaintext);
            if (str == plaintext)
            {
                ToastText("密文和明文相同");
            }
        }

        public void Rename()
        {
            if (FileFolderVirtualStorage == null)
            {
                return;
            }
            var newFileDialog = new NewFolderDialog();
            ContentDialog contentDialog = new ContentDialog()
            {
                Title = "重命名",
                Content = newFileDialog,
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消"
            };

        }

        public void Delete()
        {
            if (FileFolderVirtualStorage is VirtualFile)
            {
                Folder.File.Remove(FileFolderVirtualStorage as VirtualFile);
            }
            if (FileFolderVirtualStorage is VirtualFolder)
            {
                Folder.Folder.Remove(FileFolderVirtualStorage as VirtualFolder);
            }
            ListVirtualStorage();
        }

        /// <summary>
        ///     列出
        /// </summary>
        public async void ListVirtualStorage()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    FileFolder.Clear();
                    foreach (var temp in Folder.Folder)
                    {
                        FileFolder.Add(temp);
                    }

                    foreach (var temp in Folder.File)
                    {
                        FileFolder.Add(temp);
                    }
                });
        }

        /// <summary>
        ///     新建文件
        /// </summary>
        public async void NewFile()
        {
            NewFileDialog newFileDialog = new NewFileDialog()
            {
                //PrimaryButtonText = "确定",
                //SecondaryButtonText = "取消"
                PrimaryButtonVisibility = Visibility.Collapsed,
                SecondaryButtonVisibility = Visibility.Collapsed
            };

            var contentDialog = new ContentDialog()
            {
                Title = "新建文件",
                Content = newFileDialog,
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消"
            };

            contentDialog.PrimaryButtonClick += (sender, e) =>
            {
                string str = AreFileName(newFileDialog);

                if (!string.IsNullOrEmpty(str))
                {
                    newFileDialog.Reminder = str;
                    return;
                }

                VirtualFile newFile = new VirtualFile()
                {
                    Name = newFileDialog.FileName,
                    Size = newFileDialog.Size
                };

                try
                {
                    Folder.NewFile(newFile);
                    //contentDialog.Hide();
                    ListVirtualStorage();
                    newFileDialog.Complete = true;
                }
                catch (Exception)
                {
                    str = "文件存在";
                    newFileDialog.Reminder = str;
                }
            };

            contentDialog.SecondaryButtonClick += (sender, e) =>
            {
                newFileDialog.Complete = true;
            };

            while (!newFileDialog.Complete)
            {
                await contentDialog.ShowAsync();
            }
        }

        public async void NewFolder()
        {
            NewFolderDialog newFolderDialog = new NewFolderDialog();

            ContentDialog contentDialog = new ContentDialog()
            {
                Content = newFolderDialog,
                PrimaryButtonText = "确定",
                SecondaryButtonText = "取消"
            };

            contentDialog.PrimaryButtonClick += (sender, e) =>
            {
                string str = AreFolderName(newFolderDialog.FolderName).Trim();
                if (!string.IsNullOrEmpty(str))
                {
                    newFolderDialog.Reminder = str;
                }
                else
                {
                    try
                    {
                        Folder.NewFolder(newFolderDialog.FolderName);
                        ListVirtualStorage();
                        newFolderDialog.Complete = true;
                    }
                    catch (Exception)
                    {
                        newFolderDialog.Reminder = "文件夹存在";
                    }
                }
            };

            contentDialog.SecondaryButtonClick += (sender, e) =>
            {
                newFolderDialog.Complete = true;
            };

            while (!newFolderDialog.Complete)
            {
                await contentDialog.ShowAsync();
            }
        }

        private Account _accountVirtual;

        private VirtualStorage _fileFolderVirtualStorage;
        private VirtualFolder _folder;

        private Stack<VirtualFolder> BackFolder
        {
            set;
            get;
        } = new Stack<VirtualFolder>();

        private void ToastText(string str)
        {
            var toastText = ToastTemplateType.ToastText01;
            var content = ToastNotificationManager.GetTemplateContent(toastText);
            XmlNodeList xml = content.GetElementsByTagName("text");
            xml[0].AppendChild(content.CreateTextNode(str));
            ToastNotification toast = new ToastNotification(content);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private string AreFileName(NewFileDialog newFileDialog)
        {
            string str = "";
            int size;
            if (string.IsNullOrEmpty(newFileDialog.FileName))
            {
                str = "文件名为空";
            }
            else if (!VirtualFile.AreFileName(newFileDialog.FileName))
            {
                str = "文件名不能存在\\/*:?\"<>|";
            }
            else if (string.IsNullOrEmpty(newFileDialog.Size))
            {
                str = "文件大小为空";
            }
            else if (!int.TryParse(newFileDialog.Size, out size))
            {
                newFileDialog.Size = "";
                str = "输入文件大小不是数字";
            }
            return str;
        }

        private string AreFolderName(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "文件名为空";
            }
            if (!VirtualFile.AreFileName(str))
            {
                return "文件名不能存在\\/*:?\"<>|";
            }
            return "";
        }
    }

    //public class NewContentDialog : ContentDialog
    //{
    //    public NewContentDialog()
    //    {
    //        Closing += (sender, e) =>
    //        {
    //            e.Cancel = !Complete;
    //        };
    //    }

    //    /// <summary>
    //    ///     对话完成，如果没有完成会继续显示
    //    /// </summary>
    //    public bool Complete
    //    {
    //        set;
    //        get;
    //    }
    //}

    public static class Cryptography
    {
        /// <summary>
        /// des加密
        /// </summary>
        /// <param name="key">密码</param>
        /// <param name="plaintext">明文</param>
        /// <returns></returns>
        public static string DesEncryption(string key, string plaintext)
        {
            SymmetricKeyAlgorithmProvider des = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesEcbPkcs7);
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            CryptographicKey symmetricKey = des.CreateSymmetricKey(keyMaterial);

            IBuffer plainBuffer = CryptographicBuffer.ConvertStringToBinary(plaintext, BinaryStringEncoding.Utf8);

            IBuffer cipherBuffer = CryptographicEngine.Encrypt(symmetricKey, plainBuffer, null);
            return CryptographicBuffer.EncodeToHexString(cipherBuffer);
        }
        /// <summary>
        /// des解密
        /// </summary>
        /// <param name="key">密码</param>
        /// <param name="ciphertext">密文</param>
        /// <returns></returns>
        public static string DesDecryption(string key, string ciphertext)
        {
            SymmetricKeyAlgorithmProvider des = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.DesEcbPkcs7);
            IBuffer keyMaterial = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            CryptographicKey symmetricKey = des.CreateSymmetricKey(keyMaterial);

            IBuffer cipherBuffer = CryptographicBuffer.DecodeFromHexString(ciphertext);

            IBuffer plainBuffer = CryptographicEngine.Decrypt(symmetricKey, cipherBuffer, null);
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, plainBuffer);
        }

        public static string Md5ToBase64String(string str)
        {
            IBuffer buffer = Md5(str);
            return EncodeToBase64String(buffer);
        }

        /// <summary>
        /// Md5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5ToHexString(string str)
        {
            IBuffer buffer = Md5(str);
            return EncodeToHexString(buffer);
        }

        private static IBuffer Md5(string str)
        {
            IBuffer buffer =
                CryptographicBuffer.ConvertStringToBinary(str,
                    BinaryStringEncoding.Utf8);
            return Md5(buffer);
        }

        private static IBuffer Md5(IBuffer buffer)
        {
            HashAlgorithmProvider md5 =
                HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            return md5.HashData(buffer);
        }

        private static string EncodeToHexString(IBuffer buffer)
        {
            return CryptographicBuffer.EncodeToHexString(buffer);
        }


        private static string EncodeToBase64String(IBuffer buffer)
        {
            return CryptographicBuffer.EncodeToBase64String(buffer);
        }


    }
}
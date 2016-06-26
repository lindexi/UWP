// lindexi
// 16:13

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
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

        /// <summary>
        /// 进入文件夹
        /// </summary>
        public void ToFolder()
        {

        }

        public void Delete()
        {
            
        }

        /// <summary>
        /// 列出
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
        /// 新建文件
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



            contentDialog.PrimaryButtonClick += async (sender, e) =>
            {
                bool strNull = false;
                string str = "";
                int size = 0;
                if (string.IsNullOrEmpty(newFileDialog.FileName))
                {
                    str = "文件名为空";
                    strNull = true;
                }
                else if (!VirtualFile.AreFileName(newFileDialog.FileName))
                {
                    str = "文件名不能存在\\/*:?\"<>|";
                    strNull = true;
                }
                else if (string.IsNullOrEmpty(newFileDialog.Size))
                {
                    str = "文件大小为空";
                    strNull = true;
                }
                else if (!int.TryParse(newFileDialog.Size, out size))
                {
                    newFileDialog.Size = "";
                    str = "输入文件大小不是数字";
                    strNull = true;
                }
                if (strNull)
                {
                    newFileDialog.Reminder = str;
                    //await contentDialog.ShowAsync();
                    return;
                }

                VirtualFile newFile = new VirtualFile()
                {
                    Name = newFileDialog.FileName,
                    Size = newFileDialog.Size
                };

                try
                {

                    await Folder.NewFile(newFile);
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

        public void NewFolder()
        {

        }

        private Account _accountVirtual;
        private VirtualFolder _folder;
    }

    public class NewContentDialog : ContentDialog
    {
        public NewContentDialog()
        {
            Closing += (sender, e) =>
            {
                e.Cancel = !Complete;
            };
        }

        /// <summary>
        /// 对话完成，如果没有完成会继续显示
        /// </summary>
        public bool Complete
        {
            set;
            get;
        }


    }
}
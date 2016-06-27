using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EncryptionSyncFolder.Model;
using EncryptionSyncFolder.View;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace EncryptionSyncFolder
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //debug
            Account.AccountVirtual = new Account()
            {
                Name = "root",
                Key = "root"
            };
            Account.AccountVirtual.Confirm();
            //end

            OnAccountConfim = () =>
            {
                FileVirtual_OnClick(this, null);
            };

            OnNotAccount = () =>
            {
                Account_OnClick(this,null);
            };
        }

        private void Account_OnClick(object sender, RoutedEventArgs e)
        {
            FileVirtualFrame.Navigate(typeof(AccountPage),OnAccountConfim);
        }



        /// <summary>
        /// 没有登录
        /// </summary>
        private Action OnNotAccount
        {
            set;
            get;
        }

        private Action OnAccountConfim
        {
            set;
            get;
        }

        private void FileVirtual_OnClick(object sender, RoutedEventArgs e)
        {
            FileVirtualFrame.Navigate(typeof(FileVirtualPage),OnNotAccount);
        }
    }  
}

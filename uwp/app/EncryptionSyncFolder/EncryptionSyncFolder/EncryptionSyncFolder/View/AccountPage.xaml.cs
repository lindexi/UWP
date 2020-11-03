// lindexi
// 21:40

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using EncryptionSyncFolder.Model;
using EncryptionSyncFolder.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace EncryptionSyncFolder.View
{
    /// <summary>
    ///     可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AccountPage : Page
    {
        public AccountPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            OnAccountConfirm = e.Parameter as Action;
            View.OnAccountConfirm = OnAccountConfirm;
            base.OnNavigatedTo(e);
        }

        private Action OnAccountConfirm
        {
            set;
            get;
        }

        private AccountModel View
        {
            set;
            get;
        } = new AccountModel();

        private void Confirm_OnClick(object sender, RoutedEventArgs e)
        {
            View.AccountVirtual.Key = Password.Password;
            Password.Password = "";

            View.Confirm();
        }

        private void NewAccount_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(AccountPassword.Password) &&
                string.IsNullOrEmpty(ConfirmAccountPassword.Password)))
            {
                if (AccountPassword.Password !=
                    ConfirmAccountPassword.Password)
                {
                    View.Reminder = "两次输入密码不同";
                    return;
                }
            }
            View.AccountVirtual.Key = AccountPassword.Password;
            AccountPassword.Password = "";
            ConfirmAccountPassword.Password = "";

            View.NewAccount();
        }
    }
}

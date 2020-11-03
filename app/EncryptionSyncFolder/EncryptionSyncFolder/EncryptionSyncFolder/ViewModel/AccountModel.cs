// lindexi
// 15:07

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptionSyncFolder.Model;
using Windows.UI.Xaml;

namespace EncryptionSyncFolder.ViewModel
{
    public class AccountModel : NotifyProperty
    {
        public AccountModel()
        {
            //AccountVirtual = new Account()
            //{
            //    OnConfirmEventHandler = OnConfirm,
            //    OnNewAccountEventHandler = OnNewAccount
            //};
            AccountVirtual = Account.AccountVirtual;
            if (!AccountVirtual.AreAccountConfirm)
            {
                AccountVirtual.OnConfirmEventHandler = OnConfirm;
                AccountVirtual.OnNewAccountEventHandler = OnNewAccount;
            }

            ConfirmVisibility = Visibility.Visible;
            NewAccountVisibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     是否登录
        /// </summary>
        public bool AreAccountConfirm
        {
            set
            {
                _areAccountConfirm = value;
                OnPropertyChanged();
                OnPropertyChanged("AccountVisibility");
            }
            get
            {
                return _areAccountConfirm;
            }
        }

        /// <summary>
        ///     显示登录
        /// </summary>
        public Visibility AccountVisibility
        {
            get
            {
                return AreAccountConfirm
                    ? Visibility.Collapsed
                    : Visibility.Visible;
            }
        }

        public Visibility ConfirmVisibility
        {
            set
            {
                _confirmVisibility = value;
                OnPropertyChanged();
            }
            get
            {
                return _confirmVisibility;
            }
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

        public Visibility NewAccountVisibility
        {
            set
            {
                _newAccountVisibility = value;
                OnPropertyChanged();
            }
            get
            {
                return _newAccountVisibility;
            }
        }

        public Action OnAccountConfirm
        {
            set;
            get;
        }

        public Account AccountVirtual
        {
            set
            {
                _accountVirtual = value;
                AreAccountConfirm = AccountVirtual.AreAccountConfirm;
                OnPropertyChanged();
            }
            get
            {
                return _accountVirtual;
            }
        }

        /// <summary>
        ///     登录按钮
        /// </summary>
        public void Confirm()
        {
            if (ConfirmVisibility == Visibility.Visible)
            {
                //登录
                if (string.IsNullOrEmpty(AccountVirtual.Key))
                {
                    Reminder = "输入密码为空";
                    return;
                }
                AccountVirtual.Confirm();
            }
            else
            {
                ConfirmVisibility = Visibility.Visible;
                NewAccountVisibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     退出
        /// </summary>
        public void AccountOut()
        {
            AreAccountConfirm = false;
            AccountVirtual.Storage();
            AccountVirtual = new Account()
            {
                OnConfirmEventHandler = OnConfirm,
                OnNewAccountEventHandler = OnNewAccount
            };
            Account.AccountVirtual = AccountVirtual;
        }

        /// <summary>
        ///     注册
        /// </summary>
        public void NewAccount()
        {
            if (NewAccountVisibility == Visibility.Visible)
            {
                //注册
                if (string.IsNullOrEmpty(AccountVirtual.Key))
                {
                    Reminder = "输入密码为空";
                    return;
                }
                AccountVirtual.NewAccount();
            }
            else
            {
                NewAccountVisibility = Visibility.Visible;
                ConfirmVisibility = Visibility.Collapsed;
            }
        }

        private Account _accountVirtual;

        private bool _areAccountConfirm;

        private Visibility _confirmVisibility;
        private Visibility _newAccountVisibility;

        private string _reminder;

        private void OnConfirm(object sender, Account.ConfirmEnum e)
        {
            switch (e)
            {
                //成功
                case Account.ConfirmEnum.Success:
                    AreAccountConfirm = true;
                    OnAccountConfirm?.Invoke();
                    break;
                //没有找到账户
                case Account.ConfirmEnum.AccountNotExist:
                    Reminder = "没有找到账户";
                    break;
                //错误
                case Account.ConfirmEnum.KeyError:
                    Reminder = "密码错误";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }

        private void OnNewAccount(object sender, Account.NewAccountEnum e)
        {
            switch (e)
            {
                //成功
                case Account.NewAccountEnum.Success:
                    AreAccountConfirm = true;
                    OnAccountConfirm?.Invoke();
                    break;
                //存在账户
                case Account.NewAccountEnum.AccountExist:
                    Reminder = "账户存在";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e), e, null);
            }
        }
    }
}

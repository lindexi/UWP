// lindexi
// 16:13

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptionSyncFolder.Model;

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
        /// <summary>
        /// 进入文件夹
        /// </summary>
        public void ToFolder()
        {
            
        }
        /// <summary>
        /// 列出
        /// </summary>
        public void ListVirtualStorage()
        {
            
        }

        private Account _accountVirtual;
        private VirtualFolder _folder;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionSyncFolder.ViewModel
{
    public class EncryptionFolderModel : NotifyProperty
    {
        public EncryptionFolderModel()
        {
            Account = Model.Account.AccountVirtual;
        }
        public Model.Account Account
        {
            set;
            get;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitStamp.ViewModel
{
    public class AccoutGoverment
    {
        public AccoutGoverment()
        {
            Account=new Account();
        }

        private void Read()
        {
            
        }

        public Account Account
        {
            set;
            get;
        }

        public static AccoutGoverment AccountModel
        {
            set
            {
                _accountModel = value;
            }
            get
            {
                return _accountModel ?? (_accountModel = new AccoutGoverment());
            }
        }

        private static AccoutGoverment _accountModel;
    }
}

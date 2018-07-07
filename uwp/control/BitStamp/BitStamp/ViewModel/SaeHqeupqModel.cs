using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lindexi.MVVM.Framework.ViewModel;
using lindexi.uwp.Framework.ViewModel;

namespace BitStamp.ViewModel
{
    public class SaeHqeupqModel : ViewModelMessage
    {
        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            Account = (Account) obj;
        }

        public Account Account { get; set; }
    }
}

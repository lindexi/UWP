using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteresRoller.ViewModel
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {

        }

        public ObservableCollection<string> Str
        {
            set;
            get;
        } = new ObservableCollection<string>()
        {
            "林德熙",
            "http://blog.csdn.net/lindexi_gd",
            "http://github.com/lindexi"
        };
    }
}

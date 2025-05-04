using System;
using System.Collections.Generic;
using System.Text;

namespace lindexi.MVVM.Framework.ViewModel
{
   public class EventInfomation<TEventArgs>
    {
        public object Sender { get; set; }

        public TEventArgs EventArgs { get; set; }


        public object CommandParameter { get; set; }
    }
}

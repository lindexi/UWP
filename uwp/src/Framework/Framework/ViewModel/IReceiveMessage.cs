using System.Collections.Generic;

namespace lindexi.uwp.Framework.ViewModel
{
    public interface IReceiveMessage
    {
        void ReceiveMessage(object sender, IMessage message);
    }
}
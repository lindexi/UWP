using System;

namespace lindexi.uwp.Framework.ViewModel
{
    public interface ISendMessage
    {
        EventHandler<IMessage> SendMessageHandler { set; get; }
    }
}
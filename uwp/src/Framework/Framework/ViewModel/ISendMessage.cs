using System;

namespace lindexi.uwp.Framework.ViewModel
{
    public interface ISendMessage
    {
        EventHandler<Message> SendMessageHandler { set; get; }
    }
}
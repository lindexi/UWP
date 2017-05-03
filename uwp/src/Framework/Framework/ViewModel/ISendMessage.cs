using System;

namespace lindexi.uwp.Framework.ViewModel
{
    public interface ISendMessage: IViewModel
    {
        EventHandler<IMessage> SendMessageHandler { set; get; }
    }
}
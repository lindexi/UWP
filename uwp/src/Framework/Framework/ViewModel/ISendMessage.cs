using System;

namespace Framework.ViewModel
{
    public interface ISendMessage
    {
        EventHandler<Message> SendMessageHandler
        {
            set;
            get;
        }
    }
}
using System;

namespace Framework.ViewModel
{
    /// <summary>
    /// 接收发送信息
    /// </summary>
    interface IAdapterMessage
    {
        /// <summary>
        /// 发送信息
        /// </summary>
        EventHandler<Message> Send { set; get; }
        /// <summary>
        /// 接收信息
        /// </summary> 
        /// <param name="source"></param>
        /// <param name="message"></param>
        void Receive(object source, Message message);
    }
}
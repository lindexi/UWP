using System;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 处理消息
    /// </summary>
    public class Composite
    {
        /// <summary>
        /// 处理什么消息
        /// </summary>
        public Type Message
        {
            get; set;
        }

        public string Key
        {
            get; set;
        }

        public virtual void Run(ViewModelBase source, IMessage message)
        {

        }
    }
}
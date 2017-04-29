namespace lindexi.uwp.Framework.ViewModel
{
    public interface IMessage
    {
        ViewModelBase Source
        {
            set; get;
        }
        string Goal
        {
            set; get;
        }
    }

    public class Message : IMessage
    {
        //public Message()
        //{

        //}

        public Message(ViewModelBase source)
        {
            Source = source;
        }

        /// <summary>
        ///     发送者
        /// </summary>
        public ViewModelBase Source { set; get; }

        /// <summary>
        ///     目标
        /// </summary>
        public string Goal { set; get; }

        public object Content { set; get; }

        /// <summary>
        ///     发送什么信息
        /// </summary>
        public string Key { set; get; }
    }
}
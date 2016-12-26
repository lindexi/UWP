namespace Framework.ViewModel
{
    public class Message
    {
        public Message()
        {

        }
        /// <summary>
        /// 发送者
        /// </summary>
        public ViewModelBase Source
        {
            set;
            get;
        }
        /// <summary>
        /// 目标
        /// </summary>
        public string Goal
        {
            set;
            get;
        }

        public object Content
        {
            set;
            get;
        }
        /// <summary>
        /// 发送什么信息
        /// </summary>
        public string Key
        {
            set;
            get;
        }
    }
}
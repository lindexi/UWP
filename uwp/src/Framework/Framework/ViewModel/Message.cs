namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 消息
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 发送消息的 <see cref="ViewModelBase"/> 用于告诉是哪个发送
        /// </summary>
        IViewModel Source { set; get; }

        /// <summary>
        ///     判断使用哪个ViewModel，如果为空，返回上一层
        /// </summary>
        IPredicateViewModel Goal { set; get; }

        /// <summary>
        ///     判断ViewModel是否符合
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        bool Predicate(IViewModel viewModel);
    }

    /// <inheritdoc />
    public class Message : IMessage
    {
        /// <inheritdoc />
        public Message(ViewModelBase source)
        {
            Source = source;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public object Content { set; get; }

        /// <summary>
        ///     发送什么信息
        /// </summary>
        public string Key { set; get; }

        /// <summary>
        ///     发送者
        /// </summary>
        public IViewModel Source { set; get; }

        /// <summary>
        ///     目标
        /// </summary>
        public IPredicateViewModel Goal { set; get; }

        /// <inheritdoc />
        public bool Predicate(IViewModel viewModel)
        {
            if (Goal == null)
            {
                return true;
            }
            return Goal.Predicate(viewModel);
        }
    }
}
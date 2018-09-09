using lindexi.MVVM.Framework.ViewModel;

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 表示处理消息
    /// </summary>
    public interface IComposite
    {
        /// <summary>
        /// 运行处理方法
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        void Run(IViewModel source, IMessage message);
    }
}
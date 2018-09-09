namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 接收信息
    /// </summary>
    public interface IReceiveMessage : IViewModel
    {
        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        void ReceiveMessage(object sender, IMessage message);
    }
}
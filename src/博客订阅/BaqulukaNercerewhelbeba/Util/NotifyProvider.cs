namespace BaqulukaNercerewhelbeba.Util
{
    class NotifyProvider : INotifyProvider
    {
        /// <inheritdoc />
        public void SendText(string url, string text)
        {
            if (url.Contains("qyapi.weixin"))
            {
                var qyweixin = new Qyweixin();
                qyweixin.SendText(url, text);
            }
            else
            {
                var matterMost = new MatterMost();
                matterMost.SendText(url, text);
            }
        }
    }
}
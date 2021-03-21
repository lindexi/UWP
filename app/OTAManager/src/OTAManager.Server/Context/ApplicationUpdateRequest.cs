namespace OTAManager.Server.Controllers
{
    public class ApplicationUpdateRequest
    {
        public string ApplicationId { get; set; }

        /// <summary>
        /// 客户端的版本号
        /// </summary>
        /// 如果客户端的版本号太小了，那就加入到优先级里面去，让这个客户端更新
        public string Version { get; set; }

        /// <summary>
        /// 客户端的 mac 地址，可以用来识别这个客户端的标识，例如测试设备等
        /// </summary>
        public string MacAddress { set; get; }
    }
}

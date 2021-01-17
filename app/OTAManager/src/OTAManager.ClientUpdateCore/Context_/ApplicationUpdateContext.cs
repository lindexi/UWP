using System;

namespace OTAManager.ClientUpdateCore
{
    /// <summary>
    /// 更新服务器的软件版本信息
    /// </summary>
    public class ApplicationUpdateContext
    {
        public string ApplicationId { get; set; } = null!;

        public Version ApplicationVersion { get; set; } = null!;

        public ClientUpdateManifest? ClientUpdateManifest { get; set; }
    }
}

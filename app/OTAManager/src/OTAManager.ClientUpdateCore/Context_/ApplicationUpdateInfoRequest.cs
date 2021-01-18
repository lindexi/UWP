using System;

namespace OTAManager.ClientUpdateCore
{
    /// <summary>
    /// 获取当前更新版本
    /// </summary>
    public class ApplicationUpdateInfoRequest
    {
        public ApplicationUpdateInfoRequest(string applicationId, Version currentApplicationVersion)
        {
            ApplicationId = applicationId;
            CurrentApplicationVersion = currentApplicationVersion;
        }

        public string ApplicationId { get; }

        public Version CurrentApplicationVersion { get; }
    }
}

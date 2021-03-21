using System;
using Microsoft.EntityFrameworkCore;
using OTAManager.Server.Controllers;
using OTAManager.UpdateInfoShare.Data;

namespace OTAManager.Server.Model
{
    /// <summary>
    /// 最新的应用信息
    /// </summary>
    [Index(nameof(ApplicationId))]
    public class ApplicationUpdateInfoModel : ApplicationUpdateInfo
    {
        public int Id { set; get; }
    }
}

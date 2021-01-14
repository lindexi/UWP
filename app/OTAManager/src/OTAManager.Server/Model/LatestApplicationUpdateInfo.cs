using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OTAManager.Server.Controllers
{
    /// <summary>
    /// 最新的应用信息
    /// </summary>
    [Index(nameof(ApplicationId))]
    public class ApplicationUpdateInfoModel: ApplicationUpdateInfo
    {
        public int Id { set; get; }
    }
}

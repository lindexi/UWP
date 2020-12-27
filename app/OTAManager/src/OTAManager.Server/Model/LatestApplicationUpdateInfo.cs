using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OTAManager.Server.Controllers
{
    /// <summary>
    /// 最新的应用信息
    /// </summary>
    [Index(nameof(ApplicationId))]
    public class ApplicationUpdateInfo
    {
        public int Id { set; get; }

        public string ApplicationId { get; set; }

        public string Version { get; set; }

        public string UpdateContext { get; set; }
    }
}

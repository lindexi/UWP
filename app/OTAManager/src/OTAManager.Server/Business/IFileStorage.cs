using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OTAManager.Server.Controllers
{
    /// <summary>
    /// 文件存储服务
    /// </summary>
    public interface IFileStorage
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<string> UploadFile(UploadFileRequest request);

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IActionResult DownloadFile(string key);
    }
}

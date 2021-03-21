using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OTAManager.Server.Context;

namespace OTAManager.Server.Core
{
    /// <summary>
    /// 使用本地文件系统的文件存储服务
    /// </summary>
    /// 存放在本地
    public class FileSystemFileStorage : IFileStorage
    {
        /// <summary>
        /// 存放文件的文件夹
        /// </summary>
        public DirectoryInfo? FileStorageFolder { set; get; }

        public async Task<string> UploadFile(UploadFileRequest request)
        {
            var fileStorageFolder = FileStorageFolder ?? Directory.CreateDirectory("FileStorageFolder");

            var fileName = FileHelper.GetSafeFileName(request.Name);
            // 不能太长哦
            if (fileName.Length > 100)
            {
                fileName = fileName.Substring(0, 100);
            }

            fileName += $"_{Path.GetRandomFileName()}";

            var file = Path.Combine(fileStorageFolder.FullName, fileName);
            using (var fileStream = File.OpenWrite(file))
            {
                await request.File.CopyToAsync(fileStream);
            }

            var key = fileName;
            return key;
        }

        public IActionResult DownloadFile(string key)
        {
            var fileStorageFolder = FileStorageFolder ?? Directory.CreateDirectory("FileStorageFolder");

            var fileName = key;
            var file = Path.Combine(fileStorageFolder.FullName, fileName);
            if (File.Exists(file))
            {
                return new PhysicalFileResult(file, "application/octet-stream");
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}

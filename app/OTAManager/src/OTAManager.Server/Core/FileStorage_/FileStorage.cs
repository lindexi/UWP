using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OTAManager.Server.Context;
using OTAManager.Server.Data;
using OTAManager.Server.Model;

namespace OTAManager.Server.Core
{
    public class FileStorage : IFileStorage
    {
        public FileStorage(IConfiguration configuration, FileStorageContext fileStorageContext)
        {
            _fileStorageContext = fileStorageContext;
            // 配置 FileStorageFolder 文件
        }

        private readonly FileStorageContext _fileStorageContext;

        /// <summary>
        /// 存放文件的文件夹
        /// </summary>
        public DirectoryInfo? FileStorageFolder { set; get; }

        public DirectoryInfo? TempFolder { set; get; }

        private string BuildFilePath(string fileName, FileStorageModel fileStorageModel)
        {
            fileName = FileHelper.GetSafeFileName(fileName);
            // 不能太长哦
            if (fileName.Length > 100)
            {
                fileName = fileName.Substring(0, 100);
            }

            //fileName += $"_{Path.GetRandomFileName()}";

            // 根据 Md5 生成文件夹路径
            var md5 = fileStorageModel.Md5;
            var firstFolder = md5.Substring(0, 2);
            var secondFolder = md5.Substring(2, 2);
            var lastFolder = md5.Substring(4,8);
            var filePath = Path.Combine(firstFolder, secondFolder, lastFolder, fileName);

            fileStorageModel.FilePath = filePath;

            var fileStorageFolder = FileStorageFolder ?? Directory.CreateDirectory("FileStorageFolder");
            var file = Path.Combine(fileStorageFolder.FullName, filePath);
            Directory.CreateDirectory(Path.GetDirectoryName(file)!);

            return file;
        }

        public async Task<string> UploadFile(UploadFileRequest request)
        {
            // 逻辑是先下载到 Temp 然后判断文件是否合法等
            var tempFolder = TempFolder ?? new DirectoryInfo(Path.GetTempPath());
            var tempFile = Path.Combine(tempFolder.FullName, Path.GetRandomFileName());
            using (var fileStream = File.OpenWrite(tempFile))
            {
                await request.File.CopyToAsync(fileStream);
            }

            var fileStorageModel = new FileStorageModel();
            GetFileInfo(tempFile, fileStorageModel);
            var existModel =
                _fileStorageContext.FileStorageData.FirstOrDefault(temp => temp.Md5 == fileStorageModel.Md5);
            if (existModel != null)
            {
                // 已存在的文件，就不需要重复加入数据库
                File.Delete(tempFile);

                return existModel.FileKey;
            }

            var file = BuildFilePath(request.Name, fileStorageModel);
            // 存放文件
            FileHelper.MoveFile(tempFile, file);

            await _fileStorageContext.FileStorageData.AddAsync(fileStorageModel);
            await _fileStorageContext.SaveChangesAsync();

            return fileStorageModel.FileKey;
        }

        public IActionResult DownloadFile(string key)
        {
            var fileStorageFolder = FileStorageFolder ?? Directory.CreateDirectory("FileStorageFolder");

            var fileStorageModel = _fileStorageContext.FileStorageData.FirstOrDefault(temp => temp.FileKey == key);
            if (fileStorageModel != null)
            {
                var fileName = fileStorageModel.FilePath;
                var file = Path.Combine(fileStorageFolder.FullName, fileName);
                if (File.Exists(file))
                {
                    return new PhysicalFileResult(file, "application/octet-stream");
                }
            }

            return new NotFoundResult();
        }

        private void GetFileInfo(string filePath, FileStorageModel fileStorageModel)
        {
            var file = new FileInfo(filePath);
            var md5 = Md5Provider.GetMd5Hash(file);

            fileStorageModel.Md5 = md5;
            fileStorageModel.FileLength = file.Length;
            fileStorageModel.FileKey = md5;
        }
    }
}

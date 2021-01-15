using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using dotnetCampus.FileDownloader;

namespace OTAManager.ClientUpdateCore
{
    public class ClientUpdateFileDownloader : IClientUpdateFileDownloader
    {
        public async Task<IClientUpdateFileDownloadResult> Download(ClientUpdateFileDownloadContext clientUpdateFileDownloadContext)
        {
            var downloadFolder = clientUpdateFileDownloadContext.TempFolder;
            var clientApplicationFileInfoList = clientUpdateFileDownloadContext.ClientApplicationFileInfoList;

            // 创建下载任务
            foreach (var clientApplicationFileInfo in clientApplicationFileInfoList)
            {
                var downloadFile = new FileInfo(Path.Combine(downloadFolder.FullName, clientApplicationFileInfo.FilePath));

                // 创建下载的文件的文件夹
                downloadFile.Directory!.Create();

                var segmentFileDownloader = new SegmentFileDownloader(clientApplicationFileInfo.DownloadUrl, downloadFile);
                await segmentFileDownloader.DownloadFileAsync().ConfigureAwait(false);

                // 文件是否是对的？判断一下
                downloadFile.Refresh();
                if (!downloadFile.Exists)
                {
                    return new ClientUpdateFileDownloadResult(false);
                }

                var md5 = Md5Helper.GetMd5Hash(downloadFile);
                if (!string.Equals(md5, clientApplicationFileInfo.Md5, StringComparison.OrdinalIgnoreCase))
                {
                    return new ClientUpdateFileDownloadResult(false);
                }
            }

            return new ClientUpdateFileDownloadResult(true);
        }
    }

    class Md5Helper
    {
        /// <summary>
        ///     计算 <paramref name="file" /> 的MD5值。
        /// </summary>
        /// <returns>Md5值</returns>
        public static string GetMd5Hash(FileInfo file)
        {
            using var fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            return GetMd5HashFromStream(fileStream);
        }

        /// <summary>
        ///     计算
        ///     <param name="stream">指定流</param>
        ///     的MD5值。
        /// </summary>
        /// <returns>Md5值</returns>
        private static string GetMd5HashFromStream(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var hashBytes = md5.ComputeHash(stream);
                return string.Join(null, hashBytes.Select(temp => temp.ToString("x2")));
            }
        }
    }

    ///// <summary>
    ///// 文件下载任务
    ///// </summary>
    //class FileDownloadTask
    //{
    //    public FileDownloadTask(FileInfo downloadFile, Uri downloadUrl, string md5)
    //    {
    //        DownloadFile = downloadFile;
    //        DownloadUrl = downloadUrl;
    //        Md5 = md5;
    //    }

    //    public FileInfo DownloadFile { get; }
    //    public Uri DownloadUrl { get; }
    //    public string Md5 { get; }
    //}
}

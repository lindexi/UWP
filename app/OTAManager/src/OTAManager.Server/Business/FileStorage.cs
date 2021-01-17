#nullable enable
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OTAManager.Server.Controllers
{
    /// <summary>
    /// 文件存储服务
    /// </summary>
    /// 存放在本地
    public class FileStorage : IFileStorage
    {
        /// <summary>
        /// 存放文件的文件夹
        /// </summary>
        public DirectoryInfo? FileStorageFolder { set; get; }

        public async Task<string> UploadFile(UploadFileRequest request)
        {
            var fileStorageFolder = FileStorageFolder ?? Directory.CreateDirectory("FileStorageFolder");

            var fileName = GetSafeFileName(request.Name);
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

        static string GetSafeFileName(string arbitraryString)
        {
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            var replaceIndex = arbitraryString.IndexOfAny(invalidChars, 0);
            if (replaceIndex == -1) return arbitraryString;

            var r = new StringBuilder();
            var i = 0;

            do
            {
                r.Append(arbitraryString, i, replaceIndex - i);

                switch (arbitraryString[replaceIndex])
                {
                    case '"':
                        r.Append("''");
                        break;
                    case '<':
                        r.Append('\u02c2'); // '˂' (modifier letter left arrowhead)
                        break;
                    case '>':
                        r.Append('\u02c3'); // '˃' (modifier letter right arrowhead)
                        break;
                    case '|':
                        r.Append('\u2223'); // '∣' (divides)
                        break;
                    case ':':
                        r.Append('-');
                        break;
                    case '*':
                        r.Append('\u2217'); // '∗' (asterisk operator)
                        break;
                    case '\\':
                    case '/':
                        r.Append('\u2044'); // '⁄' (fraction slash)
                        break;
                    case '\0':
                    case '\f':
                    case '?':
                        break;
                    case '\t':
                    case '\n':
                    case '\r':
                    case '\v':
                        r.Append(' ');
                        break;
                    default:
                        r.Append('_');
                        break;
                }

                i = replaceIndex + 1;
                replaceIndex = arbitraryString.IndexOfAny(invalidChars, i);
            } while (replaceIndex != -1);

            r.Append(arbitraryString, i, arbitraryString.Length - i);

            return r.ToString();
        }
    }
}

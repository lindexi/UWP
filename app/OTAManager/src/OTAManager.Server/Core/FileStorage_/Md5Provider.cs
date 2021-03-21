using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace OTAManager.Server.Core
{
    static class Md5Provider
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
}

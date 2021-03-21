using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Lindexi.Src.DirectoryToZipStream
{
    /// <summary>
    /// 将一个文件夹的内容读取为 Stream 的压缩包
    /// </summary>
    public static class DirectoryToZipStreamHelper
    {
        /// <summary>
        /// 将一个文件夹的内容读取为 Stream 的压缩包
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="stream"></param>
        public static void ReadDirectoryToZipStream(DirectoryInfo directory, Stream stream)
        {
            var fileList = GetFileList(directory);

            using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create);
            foreach (var file in fileList)
            {
                var relativePath = GetRelativePath(directory, file);

                var zipArchiveEntry = zipArchive.CreateEntry(relativePath, CompressionLevel.NoCompression);

                using (var entryStream = zipArchiveEntry.Open())
                {
                    using var toZipStream = file.OpenRead();
                    toZipStream.CopyTo(entryStream);
                }

                stream.Flush();
            }
        }

        /// <summary>
        /// 将一个文件夹的内容读取为 Stream 的压缩包
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="stream"></param>
        public static async Task ReadDirectoryToZipStreamAsync(DirectoryInfo directory, Stream stream)
        {
            var fileList = GetFileList(directory);

            using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create);
            foreach (var file in fileList)
            {
                var relativePath = GetRelativePath(directory, file);

                var zipArchiveEntry = zipArchive.CreateEntry(relativePath, CompressionLevel.NoCompression);

                using (var entryStream = zipArchiveEntry.Open())
                {
                    using var toZipStream = file.OpenRead();
                    await toZipStream.CopyToAsync(entryStream);
                }

                await stream.FlushAsync();
            }
        }

        private static FileInfo[] GetFileList(DirectoryInfo directory)
        {
            return directory.GetFiles("*.*", SearchOption.AllDirectories);
        }

        private static string GetRelativePath(DirectoryInfo directory, FileInfo file)
        {
            var relativePath = file.FullName.Replace(directory.FullName, "");
            if (relativePath.StartsWith("\\") || relativePath.StartsWith("//"))
            {
                relativePath = relativePath.Substring(1);
            }

            return relativePath;
        }
    }
}

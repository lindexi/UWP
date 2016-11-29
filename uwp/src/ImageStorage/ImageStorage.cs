using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace lindexi.uwp.src.ImageStorage
{
    public static class ImageStorage
    {
        /// <summary>
        /// 获取图片
        /// 如果本地存在，就获取本地
        /// 如果本地不存在，获取网络
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<BitmapImage> GetImage(Uri uri)
        {
            return await GetLoacalFolderImage(uri) ??
                   await GetHttpImage(uri);
        }

        /// <summary>
        /// 从本地获取图片
        /// </summary>
        /// <param name="uri"></param>
        private static async Task<BitmapImage> GetLoacalFolderImage(Uri uri)
        {
            StorageFolder folder = await GetImageFolder();

            string name = Md5(uri.AbsolutePath);

            try
            {
                StorageFile file = await folder.GetFileAsync(name);
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage img = new BitmapImage();
                    await img.SetSourceAsync(stream);

                    return img;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static async Task<BitmapImage> GetHttpImage(Uri uri)
        {
            try
            {
                Windows.Web.Http.HttpClient http = new Windows.Web.Http.HttpClient();
                IBuffer buffer = await http.GetBufferAsync(uri);

                BitmapImage img = new BitmapImage();
                using (IRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await stream.WriteAsync(buffer);
                    stream.Seek(0);
                    await img.SetSourceAsync(stream);
                    await StorageImageFolder(stream, uri);
                    return img;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static async Task StorageImageFolder(IRandomAccessStream stream, Uri uri)
        {
            StorageFolder folder = await GetImageFolder();

            string image = Md5(uri.AbsolutePath);

            try
            {
                StorageFile file = await folder.CreateFileAsync(image);
                await FileIO.WriteBytesAsync(file, await ConvertIRandomAccessStreamByte(stream));
            }
            catch (Exception)
            {

            }

        }

        private static async Task<byte[]> ConvertIRandomAccessStreamByte(IRandomAccessStream stream)
        {
            DataReader read = new DataReader(stream.GetInputStreamAt(0));
            await read.LoadAsync((uint)stream.Size);
            byte[] temp = new byte[stream.Size];
            read.ReadBytes(temp);
            return temp;
        }

        private static async Task<StorageFolder> GetImageFolder()
        {
            //文件夹
            string name = "image";

            StorageFolder folder = null;

            //从本地获取文件夹
            try
            {
                folder = await ApplicationData.Current.LocalCacheFolder.GetFolderAsync(name);
            }
            catch (FileNotFoundException)
            {
                //没找到
                folder = await ApplicationData.Current.LocalCacheFolder.
                    CreateFolderAsync(name);
            }

            return folder;
        }

        private static string Md5(string str)
        {
            HashAlgorithmProvider hashAlgorithm =
                HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            CryptographicHash cryptographic = hashAlgorithm.CreateHash();

            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);

            cryptographic.Append(buffer);

            return CryptographicBuffer.EncodeToHexString(cryptographic.GetValueAndReset());
        }

    }
}
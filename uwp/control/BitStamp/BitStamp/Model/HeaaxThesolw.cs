using System;
using System.Threading.Tasks;
using Windows.Storage;
using BitStamp.ViewModel;
using lindexi.MVVM.Framework.Annotations;
using lindexi.uwp.ImageShack.Model;

namespace BitStamp.Model
{
    public class HeaaxThesolw
    {
        /// <inheritdoc />
        public HeaaxThesolw(Account account, [NotNull] StorageFile file)
        {
            Account = account;
            File = file ?? throw new ArgumentNullException(nameof(file));
        }

        private Account Account { get; set; }

        private StorageFile File { get; set; }

        private UploadImageTask NewUploadImageTask(ImageShackEnum imageShack, StorageFile file)
        {
            switch (imageShack)
            {
                case ImageShackEnum.Jiuyou:
                    return new JyUploadImage(file);
                case ImageShackEnum.Smms:
                    return new SmmsUploadImage(file);
                case ImageShackEnum.Qin:
                    return new QnUploadImage(file)
                    {
                        Accound = Account.CloundAccound
                    };
                case ImageShackEnum.Cimage:
                    return new BitStamp.Model.Cimage.Cimage(file);
                default:
                    throw new ArgumentOutOfRangeException(nameof(imageShack), imageShack, null);
            }

            //return new JyUploadImage(file);
        }

        public string Url { get; set; }

        public async Task Jcloud(Action<bool> onUpload)
        {
            ImageShackEnum imageShack = Account.ImageShack;
            //if (File.FileType == ".gif" && imageShack == ImageShackEnum.Jiuyou)
            //{
            //    imageShack = ImageShackEnum.Qin;
            //}

            var size = (await File.GetBasicPropertiesAsync()).Size;

            //1M
            //1024k
            //‪125000‬
            if (size > 500000)
            {
                imageShack = ImageShackEnum.Smms;
            }
            //4326  24,447 

            imageShack = CheckShack(imageShack);

#if DEBUG
            //imageShack = ImageShackEnum.Cimage;
#endif

            UploadImageTask uploadImageTask = NewUploadImageTask(
                imageShack, File);
            uploadImageTask.OnUploaded += (s, e) =>
            {
                if (!(s is UploadImageTask uploadImage))
                {
                    onUpload?.Invoke(false);
                    return;
                }

                Url = uploadImage.Url;

                onUpload?.Invoke(e);
            };
            uploadImageTask.UploadImage();
        }

        private ImageShackEnum CheckShack(ImageShackEnum imageShack)
        {
            //检查当前是否可以使用
            if (imageShack == ImageShackEnum.Qin)
            {
                //如果图床账号错误
                if (string.IsNullOrEmpty(Account.CloundAccound.AccessKey) ||
                    string.IsNullOrEmpty(Account.CloundAccound.Bucket) ||
                    string.IsNullOrEmpty(Account.CloundAccound.SecretKey) ||
                    string.IsNullOrEmpty(Account.CloundAccound.Url))
                {
                    return ImageShackEnum.Smms;
                }
            }

            return imageShack;
        }
    }
}
using System.Threading.Tasks;
using BitStamp.Model.Cimage;
using BitStamp.ViewModel;
using JyCloudTool.JsonModel;
using Windows.Storage;

namespace BitStamp.Model
{
    public class JyUploadImage : UploadImageTask
    {
        public JyUploadImage(UploadImageTask uploadImageTask)
            : base(uploadImageTask)
        {
        }

        public JyUploadImage(StorageFile file)
            : base(file)
        {
        }

        public override async void UploadImage()
        {
            await UploadImage(this);
        }

        private async Task UploadImage(UploadImageTask uploadImageTask)
        {
            //Appid 为静态，有_appid 应用的appid
            //_secretId 九幽的
            var account = AccoutGoverment.AccountModel;
            if (!string.IsNullOrEmpty(account.Account.JiuYouId))
            {
                AppId._appId = account.Account.JiuYouId;
            }

            if (!string.IsNullOrEmpty(account.Account.JiuYouSecretId))
            {
                AppId._secretId = account.Account.JiuYouSecretId;
            }

            ResponseInfo responseInfo;
            if (uploadImageTask.Scale > 0)
            {
                responseInfo = await JyCloudTool.JyCloudTool.sendFileScale(
                    AppId._appId, AppId._secretId, uploadImageTask.File,
                    uploadImageTask.Scale);
            }
            else if ((uploadImageTask.Width > 0) && (uploadImageTask.Height > 0))
            {
                responseInfo = await JyCloudTool.JyCloudTool.sendFileCustom(
                    AppId._appId, AppId._secretId, uploadImageTask.File,
                    (uint) uploadImageTask.Width, (uint) uploadImageTask.Height);
            }
            else
            {
                responseInfo = await JyCloudTool.JyCloudTool.sendFileOriginal(
                    AppId._appId, AppId._secretId, uploadImageTask.File);
            }
            if (responseInfo.respose_Status == 0)
            {
                uploadImageTask.Url = responseInfo.ImgUrl;
                uploadImageTask.OnUploaded?.Invoke(uploadImageTask, true);
            }
            else
            {
                uploadImageTask.OnUploaded?.Invoke(uploadImageTask, false);
            }
        }
    }

    //public static class AppId
    //{
    //    public static string _appId
    //    {
    //        set;
    //        get;
    //    } = "";
    //    public static string _secretId
    //    {
    //        set;
    //        get;
    //    } = "";
    //}
}

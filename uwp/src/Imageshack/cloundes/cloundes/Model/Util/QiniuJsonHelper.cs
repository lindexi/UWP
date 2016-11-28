// lindexi
// 16:34

using Newtonsoft.Json;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.Util
{
    public static class QiniuJsonHelper
    {
        public static string JsonEncode(object obj)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(obj, setting);
        }

        public static T ToObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
// lindexi
// 16:34

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.IO.Resumable
{
    /// <summary>
    ///     断点续传上传参数设置
    /// </summary>
    internal class Settings
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="chunkSize">chunk大小,默认为4MB</param>
        /// <param name="tryTimes">失败重试次数,默认为3</param>
        public Settings(int chunkSize = 1 << 22, int tryTimes = 3)
        {
            //chunkSize 已经删除，兼容保留

            this.TryTimes = tryTimes;
        }

        /// <summary>
        ///     chunk大小,默认为4MB;
        ///     兼容保留
        /// </summary>
        public int ChunkSize
        {
            set;
            get;
        }

        /// <summary>
        ///     失败重试次数,默认为3
        /// </summary>
        public int TryTimes
        {
            set;
            get;
        }
    }
}

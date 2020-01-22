using System;

namespace BaqulukaNercerewhelbeba.Model
{
    /// <summary>
    /// 发布的博客
    /// </summary>
    public class PublishedBlog
    {
        public int Id { set; get; }

        public string Blog { set; get; }

        public string MatterMost { set; get; }

        /// <summary>
        /// 发送到 MatterMost 的时间
        /// </summary>
        public DateTime Time { set; get; }
    }
}
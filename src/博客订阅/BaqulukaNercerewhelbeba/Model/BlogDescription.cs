using System;

namespace BaqulukaNercerewhelbeba.Model
{
    public class BlogDescription
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public DateTime Time { set; get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Title} {Time}\n{Url}";
        }
    }
}
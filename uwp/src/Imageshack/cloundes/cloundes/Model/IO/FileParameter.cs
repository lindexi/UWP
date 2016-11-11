// lindexi
// 16:34

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.IO
{
    internal class PutParameter
    {
        public PutParameter(string mimeType)
        {
            this.mimeType = mimeType;
        }

        public string MimeType
        {
            get
            {
                return mimeType;
            }
            set
            {
                mimeType = value;
            }
        }

        public virtual long CopyTo(Stream body)
        {
            return 0;
        }

        protected string mimeType;
    }

    internal class StreamParameter : PutParameter
    {
        public StreamParameter(StreamReader reader, string mimeType)
            : base(mimeType)
        {
            this.reader = reader;
        }

        public StreamReader Reader
        {
            get
            {
                return reader;
            }
            set
            {
                reader = value;
            }
        }

        public override long CopyTo(Stream body)
        {
            return 0;
        }

        private StreamReader reader;
    }

    internal class FileParameter : PutParameter
    {
        public FileParameter(string fname, string mimeType)
            : base(mimeType)
        {
            this.fileName = fname;
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        public override long CopyTo(Stream body)
        {
            using (FileStream fs = File.OpenRead(this.fileName))
            {
                fs.CopyTo(body);
                return fs.Length;
            }
        }

        private string fileName;
    }
}
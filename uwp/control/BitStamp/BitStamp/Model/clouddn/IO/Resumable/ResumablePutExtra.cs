// lindexi
// 15:46

using System;
using System.Collections.Specialized;
using System.Net;

namespace Qiniu.IO.Resumable
{
    /// <summary>
    ///     Block上传成功事件参数
    /// </summary>
    public class PutNotifyEvent : EventArgs
    {
        public PutNotifyEvent(int blkIdx, int blkSize, BlkputRet ret)
        {
            BlkIdx = blkIdx;
            BlkSize = blkSize;
            Ret = ret;
        }

        public int BlkIdx
        {
            get;
        }

        public int BlkSize
        {
            get;
        }

        public BlkputRet Ret
        {
            get;
        }
    }

    /// <summary>
    ///     上传错误事件参数
    /// </summary>
    public class PutNotifyErrorEvent : EventArgs
    {
        public PutNotifyErrorEvent(int blkIdx, int blkSize, string error)
        {
            BlkIdx = blkIdx;
            BlkSize = blkSize;
            Error = error;
        }

        public int BlkIdx
        {
            get;
        }

        public int BlkSize
        {
            get;
        }

        public string Error
        {
            get;
        }
    }

    /// <summary>
    /// </summary>
    public class ResumablePutExtra
    {
        public event EventHandler<PutNotifyEvent> OnNotify;

        public event EventHandler<PutNotifyErrorEvent> OnNotifyErr;

        public void Notify(PutNotifyEvent arg)
        {
            OnNotify?.Invoke(this, arg);
        }

        public void NotifyErr(PutNotifyErrorEvent arg)
        {
            OnNotifyErr?.Invoke(this, arg);
        }

        //key format as: "x:var"
        public /*NameValueCollection*/ WebHeaderCollection CallbackParams
        {
            set;
            get;
        }
        public int ChunkSize
        {
            set;
            get;
        }
        public string CustomMeta
        {
            set;
            get;
        }
        public string MimeType
        {
            set;
            get;
        }
        public BlkputRet[] Progresses
        {
            set;
            get;
        }
        public int TryTimes
        {
            set;
            get;
        }
    }
}
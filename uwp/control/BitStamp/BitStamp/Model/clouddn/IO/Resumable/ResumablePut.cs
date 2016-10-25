// lindexi
// 15:38

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Qiniu.Auth;
using Qiniu.Conf;
using Qiniu.RPC;
using Qiniu.RS;
using Qiniu.Util;
using System.Threading.Tasks;

namespace Qiniu.IO.Resumable
{
    /// <summary>
    ///     异步并行断点上传类
    /// </summary>
    public class ResumablePut
    {
        /// <summary>
        ///     断点续传类
        /// </summary>
        /// <param name="putSetting"></param>
        /// <param name="extra"></param>
        public ResumablePut(Settings putSetting, ResumablePutExtra extra)
        {
            this.PutSetting = putSetting;
            this.Extra = extra;
        }

        private const int BlockBits = 22;
        private const int BlockMashk = (1 << BlockBits) - 1;

        /// <summary>
        ///     上传设置
        /// </summary>
        public Settings PutSetting
        {
            get;
            set;
        }

        /// <summary>
        ///     PutExtra
        /// </summary>
        public ResumablePutExtra Extra
        {
            get;
            set;
        }

        /// <summary>
        ///     上传完成事件
        /// </summary>
        public event EventHandler<CallRet> PutFinished;

        /// <summary>
        ///     上传Failure事件
        /// </summary>
        public event EventHandler<CallRet> PutFailure;

        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="upToken">上传Token</param>
        /// <param name="key">key</param>
        /// <param name="localFile">本地文件名</param>
        public async Task<CallRet> PutFile(string upToken, string localFile, string key)
        {
            if (!File.Exists(localFile))
            {
                throw new Exception(string.Format("{0} does not exist", localFile));
            }

            PutAuthClient client = new PutAuthClient(upToken);
            CallRet ret;
            using (FileStream fs = File.OpenRead(localFile))
            {
                int blockCnt = BlockCount(fs.Length);
                long fsize = fs.Length;
                Extra.Progresses = new BlkputRet[blockCnt];
                byte[] byteBuf = new byte[BLOCKSIZE];
                int readLen = BLOCKSIZE;
                for (int i = 0; i < blockCnt; i++)
                {
                    if (i == blockCnt - 1)
                    {
                        readLen = (int) (fsize - (long) i*BLOCKSIZE);
                    }
                    fs.Seek((long) i*BLOCKSIZE, SeekOrigin.Begin);
                    fs.Read(byteBuf, 0, readLen);
                    BlkputRet blkRet = await ResumableBlockPut(client, byteBuf, i, readLen);
                    if (blkRet == null)
                    {
                        Extra.OnNotifyErr(new PutNotifyErrorEvent(i, readLen, "Make Block Error"));
                    }
                    else
                    {
                        Extra.OnNotify(new PutNotifyEvent(i, readLen, Extra.Progresses[i]));
                    }
                }
                ret = await Mkfile(client, key, fsize);
            }
            if (ret.OK)
            {
                PutFinished?.Invoke(this, ret);
            }
            else
            {
                PutFailure?.Invoke(this, ret);
            }
            return ret;
        }

        private async Task<BlkputRet> ResumableBlockPut(Client client, byte[] body, int blkIdex, int blkSize)
        {
            #region Mkblock

            uint crc32 = CRC32.CheckSumBytes(body, blkSize);
            for (int i = 0; i < PutSetting.TryTimes; i++)
            {
                try
                {
                    Extra.Progresses[blkIdex] = await Mkblock(client, body, blkSize);
                }
                catch (Exception ee)
                {
                    if (i == PutSetting.TryTimes - 1)
                    {
                        throw;
                    }
                    await Task.Delay(1000);
                    //System.Threading.Thread.Sleep(1000);
                    continue;
                }
                if ((Extra.Progresses[blkIdex] == null) || (crc32 != Extra.Progresses[blkIdex].crc32))
                {
                    if (i == PutSetting.TryTimes - 1)
                    {
                        return null;
                    }
                    await Task.Delay(1000);
                    //System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
            }

            #endregion

            return Extra.Progresses[blkIdex];
        }

        private static async Task<BlkputRet> Mkblock(Client client, 
            byte[] firstChunk, 
            int blkSize)
        {
            string url = string.Format("{0}/mkblk/{1}", Config.UP_HOST, blkSize);

            CallRet callRet =
                await
                    client.CallWithBinary(url, "application/octet-stream", new MemoryStream(firstChunk, 0, blkSize),
                        blkSize);
            if (callRet.OK)
            {
                return QiniuJsonHelper.ToObject<BlkputRet>(callRet.Response);
            }
            return null;
        }

        private async Task<CallRet> Mkfile(Client client, string key, long fsize)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.AppendFormat("{0}/mkfile/{1}", Config.UP_HOST, fsize);
            if (key != null)
            {
                urlBuilder.AppendFormat("/key/{0}", Base64URLSafe.ToBase64URLSafe(key));
            }
            if (!string.IsNullOrEmpty(Extra.MimeType))
            {
                urlBuilder.AppendFormat("/mimeType/{0}", Base64URLSafe.ToBase64URLSafe(Extra.MimeType));
            }
            if (!string.IsNullOrEmpty(Extra.CustomMeta))
            {
                urlBuilder.AppendFormat("/meta/{0}", Base64URLSafe.ToBase64URLSafe(Extra.CustomMeta));
            }
            if ((Extra.CallbackParams != null) && (Extra.CallbackParams.Count > 0))
            {
                StringBuilder sb = new StringBuilder();
                foreach (string temp in Extra.CallbackParams.AllKeys /*Keys*/)
                {
                    sb.AppendFormat("/{0}/{1}", temp, Base64URLSafe.ToBase64URLSafe(Extra.CallbackParams[temp]));
                }
                urlBuilder.Append(sb.ToString());
            }

            int proCount = Extra.Progresses.Length;
            using (Stream body = new MemoryStream())
            {
                for (int i = 0; i < proCount; i++)
                {
                    byte[] bctx = Encoding.ASCII.GetBytes(Extra.Progresses[i].ctx);
                    body.Write(bctx, 0, bctx.Length);
                    if (i != proCount - 1)
                    {
                        body.WriteByte((byte) ',');
                    }
                }
                body.Seek(0, SeekOrigin.Begin);
                return await client.CallWithBinary(urlBuilder.ToString(), "text/plain", body, body.Length);
            }
        }

        private static int BlockCount(long fsize)
        {
            return (int) ((fsize + BlockMashk) >> BlockBits);
        }

        private static int BLOCKSIZE = 4*1024*1024;
    }
}
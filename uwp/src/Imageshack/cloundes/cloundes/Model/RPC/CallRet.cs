// lindexi
// 16:34

using System;
using System.Net;

namespace lindexi.uwp.ImageShack.Thirdqiniucs.Model.RPC
{
    public class CallRet : EventArgs
    {
        public CallRet(HttpStatusCode statusCode, string response)
        {
            StatusCode = statusCode;
            Response = response;
        }

        public CallRet(HttpStatusCode statusCode, Exception e)
        {
            StatusCode = statusCode;
            Exception = e;
        }

        public CallRet(CallRet ret)
        {
            StatusCode = ret.StatusCode;
            Exception = ret.Exception;
            Response = ret.Response;
        }

        public HttpStatusCode StatusCode
        {
            get;
            protected set;
        }

        public Exception Exception
        {
            get;
            protected set;
        }

        public string Response
        {
            get;
            protected set;
        }

        public bool OK
        {
            get
            {
                return (int) StatusCode / 100 == 2;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OTAManager.Server.Data;

namespace OTAManager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpdateManagerController : ControllerBase
    {
        public UpdateManagerController(OTAManagerServerContext context)
        {
            _context = context;

            if (!_context.LatestApplicationUpdateInfo.Any())
            {
                _context.LatestApplicationUpdateInfo.Add(new ApplicationUpdateInfoModel()
                {
                    ApplicationId = "123123123123",
                    Version = "5.1",
                    UpdateContext = "{\"Name\":\"\\u6797\\u5FB7\\u7199\\u662F\\u9017\\u6BD4\",\"ClientApplicationFileInfoList\":[{\"FilePath\":\"Installer.exe\",\"Md5\":\"9f650f3eb7be0a8e82efeb822f53f13a\",\"DownloadUrl\":\"https://10000.gd.cn/10000.gd_speedtest.exe\"},{\"FilePath\":\"Windows6.1-KB2533623-x86.msu.zip\",\"Md5\":\"dounide0000000000000000000000000\",\"DownloadUrl\":\"https://d.o0o0o0o.cn/Windows6.1-KB2533623-x86.msu.zip\"}],\"InstallerFileName\":null,\"InstallerArgument\":null}"
                });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ApplicationUpdateInfoModel Get([FromQuery]string applicationId)
        {
            return _context.LatestApplicationUpdateInfo.FirstOrDefault(temp =>
                temp.ApplicationId == applicationId);
        }

        // POST: /UpdateManager
        [HttpPost]
        public ApplicationUpdateInfoModel Post([FromBody] ApplicationUpdateRequest request)
        {
            return _context.LatestApplicationUpdateInfo.FirstOrDefault(temp =>
                temp.ApplicationId == request.ApplicationUpdateInfoModel.ApplicationId);
        }

        [HttpPut]
        public ApplicationUpdateInfoModel Put([FromBody] ApplicationUpdateInfoModel applicationUpdateInfoModel)
        {
            // 后续考虑安全性
            var updateInfo = _context.LatestApplicationUpdateInfo.FirstOrDefault(temp =>
                temp.ApplicationId == applicationUpdateInfoModel.ApplicationId);
            if (updateInfo != null)
            {
                var currentVersion = Version.Parse(updateInfo.Version);
                var version = Version.Parse(applicationUpdateInfoModel.Version);
                if (currentVersion < version)
                {
                    _context.LatestApplicationUpdateInfo.Remove(updateInfo);
                    _context.LatestApplicationUpdateInfo.Add(applicationUpdateInfoModel);
                    _context.SaveChanges();
                }
            }
            else
            {
                _context.LatestApplicationUpdateInfo.Add(applicationUpdateInfoModel);
                    _context.SaveChanges();
            }

            return _context.LatestApplicationUpdateInfo.FirstOrDefault(temp =>
                temp.ApplicationId == applicationUpdateInfoModel.ApplicationId);
        }

        ///// <summary>
        ///// 上传文件，将会返回文件下载链接
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("UploadFile")]
        //public IActionResult UploadFile([FromForm] UploadFileRequest request)
        //{

        //}

        //[HttpGet]
        //[Route("DownloadFile")]
        //public IActionResult DownloadFile([FromQuery]string key)
        //{

        //}

        private readonly OTAManagerServerContext _context;
    }

    ///// <summary>
    ///// 文件存储服务
    ///// </summary>
    //public class FileStorage
    //{
    //    public string UploadFile(UploadFileRequest request)
    //    {

    //    }

    //    public IActionResult DownloadFile(string key)
    //    {

    //    }
    //}

    public class ApplicationUpdateRequest
    {
        public ApplicationUpdateInfoModel ApplicationUpdateInfoModel { get; set; }
    }

    public class UploadFileRequest
    {
        public IFormFile File { set; get; }
        public string Name { get; set; }
    }
}

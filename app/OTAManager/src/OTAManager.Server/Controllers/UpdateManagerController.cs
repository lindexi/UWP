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
                    Version = "5.1"
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
        public ApplicationUpdateInfoModel Update([FromBody] ApplicationUpdateInfoModel applicationUpdateInfoModel)
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

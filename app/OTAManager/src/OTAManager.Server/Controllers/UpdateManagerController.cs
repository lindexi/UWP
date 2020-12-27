using System;
using System.Collections.Generic;
using System.Linq;
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
                _context.LatestApplicationUpdateInfo.Add(new ApplicationUpdateInfo()
                {
                    ApplicationId = "123123123123"
                });
                _context.SaveChanges();
            }
        }

        // POST: /UpdateManager
        [HttpPost]
        public ApplicationUpdateInfo Post([FromBody] ApplicationUpdateRequest request)
        {
            return _context.LatestApplicationUpdateInfo.FirstOrDefault(temp =>
                temp.ApplicationId == request.ApplicationUpdateInfo.ApplicationId);
        }

        [HttpPut]
        public ApplicationUpdateInfo Update([FromBody] ApplicationUpdateInfo applicationUpdateInfo)
        {
            // 后续考虑安全性
            var updateInfo = _context.LatestApplicationUpdateInfo.FirstOrDefault(temp =>
                temp.ApplicationId == applicationUpdateInfo.ApplicationId);
            if (updateInfo != null)
            {
                var currentVersion = Version.Parse(updateInfo.Version);
                var version = Version.Parse(applicationUpdateInfo.Version);
                if (currentVersion < version)
                {
                    _context.LatestApplicationUpdateInfo.Remove(updateInfo);
                    _context.LatestApplicationUpdateInfo.Add(applicationUpdateInfo);
                    _context.SaveChanges();
                }
            }
            else
            {
                _context.LatestApplicationUpdateInfo.Add(applicationUpdateInfo);
                    _context.SaveChanges();
            }

            return _context.LatestApplicationUpdateInfo.FirstOrDefault(temp =>
                temp.ApplicationId == applicationUpdateInfo.ApplicationId);
        }

        private readonly OTAManagerServerContext _context;
    }

    public class ApplicationUpdateRequest
    {
        public ApplicationUpdateInfo ApplicationUpdateInfo { get; set; }
    }


}

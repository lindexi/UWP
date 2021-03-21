using Microsoft.AspNetCore.Http;

namespace OTAManager.Server.Context
{
    public class UploadFileRequest
    {
        public IFormFile File { set; get; }
        public string Name { get; set; }
    }
}

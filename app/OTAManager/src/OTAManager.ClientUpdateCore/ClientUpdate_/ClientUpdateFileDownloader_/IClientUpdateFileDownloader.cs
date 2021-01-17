using System.Threading.Tasks;

namespace OTAManager.ClientUpdateCore
{
    public interface IClientUpdateFileDownloader
    {
        Task<IClientUpdateFileDownloadResult> Download(ClientUpdateFileDownloadContext clientUpdateFileDownloadContext);
    }
}

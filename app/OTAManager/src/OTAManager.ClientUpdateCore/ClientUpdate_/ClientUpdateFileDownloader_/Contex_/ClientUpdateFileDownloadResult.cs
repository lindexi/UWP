namespace OTAManager.ClientUpdateCore
{
    public class ClientUpdateFileDownloadResult : IClientUpdateFileDownloadResult
    {
        public ClientUpdateFileDownloadResult(bool success)
        {
            Success = success;
        }

        public bool Success { get; }
    }
}
using System.Threading.Tasks;

namespace OTAManager.ClientUpdateCore
{
    public interface IClientUpdateInstaller
    {
        Task Install(ClientUpdateInstallContext context);
    }
}

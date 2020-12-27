using System.Text.Json;

namespace OTAManager.ClientUpdateCore
{
    public class ClientUpdateManifestSerializer
    {
        public string Serialize(ClientUpdateManifest manifest)
        {
            return JsonSerializer.Serialize(manifest);
        }

        public ClientUpdateManifest Deserialize(string text)
        {
            return JsonSerializer.Deserialize<ClientUpdateManifest>(text);
        }
    }
}
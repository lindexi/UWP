using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace OTAManager.ClientUpdateCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientUpdateManifest = new ClientUpdateManifest()
            {
                Name = "林德熙应用",
                ClientApplicationFileInfos = new List<ClientApplicationFileInfo>()
                {
                    new ClientApplicationFileInfo()
                    {
                        FilePath = ClientUpdateManifest.DefaultInstallerFileName,
                        DownloadUrl = $"/download-file?file={ClientUpdateManifest.DefaultInstallerFileName}",
                        Md5 = string.Empty,
                    }
                },
                
            };

            var clientUpdateManifestSerializer = new ClientUpdateManifestSerializer();
            var text = clientUpdateManifestSerializer.Serialize(clientUpdateManifest);

            DownloadClientUpdateManifest(text);
        }

        private static void DownloadClientUpdateManifest(string text)
        {
            var clientUpdateManifestSerializer = new ClientUpdateManifestSerializer();
            var clientUpdateManifest = clientUpdateManifestSerializer.Deserialize(text);

            var clientUpdateDispatcher = new ClientUpdateDispatcher(clientUpdateManifest);
            clientUpdateDispatcher.Start();

           

        }
    }
}

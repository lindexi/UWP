using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Storage.Streams;

namespace BitStamp.Model;

internal static class LoggerFileProvider
{
    public static async Task AppendLogMessage(string message)
    {
        try
        {
            StorageFolder temporaryFolder = ApplicationData.Current.TemporaryFolder;
            StorageFile storageFile = await temporaryFolder.CreateFileAsync("Log.txt", CreationCollisionOption.OpenIfExists);

            using Stream fileStream = await storageFile.OpenStreamForWriteAsync();
            using var streamWriter = new StreamWriter(fileStream,Encoding.UTF8);
            await streamWriter.WriteLineAsync(message);
        }
        catch
        {
            // 强行忽略
        }
    }
}

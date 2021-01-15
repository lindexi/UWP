using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace OTAManager.ClientUpdateCore
{
    /// <summary>
    /// 默认的应用安装器
    /// </summary>
    /// 逻辑是先尝试找到 installer.exe 来安装。如果此文件不存在，同时只是下载了一个压缩包，那么将压缩包解压缩一下，然后再次尝试找 installer.exe 来安装
    public class DefaultClientUpdateInstaller : IClientUpdateInstaller
    {
        public async Task Install(ClientUpdateInstallContext context)
        {
            // 先判断安装文件是否存在
            var installerFileName = context.InstallerFileName;

            var installerFile = Path.Combine(context.WorkFolder.FullName, installerFileName);

            if (File.Exists(installerFile))
            {
                // 然后运行安装器
                var process = Process.Start(installerFile, context.InstallerArgument ?? string.Empty);
                await process.WaitForExitAsync();
                return;
            }
            else
            {
                // 是不是只是下载了一个压缩包，如果是的话，那就解压缩一下咯
                var clientApplicationFileInfoList = context.ClientUpdateManifest.ClientApplicationFileInfoList;
                if (clientApplicationFileInfoList.Count == 1)
                {
                    var filePath = clientApplicationFileInfoList[0].FilePath;
                    if (Path.GetExtension(filePath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                    {
                        // 尝试解压缩一下
                        // 然后再运行
                        var newWorkFolder = UnzipFile(context.WorkFolder, filePath);
                        if (newWorkFolder != null)
                        {
                            installerFile = Path.Combine(newWorkFolder.FullName, installerFileName);
                            if (File.Exists(installerFile))
                            {
                                // 然后运行安装器
                                var process = Process.Start(installerFile, context.InstallerArgument ?? string.Empty);
                                await process.WaitForExitAsync();
                                return;
                            }
                            else
                            {
                                // 不知道如何做了
                                //goto Exception;
                            }
                        }
                        else
                        {
                            // 不知道如何做了
                            //goto Exception;
                        }
                    }
                    else
                    {
                        // 不知道如何做了
                        //goto Exception;
                    }
                }
                else
                {
                    // 不知道怎么做了
                    //goto Exception;
                }
            }

#pragma warning disable CS0164 // 这个标签尚未被引用
Exception:
            throw new ArgumentException($"根据传入的参数，无法完成应用更新");
#pragma warning restore CS0164 // 这个标签尚未被引用
        }

        private DirectoryInfo? UnzipFile(DirectoryInfo workFolder, string filePath)
        {
            var file = Path.Combine(workFolder.FullName, filePath);
            if (File.Exists(file))
            {
                var unzipFolder = Path.Combine(workFolder.FullName, Path.GetRandomFileName());

                ZipFile.ExtractToDirectory(file, unzipFolder);
                return new DirectoryInfo(unzipFolder);
            }
            else
            {
                return null;
            }
        }
    }
}

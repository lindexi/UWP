using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace OTAManager.Server.Core
{
    public static class FileHelper
    {
        public static void MoveFile(string sourceFile, string destFile)
        {
            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (IsSameDrive(sourceFile, destFile))
                {
                    File.Move(sourceFile, destFile);
                }
                else
                {
                    // 先复制再删除
                    File.Copy(sourceFile, destFile);
                    File.Delete(sourceFile);
                }
            }
            else
            {
                File.Move(sourceFile, destFile);
            }
        }

        public static bool IsSameDrive(string file1, string file2)
        {
            var driveInfo1 = new DriveInfo(file1);
            var driveInfo2 = new DriveInfo(file2);

            return driveInfo1.Name == driveInfo2.Name;
        }

        public static string GetSafeFileName(string arbitraryString)
        {
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            var replaceIndex = arbitraryString.IndexOfAny(invalidChars, 0);
            if (replaceIndex == -1) return arbitraryString;

            var r = new StringBuilder();
            var i = 0;

            do
            {
                r.Append(arbitraryString, i, replaceIndex - i);

                switch (arbitraryString[replaceIndex])
                {
                    case '"':
                        r.Append("''");
                        break;
                    case '<':
                        r.Append('\u02c2'); // '˂' (modifier letter left arrowhead)
                        break;
                    case '>':
                        r.Append('\u02c3'); // '˃' (modifier letter right arrowhead)
                        break;
                    case '|':
                        r.Append('\u2223'); // '∣' (divides)
                        break;
                    case ':':
                        r.Append('-');
                        break;
                    case '*':
                        r.Append('\u2217'); // '∗' (asterisk operator)
                        break;
                    case '\\':
                    case '/':
                        r.Append('\u2044'); // '⁄' (fraction slash)
                        break;
                    case '\0':
                    case '\f':
                    case '?':
                        break;
                    case '\t':
                    case '\n':
                    case '\r':
                    case '\v':
                        r.Append(' ');
                        break;
                    default:
                        r.Append('_');
                        break;
                }

                i = replaceIndex + 1;
                replaceIndex = arbitraryString.IndexOfAny(invalidChars, i);
            } while (replaceIndex != -1);

            r.Append(arbitraryString, i, arbitraryString.Length - i);

            return r.ToString();
        }
    }
}

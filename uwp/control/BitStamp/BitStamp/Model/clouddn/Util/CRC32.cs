// lindexi
// 16:34

using System.IO;

namespace lindexi.uwp.ImageShack.Model.Util
{
    public class CRC32
    {
        public CRC32()
        {
            _value = 0;
            _table = MakeTable(IEEE);
        }

        public const uint IEEE = 0xedb88320;

        public void Write(byte[] p, int offset, int count)
        {
            _value = Update(_value, _table, p, offset, count);
        }

        public uint Sum32()
        {
            return _value;
        }

        public static uint Update(uint crc, uint[] table, byte[] p,
            int offset, int count)
        {
            crc = ~crc;
            for (int i = 0; i < count; i++)
            {
                crc = table[(byte) crc ^ p[offset + i]] ^ (crc >> 8);
            }
            return ~crc;
        }

        public static uint CheckSumBytes(byte[] data, int length)
        {
            CRC32 crc = new CRC32();
            crc.Write(data, 0, length);
            return crc.Sum32();
        }

        public static uint CheckSumFile(string fileName)
        {
            CRC32 crc = new CRC32();
            int bufferLen = 32 * 1024;
            using (FileStream stream = File.OpenRead(fileName))
            {
                byte[] buffer = new byte[bufferLen];
                while (true)
                {
                    int n = stream.Read(buffer, 0, bufferLen);
                    if (n == 0)
                    {
                        break;
                    }
                    crc.Write(buffer, 0, n);
                }
            }
            return crc.Sum32();
        }

        private uint[] _table;
        private uint _value;

        private static uint[] MakeTable(uint poly)
        {
            uint[] table = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                var crc = (uint) i;
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = (crc >> 1) ^ poly;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
                table[i] = crc;
            }
            return table;
        }
    }
}

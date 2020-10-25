using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace lindexi.src
{
    public class MacAddress
    {
        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> GetActiveMacAddress(string separator = "-")
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            // 方法里面的注释不删除
            //Debug.WriteLine("Interface information for {0}.{1}     ",
            //    computerProperties.HostName, computerProperties.DomainName);
            if (nics == null || nics.Length < 1)
            {
                Debug.WriteLine("  No network interfaces found.");
                return new List<string>();
            }

            var macAddress = new List<string>();

            //Debug.WriteLine("  Number of interfaces .................... : {0}", nics.Length);
            foreach (NetworkInterface adapter in nics.Where(c =>
                c.NetworkInterfaceType != NetworkInterfaceType.Loopback && c.OperationalStatus == OperationalStatus.Up))
            {
                //Debug.WriteLine("");
                //Debug.WriteLine(adapter.Name + "," + adapter.Description);
                //Debug.WriteLine(string.Empty.PadLeft(adapter.Description.Length, '='));
                //Debug.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                //Debug.Write("  Physical address ........................ : ");
                //PhysicalAddress address = adapter.GetPhysicalAddress();
                //byte[] bytes = address.GetAddressBytes();
                //for (int i = 0; i < bytes.Length; i++)
                //{
                //    // Display the physical address in hexadecimal.
                //    Debug.Write($"{bytes[i]:X2}");
                //    // Insert a hyphen after each byte, unless we are at the end of the 
                //    // address.
                //    if (i != bytes.Length - 1)
                //    {
                //        Debug.Write("-");
                //    }
                //}

                //Debug.WriteLine("");

                //Debug.WriteLine(address.ToString());

                IPInterfaceProperties properties = adapter.GetIPProperties();

                var unicastAddresses = properties.UnicastAddresses;
                if (unicastAddresses.Any(temp => temp.Address.AddressFamily == AddressFamily.InterNetwork))
                {
                    var address = adapter.GetPhysicalAddress();
                    if (string.IsNullOrEmpty(separator))
                    {
                        macAddress.Add(address.ToString());
                    }
                    else
                    {
                        macAddress.Add(string.Join(separator, address.GetAddressBytes()));
                    }
                }
            }

            return macAddress;
        }
    }
}

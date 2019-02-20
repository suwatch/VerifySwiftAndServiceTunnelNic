using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace VerifySwiftAndServiceTunnelNic
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Swift: n/a, ServiceTunnel: n/a"
                    // Swift: n/a, ServiceTunnel: Up/2603.."
                    // Swift: Up/10.0.0.16, ServiceTunnel: n/a"

                    if (nic.Description.StartsWith("ServiceTunnelAdapter"))
                    {
                        // nic.OperationalStatus: Up
                        Console.WriteLine("nic.OperationalStatus: {0}", nic.OperationalStatus);
                        // nic.Description: ServiceTunnelAdapter
                        Console.WriteLine("nic.Description: {0}", nic.Description);

                        Console.WriteLine("nic.IsReceiveOnly: {0}", nic.IsReceiveOnly);
                        Console.WriteLine("nic.Speed: {0}", nic.Speed);
                        Console.WriteLine("nic.Name: {0}", nic.Name);
                        Console.WriteLine("nic.Id: {0}", nic.Id);
                        Console.WriteLine("nic.NetworkInterfaceType: {0}", nic.NetworkInterfaceType);
                        Console.WriteLine("nic.SupportsMulticast: {0}", nic.SupportsMulticast);
                        Console.WriteLine("nic.GetPhysicalAddress(): {0}", ConvertToPhysicalAddress(nic.GetPhysicalAddress().GetAddressBytes()));

                        foreach (var ip in nic.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6 &&
                                !ip.Address.IsIPv6LinkLocal)
                            {
                                // InterNetworkV6: 2603:10e1:100:2::a00:7
                                Console.WriteLine("{0}: {1}", ip.Address.AddressFamily, ip.Address);
                            }
                        }
                    }
                    else if (nic.Description.StartsWith("Swift_"))
                    {
                        // nic.OperationalStatus: Up
                        Console.WriteLine("nic.OperationalStatus: {0}", nic.OperationalStatus);
                        // nic.Description: Swift_D0D59599-B110-4D5E-99B5-F24B5C1AACE5
                        Console.WriteLine("nic.Description: {0}", nic.Description);

                        Console.WriteLine("nic.IsReceiveOnly: {0}", nic.IsReceiveOnly);
                        Console.WriteLine("nic.Speed: {0}", nic.Speed);
                        Console.WriteLine("nic.Name: {0}", nic.Name);
                        Console.WriteLine("nic.Id: {0}", nic.Id);
                        Console.WriteLine("nic.NetworkInterfaceType: {0}", nic.NetworkInterfaceType);
                        Console.WriteLine("nic.SupportsMulticast: {0}", nic.SupportsMulticast);
                        Console.WriteLine("nic.GetPhysicalAddress(): {0}", ConvertToPhysicalAddress(nic.GetPhysicalAddress().GetAddressBytes()));

                        foreach (var ip in nic.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                // InterNetwork: 10.0.16.4
                                Console.WriteLine("{0}: {1}", ip.Address.AddressFamily, ip.Address);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static string ConvertToPhysicalAddress(byte[] bytes)
        {
            var str = new StringBuilder();
            for (var i = 0; i < bytes.Length; ++i)
            {
                str.AppendFormat("{0}{1}", i == 0 ? string.Empty : "-", bytes[i].ToString("X2"));
            }

            return str.ToString();
        }
    }
}
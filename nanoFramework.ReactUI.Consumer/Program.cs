using System.Diagnostics;
using System.Threading;

namespace nanoFramework.ReactUI.Consumer
{
    public class Program
    {
        public static void Main()
        {
            WirelessAP.SetWiFiAP();

            var server = new ResourcesServer();

            server.Start();

            Debug.WriteLine("Hello from nanoFramework!");

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
using System;
using System.Diagnostics;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.Text;

namespace PictureAuction.Api.Host
{
    internal static class Program
    {
        private static void Main()
        {
            using (var appHost = new AppHost())
            {
#if DEBUG
                LogManager.LogFactory = new ConsoleLogFactory();
#endif

                appHost.Init();
                appHost.Start("http://*:1337/");

                "ServiceStack SelfHost listening at http://localhost:1337 ".Print();

                Console.ReadLine();
            }
        }
    }
}
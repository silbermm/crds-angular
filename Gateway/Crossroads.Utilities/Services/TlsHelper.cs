using System.Net;

namespace Crossroads.Utilities.Services
{
    public class TlsHelper
    {
        public static void AllowTls12()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

    }
}
using System.Threading;

namespace Crossroads.Utilities.Services
{
    public class ImpersonatedUserGuid
    {
        private static ThreadLocal<string> _instance = new ThreadLocal<string>();

        public static void Set(string guid)
        {
            _instance.Value = guid;
        }

        public static string Get()
        {
            return (_instance.Value);
        }

        public static bool HasValue()
        {
            return (_instance.IsValueCreated && !string.IsNullOrWhiteSpace(_instance.Value));
        }

        public static void Clear()
        {
            _instance = new ThreadLocal<string>();
        }
    }
}

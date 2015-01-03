using System.Configuration;
namespace TYW.SDK
{
    class TywiConfiguration
    {
        public static string RequestUserAgent { get; private set; }

        public static int RequestTimeout { get; private set; }

        public static string AuthServiceUri { get; private set; }

        public static string TywiServiceUri { get; private set; }

        static TywiConfiguration()
        {
            RequestUserAgent = "Test";
            RequestTimeout = 30000;
            AuthServiceUri = ConfigurationManager.AppSettings["AuthApi"];
            TywiServiceUri = ConfigurationManager.AppSettings["TywiApi"];
        }
    }
}

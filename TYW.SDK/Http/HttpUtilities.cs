using System.Text;
using System.Web.Script.Serialization;
namespace TYW.SDK.Http
{
    public class HttpUtilities
    {
        public static byte[] EncodeString(string str)
        {
            return str == null ? null : ASCIIEncoding.ASCII.GetBytes(str);
        }

        public static T ParseJson<T>(string str)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return (T)serializer.Deserialize(str, typeof(T));
        }

        public static string EncodeJson<T>(T _object)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(_object);
        }

        public enum Methods
        {
            GET, POST, PUT, DELETE
        }

        internal static string ByteArrayToString(byte[] buffer)
        {
            return ASCIIEncoding.ASCII.GetString(buffer);
        }

        internal static string CreateRandomId(int p)
        {
            return "randomid";
        }
    }
}

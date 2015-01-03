using System.IO;
using System.Net;
using System.Web;

namespace TYW.SDK.Http
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of the request payload</typeparam>
    /// <typeparam name="U">Type of the response payload</typeparam>
    public class AbstractApiRequest<T, U>
    {
        /// <summary>
        /// Target uri for the request
        /// </summary>
        private string _uri;

        /// <summary>
        /// API request verb
        /// </summary>
        private HttpUtilities.Methods _method;
        
        /// <summary>
        /// User agent string to use with requests
        /// </summary>
        private string _userAgent;

        /// <summary>
        /// Request timeout when connecting to the API
        /// </summary>
        private int _timeout;

        /// <summary>
        /// Payload data, if any
        /// </summary>
        private T _data;

        public AbstractApiRequest(string uri, HttpUtilities.Methods method, T payloadData, string userAgent, int timeout)
        {
            _method = method;
            _userAgent = userAgent;
            _uri = uri;
            _data = payloadData;
            _timeout = timeout;
        }

        public virtual HttpWebRequest GetHttpRequest()
        {
            //-------------------------------------------------------------------------------------
            // Initialise HTTP request with correct headers for target URI
            //-------------------------------------------------------------------------------------
            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(_uri);
            httpRequest.KeepAlive = true;
            httpRequest.UserAgent = _userAgent;
            httpRequest.AllowAutoRedirect = false;
            httpRequest.ServicePoint.Expect100Continue = false;
            httpRequest.Timeout = _timeout;
            httpRequest.Method = _method.ToString();

            ProcessHeaders(httpRequest);

            //-------------------------------------------------------------------------------------
            // Attach payload data, if any
            //-------------------------------------------------------------------------------------
            if (_data != null)
            {
                string json = HttpUtilities.EncodeJson<T>(_data);

                httpRequest.ContentType = "text/json";
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
            }

            return httpRequest;
        }

        protected virtual void ProcessHeaders(HttpWebRequest request)
        {

        }

        public ApiResponseWrapper<U> ParseResponse(HttpWebResponse httpResponse)
        {
            using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = reader.ReadToEnd();
                return HttpUtilities.ParseJson<ApiResponseWrapper<U>>(result);
            }
        }
    }
}

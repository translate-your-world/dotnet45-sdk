using System;
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
        /// The format of the data for the request
        /// </summary>
        private string _format;

        /// <summary>
        /// Payload data, if any
        /// </summary>
        private T _data;

        public AbstractApiRequest(string uri, HttpUtilities.Methods method, T payloadData, string userAgent, int timeout) :
            this(uri, method, payloadData, "text/json", userAgent, timeout)
        {

        }

        public AbstractApiRequest(string uri, HttpUtilities.Methods method, T payloadData, string format, string userAgent, int timeout)
        {
            _method = method;
            _userAgent = userAgent;
            _uri = uri;
            _data = payloadData;
            _timeout = timeout;
            _format = format;
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
                httpRequest.ContentType = _format;
                if (_format == "text/json")
                {
                    string json = HttpUtilities.EncodeJson<T>(_data);
                    httpRequest.ContentLength = json.Length;
                    using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                    }
                }
                else
                {
                    byte[] data = _data as byte[];
                    httpRequest.ContentLength = data.Length;
                    Console.WriteLine("Sending audio length: " + data.Length);
                    using (Stream stream = httpRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
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

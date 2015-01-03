using System;
using System.Net;
namespace TYW.SDK.Http
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; private set; }
        
        public HttpStatusCode HttpCode { get; private set; }
        
        public ApiErrorDetail Detail { get; private set; }
    }

    public class ApiErrorDetail
    {
        public bool Okay { get; set; }

        public string Message { get; set; }

        public int Code { get; set; }
    }
}

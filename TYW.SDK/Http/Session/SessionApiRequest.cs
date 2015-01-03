using System.Net;
using TYW.SDK.Http;
using TYW.SDK.Http.Session.Models;
using TYW.SDK.Models;

namespace TYW.SDK.Http.Session
{
    public class SessionApiRequest<T, U> : AbstractApiRequest<T, U>
    {
        protected DeviceProfile _device;

        public SessionApiRequest(DeviceProfile device, string uri, HttpUtilities.Methods method, T data) 
            : base(uri, method, data, TywiConfiguration.RequestUserAgent, TywiConfiguration.RequestTimeout)  
        {
            _device = device;
        }

        protected override void ProcessHeaders(HttpWebRequest request)
        {
            request.Headers["account"] = _device.accountName;
            request.Headers["bearer"] = _device.bearerToken;
            request.Headers["device"] = _device.deviceId;
        }
    }
}

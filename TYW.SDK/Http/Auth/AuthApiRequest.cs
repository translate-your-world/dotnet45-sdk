using TYW.SDK.Http;

namespace TYW.SDK.Http.Auth
{
    public class AuthApiRequest<T, U> : AbstractApiRequest<T, U>
    {
        public AuthApiRequest(string uri, HttpUtilities.Methods method, T data) 
            : base(uri, method, data, TywiConfiguration.RequestUserAgent, TywiConfiguration.RequestTimeout)  
        {
        }
    }
}

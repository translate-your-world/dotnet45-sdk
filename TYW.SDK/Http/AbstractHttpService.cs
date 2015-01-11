using System.Net;
namespace TYW.SDK.Http
{
    abstract public class AbstractHttpService
    {
        protected virtual U ProcessRequest<T, U>(AbstractApiRequest<T, U> apiRequest)
        {
            HttpWebRequest httpRequest = apiRequest.GetHttpRequest();

            using (HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse())
            {
                ApiResponseWrapper<U> response = apiRequest.ParseResponse(httpResponse);
                if (response.Data != null)
                {
                    return response.Data;
                }
                else if (response.Status != null && !response.Status.Okay)
                {
                    throw new TywiException(response.Status.Message);
                }
                else
                {
                    return default(U);
                }
            }
        }
    }
}

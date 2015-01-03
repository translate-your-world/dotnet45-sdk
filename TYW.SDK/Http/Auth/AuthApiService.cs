using System;
using System.Configuration;
using TYW.SDK.Http.Auth.Models;
using TYW.SDK.Models;

namespace TYW.SDK.Http.Auth
{
    /// <summary>
    /// Helper service for connecting to authorisation API
    /// </summary>
    public class AuthApiService : AbstractHttpService
    {
        /// <summary>
        /// The TYWI account to use for authorisation
        /// </summary>
        private TywiAccount _account;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="account">The TYWI account to use for authorisation</param>
        public AuthApiService(TywiAccount account)
        {
            _account = account;
        }

        /// <summary>
        /// Request an access token. This is a temporary token that grants us short-term access to the main
        /// athorisation endpoint
        /// </summary>
        public TokenResponse RequestToken()
        {
            string absoluteUri = null;
            try
            {
                string relativeUri = String.Format(UriTemplates.TOKEN_REQUEST, _account.Name, _account.ClientId);
                absoluteUri = TywiConfiguration.AuthServiceUri + relativeUri;
                AuthApiRequest<string, TokenResponse> request = new AuthApiRequest<string, TokenResponse>(absoluteUri, Http.HttpUtilities.Methods.GET, null);
                return this.ProcessRequest<string, TokenResponse>(request);
            }
            catch (Exception exc)
            {
                throw new AccessDeniedException(String.Format("Authorisation failed for account {0}", _account.Name), exc);
            }
        }

        /// <summary>
        /// Get a full access token for the main TYWI API.
        /// </summary>
        /// <param name="accessToken">The access token</param>
        /// <returns></returns>
        public AccessResponse Authorise(string accessToken)
        {
            try
            {
                string tokenUri = TywiConfiguration.AuthServiceUri + UriTemplates.AUTHORISE_REQUEST;
                AccessRequest accessRequest = new AccessRequest() { clientSecret = _account.ClientSecret, clientId = _account.ClientId, code = accessToken };
                AuthApiRequest<AccessRequest, AccessResponse> request = new AuthApiRequest<AccessRequest, AccessResponse>(tokenUri, Http.HttpUtilities.Methods.POST, accessRequest);
                return this.ProcessRequest<AccessRequest, AccessResponse>(request);
            }
            catch (Exception exc)
            {
                throw new AccessDeniedException(String.Format("Authorisation failed for account {0}", _account.Name), exc);
            }
        }

        /// <summary>
        /// Utility method to perform both authorisation steps
        /// </summary>
        /// <returns>The authorisation token for access to the TYWI API</returns>
        internal string Authorise()
        {
            TokenResponse token = this.RequestToken();
            AccessResponse auth = this.Authorise(token.accessCode);
            return auth.accessToken;
        }

        /// <summary>
        /// Uri templates for the authorisation endpoints
        /// </summary>
        class UriTemplates
        {
            public const string TOKEN_REQUEST = "token?account={0}&client_id={1}";
            public const string AUTHORISE_REQUEST = "authorise";
        }
    }
}

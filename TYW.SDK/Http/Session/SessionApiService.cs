using System;
using System.Configuration;
using TYW.SDK.Http;
using TYW.SDK.Http.Session.Models;
using TYW.SDK.Models;

namespace TYW.SDK.Http.Session
{
    /// <summary>
    /// Helper service for connecting to the main TYWI session API
    /// </summary>
    public class SessionApiService : AbstractHttpService
    {
        /// <summary>
        /// The session to which the service should connect
        /// </summary>
        protected TywiSession _session;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session">The session to which the service should connect</param>
        public SessionApiService(TywiSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Get the latest text from the session
        /// </summary>
        /// <param name="device">The device details for the connection</param>
        /// <returns>TranscriptionModel with the latest text</returns>
        public TranscriptionModel GetText(DeviceProfile device)
        {
            try
            {
                string relativeUri = String.Format(UriTemplates.GET_TEXT_URI, _session.SessionId, device.profile.id);
                string absoluteUri = TywiConfiguration.TywiServiceUri + relativeUri;
                SessionApiRequest<string, TranscriptionModel> request = new SessionApiRequest<string, TranscriptionModel>(
                    device, absoluteUri, Http.HttpUtilities.Methods.GET, null);
                return this.ProcessRequest<string, TranscriptionModel>(request);
            }
            catch (Exception exc)
            {
                throw new AccessDeniedException(String.Format("Failed to get text for device {0}", device.profile.id), exc);
            }
        }

        /// <summary>
        /// Post text to the session
        /// </summary>
        /// <param name="text"></param>
        public void PostText(TextBlobModel text, DeviceProfile device)
        {
            try
            {
                string relativeUri = String.Format(UriTemplates.POST_TEXT_URI, _session.SessionId, device.profile.id);
                string absoluteUri = TywiConfiguration.TywiServiceUri + relativeUri;
                SessionApiRequest<TextBlobModel, TextBlobModel> request = new SessionApiRequest<TextBlobModel, TextBlobModel>(
                    device, absoluteUri, Http.HttpUtilities.Methods.POST, text);
                this.ProcessRequest<TextBlobModel, TextBlobModel>(request);
            }
            catch (Exception exc)
            {
                throw new AccessDeniedException(String.Format("Failed to post text from device {0}: {1}", device.profile.id, exc.Message), exc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        internal AudioModel GetAudio(DeviceProfile device)
        {
            try
            {
                string relativeUri = String.Format(UriTemplates.POST_TEXT_URI, _session.SessionId, device.profile.id);
                string absoluteUri = TywiConfiguration.TywiServiceUri + relativeUri;
                SessionApiRequest<string, AudioModel> request = new SessionApiRequest<string, AudioModel>(
                    device, absoluteUri, Http.HttpUtilities.Methods.GET, null);
                return this.ProcessRequest<string, AudioModel>(request);
            }
            catch (Exception exc)
            {
                throw new AccessDeniedException(String.Format("Failed to post text from device {0}", device.profile.id), exc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="device"></param>
        internal void PostAudio(byte[] audio, DeviceProfile device)
        {
            try
            {
                string relativeUri = String.Format(UriTemplates.POST_AUDIO_URI, _session.SessionId, device.profile.id);
                string absoluteUri = TywiConfiguration.TywiServiceUri + relativeUri;
                SessionApiRequest<byte[], byte[]> request = new SessionApiRequest<byte[], byte[]>(
                    device, absoluteUri, Http.HttpUtilities.Methods.POST, audio);
                this.ProcessRequest<byte[], byte[]>(request);
            }
            catch (Exception exc)
            {
                throw new AccessDeniedException(String.Format("Failed to post audio from device {0}: {1}", device.profile.id, exc.Message), exc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal SessionModel PostSession(SessionModel session, DeviceProfile device)
        {
            try
            {
                string absoluteUri = TywiConfiguration.TywiServiceUri + UriTemplates.POST_SESSION_URI;
                SessionApiRequest<SessionModel, SessionModel> request = new SessionApiRequest<SessionModel, SessionModel>(
                    device, absoluteUri, Http.HttpUtilities.Methods.POST, session);
                return this.ProcessRequest<SessionModel, SessionModel>(request);
            }
            catch (Exception exc)
            {
                throw new AccessDeniedException(String.Format("Failed to post audio from device {0}: {1}", device.profile.id, exc.Message), exc);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="device"></param>
        internal UserProfileModel PostProfile(UserProfileModel userProfile, DeviceProfile device)
        {
            try
            {
                string relativeUri = String.Format(UriTemplates.POST_PROFILE_URI, _session.SessionId);
                string absoluteUri = TywiConfiguration.TywiServiceUri + relativeUri;
                SessionApiRequest<UserProfileModel, UserProfileModel> request = new SessionApiRequest<UserProfileModel, UserProfileModel>(
                    device, absoluteUri, Http.HttpUtilities.Methods.POST, userProfile);
                return this.ProcessRequest<UserProfileModel, UserProfileModel>(request);
            }
            catch (Exception exc)
            {
                throw new AccessDeniedException(String.Format("Failed to post audio from device {0}: {1}", device.profile.id, exc.Message), exc);
            }
        }

        class UriTemplates
        {
            public const string POST_SESSION_URI = "session";
            public const string POST_PROFILE_URI = "session/{0}/profile";
            public const string GET_PROFILE_URI = "session/{0}/profile/{1}";
            public const string PUT_PROFILE_URI = "session/{0}/profile/{1}";
            public const string GET_TEXT_URI = "session/{0}/transcriptions";
            public const string POST_TEXT_URI = "session/{0}/profile/{1}/text";
            public const string GET_AUDIO_URI = "session/{0}/audiofiles";
            public const string POST_AUDIO_URI = "session/{0}/profile/{1}/audio";
        }
    }
}

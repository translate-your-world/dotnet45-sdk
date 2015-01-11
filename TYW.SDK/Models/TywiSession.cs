using System;
using System.IO;
using System.Threading;
using TYW.SDK.Audio;
using TYW.SDK.Http;
using TYW.SDK.Http.Auth;
using TYW.SDK.Http.Session;
using TYW.SDK.Http.Session.Models;

namespace TYW.SDK.Models
{
    public class TywiSession
    {
        #region factory

        /// <summary>
        /// Create a new TYWI Session using the given account
        /// </summary>
        public static TywiSession Create(TywiAccount account)
        {
            return ConnectTo(null, account);
        }

        /// <summary>
        /// Connect to an existing TYWI Session using the given account and ID
        /// </summary>
        public static TywiSession ConnectTo(string id, TywiAccount account)
        {
            AuthApiService authService = new AuthApiService(account);
            string bearerToken = authService.Authorise();

            return new TywiSession(account.Name, bearerToken, id);
        }

        #endregion

        #region data

        /// <summary>
        /// ID of the session to connect to
        /// </summary>
        public string SessionId { get; private set; }

        /// <summary>
        /// Account name 
        /// </summary>
        public string AccountName { get; private set; }

        /// <summary>
        /// Device
        /// </summary>
        public DeviceProfile Device { get; private set; }

        /// <summary>
        /// BearerToken
        /// </summary>
        public string BearerToken { get; private set; }

        /// <summary>
        /// Is the session active
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// Stream of text to send to TYWI 
        /// </summary>
        private TywiTextStream _inputText;

        /// <summary>
        /// Stream of text received from TYWI
        /// </summary>
        private TywiTextStream _outputText;

        /// <summary>
        /// Stream of audio to send to TYWI
        /// </summary>
        private TywiAudioStream _inputAudio;

        /// <summary>
        /// Stream of audio to receive from TYWI
        /// </summary>
        private TywiAudioStream _outputAudio;

        /// <summary>
        /// Service to handle connections to the TYWI API
        /// </summary>
        private SessionApiService _service;

        /// <summary>
        /// The thread on which the ongoing session process will run
        /// </summary>
        private Thread _sessionThread;

        /// <summary>
        /// Delegate to handle one of the TYWI API text streams
        /// </summary>
        private delegate void TextStreamHandlerDelegate(int index, TywiTextStream stream, DeviceProfile device);

        /// <summary>
        /// Delegate to handle one of the TYWI API audio streams
        /// </summary>
        private delegate void AudioStreamHandlerDelegate(int index, TywiAudioStream stream, DeviceProfile device);

        #endregion

        #region session

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountName">The name of the account through which we are conecting</param>
        /// <param name="bearerToken">The authorisation token for connecting to TYWI</param>
        /// <param name="sessionId">The ID of the session to which we are connecting (or null to create a new session on connection)</param>
        public TywiSession(string accountName, string bearerToken, string sessionId)
        {
            AccountName = accountName;
            BearerToken = bearerToken;
            SessionId = sessionId;
            _service = new SessionApiService(this);
        }

        public event EventHandler SessionConnected;

        public void Connect()
        {
            Active = true;
            _sessionThread = new Thread(this.Run);
            _sessionThread.Start();
        }

        public void Disconnect()
        {
            Active = false;
        }

        public void RegisterInputText(TywiTextStream stream)
        {
            _inputText = stream;
        }

        public void RegisterOutputText(TywiTextStream stream)
        {
            _outputText = stream;
        }

        public void RegisterInputAudio(TywiAudioStream stream)
        {
            _inputAudio = stream;
        }

        public void RegisterOutputAudio(TywiAudioStream stream)
        {
            _outputAudio = stream;
        }

        #endregion

        #region run

        private void Run()
        {
            InitDevice();
            InitSession();
            InitUser("Mark", "English", "Spanish");

            int index = 0;
            DateTime next = DateTime.Now;

            while (this.Active)
            {
                if (_inputText != null)
                {
                    TextStreamHandlerDelegate inputStreamHandler = new TextStreamHandlerDelegate(this.HandleInputText);
                    inputStreamHandler.BeginInvoke(index, _inputText, Device, null, null);
                }

                if (_outputText != null)
                {
                    TextStreamHandlerDelegate inputStreamHandler = new TextStreamHandlerDelegate(this.HandleOutputText);
                    inputStreamHandler.BeginInvoke(index, _outputText, Device, null, null);
                }

                if (_inputAudio != null)
                {
                    AudioStreamHandlerDelegate inputStreamHandler = new AudioStreamHandlerDelegate(this.HandleInputAudio);
                    inputStreamHandler.BeginInvoke(index, _inputAudio, Device, null, null);
                }

                if (_outputAudio != null)
                {
                    AudioStreamHandlerDelegate inputStreamHandler = new AudioStreamHandlerDelegate(this.HandleOutputAudio);
                    inputStreamHandler.BeginInvoke(index, _outputAudio, Device, null, null);
                }

                next = next.AddMilliseconds(500);
                double waitMs = (next - DateTime.Now).TotalMilliseconds;
                if (waitMs > 0) Thread.Sleep((int)waitMs);
                
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitDevice()
        {
            if (Device == null)
            {
                Device = new DeviceProfile()
                {
                    accountName = this.AccountName,
                    bearerToken = this.BearerToken,
                    deviceId = HttpUtilities.CreateRandomId(8),
                    profile = new UserProfileModel()
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitSession()
        {
            if (String.IsNullOrEmpty(SessionId))
            {
                SessionCapabilityModel[] capabilities = new SessionCapabilityModel[] { 
                    new SessionCapabilityModel() { name = "allowSecondarySpeakers", value = "true" },
                    new SessionCapabilityModel() { name = "defaultLayout", value = "medium" },
                    new SessionCapabilityModel() { name = "defaultTTS", value = "false" },
                };
                SessionModel session = new SessionModel() { capabilities = capabilities };
                SessionModel returnedSession = _service.PostSession(session, Device);
                this.SessionId = returnedSession.sessionId;
            }

            if (this.SessionConnected != null)
            {
                SessionConnected(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="language"></param>
        /// <param name="translationEngine"></param>
        private void InitUser(string name, string textLanguage, string audioLanguage)
        {
            UserProfileModel user = new UserProfileModel()
            {
                name = name,
                language = textLanguage,
                textLanguage = new LanguageStreamModel() { language = textLanguage, translationProvider = "Google" },
                audioLanguage = new LanguageStreamModel() { language = audioLanguage, translationProvider = "Google" },
            };
            Device.profile = _service.PostProfile(user, Device);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="stream"></param>
        /// <param name="device"></param>
        private void HandleInputText(int index, TywiTextStream stream, DeviceProfile device)
        {
            string text = stream.ReadText();
            if (text != null)
            {
                TextBlobModel textBlob = new TextBlobModel() { receivedTime = DateTime.Now, text = text };
                _service.PostText(textBlob, device);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="stream"></param>
        /// <param name="device"></param>
        private void HandleOutputText(int index, TywiTextStream stream, DeviceProfile device)
        {
            TranscriptionModel transcriptions = _service.GetText(device);
            foreach (var line in transcriptions.transcriptions)
                stream.WriteText(line.text);            
        }

        private void HandleInputAudio(int index, TywiAudioStream stream, DeviceProfile device)
        {

            byte[] content = stream.ReadAudio();
            if (content != null && content.Length > 0)
            {
                PcmWrapper wrapper = new PcmWrapper(content, 16000, 1, 16);
                _service.PostAudio(wrapper.Encode(), device);
            }
        }

        private void HandleOutputAudio(int index, TywiAudioStream stream, DeviceProfile device)
        {
            AudioModel audio = _service.GetAudio(device);
            foreach (var line in audio.files)
            {     
                stream.QueueAudio(line.audioFile);
            }
        }

        #endregion

        #region options

        #endregion
    }
}

using NAudio.Wave;
using System;
using System.IO;
using System.Windows;
using TYW.SDK.Models;
using TYW.SDK.Sample.Audio;
namespace TYW.SDK.Sample.ViewModels
{
    public class SessionModel : DependencyObject
    {
        public static readonly DependencyProperty IDProperty = DependencyProperty.Register(
            "ID", typeof(string), typeof(SessionModel));

        public static readonly DependencyProperty InputTextProperty = DependencyProperty.Register(
            "InputText", typeof(string), typeof(SessionModel), new PropertyMetadata(null, InputTextChanged));

        public static readonly DependencyProperty OutputTextProperty = DependencyProperty.Register(
            "OutputText", typeof(string), typeof(SessionModel));

        public string ID
        {
            get { return (string)this.GetValue(IDProperty); }
            set { this.SetValue(IDProperty, value); }
        }

        public string InputText
        {
            get { return (string)this.GetValue(InputTextProperty); }
            set { this.SetValue(InputTextProperty, value); }
        }

        public string OutputText
        {
            get { return (string)this.GetValue(OutputTextProperty); }
            set { this.SetValue(OutputTextProperty, value); }
        }

        public static void InputTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((SessionModel)sender).TextInput();
        }

        private TywiTextStream _textIn;
        private TywiTextStream _textOut;
        private TywiSession _session;

        public void Connect(AccountModel account)
        {
            _session = String.IsNullOrEmpty(ID) ? TywiSession.Create(account.GetTywiAccount()) : TywiSession.ConnectTo(ID, account.GetTywiAccount());
            _session.SessionConnected += OnSessionConnect;
            InitialiseText();
            InitialiseMicrophone();
            InitialiseAudio();
            _session.Connect();
        }

        private void OnSessionConnect(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => {
                this.ID = _session.SessionId;
            });
        }

        public void Disconnect(AccountModel account)
        {
            _session.Disconnect();
        }

        private void InitialiseText()
        {
            _textIn = new TywiTextStream(new MemoryStream());
            _session.RegisterInputText(_textIn);

            _textOut = new TywiTextStream(new MemoryStream());
            _textOut.TextUpdated += this.UpdateTicker;
            _session.RegisterOutputText(_textOut);
        }

        private void InitialiseMicrophone()
        {
            WaveIn waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new WaveFormat(44100, 1);

            AudioInStream stream = new AudioInStream(waveIn, new MemoryStream());
            _session.RegisterInputAudio(stream);
        }

        private void InitialiseAudio()
        {

        }

        private void TextInput()
        {
            string text = InputText;
            InputText = null;
            _textIn.WriteText(text);
        }

        private void UpdateTicker(string text)
        {
            Dispatcher.Invoke(() =>
            {
                this.OutputText += text + Environment.NewLine;
            });
        }
    }
}

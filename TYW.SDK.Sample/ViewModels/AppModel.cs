using System.Windows;
using System.Windows.Input;
using Tyw.SDK.Sample;

namespace TYW.SDK.Sample.ViewModels
{
    public class AppModel : DependencyObject
    {
        #region dependency property declarations

        public static readonly DependencyProperty AccountVisibleProperty = DependencyProperty.Register(
            "AccountVisible", typeof(Visibility), typeof(AppModel));

        public static readonly DependencyProperty SessionVisibleProperty = DependencyProperty.Register(
            "SessionVisible", typeof(Visibility), typeof(AppModel));

        public static readonly DependencyProperty CallVisibleProperty = DependencyProperty.Register(
            "CallVisible", typeof(Visibility), typeof(AppModel));

        public static readonly DependencyProperty AccountProperty = DependencyProperty.Register(
            "Account", typeof(AccountModel), typeof(AppModel));

        public static readonly DependencyProperty SessionProperty = DependencyProperty.Register(
            "Session", typeof(SessionModel), typeof(AppModel));

        public static readonly DependencyProperty ConnectedProperty = DependencyProperty.Register(
            "Connected", typeof(bool), typeof(AppModel));

        #endregion

        #region dependency properties

        public Visibility AccountVisible
        {
            get { return (Visibility)this.GetValue(AccountVisibleProperty); }
            set { this.SetValue(AccountVisibleProperty, value); }
        }

        public Visibility SessionVisible
        {
            get { return (Visibility)this.GetValue(SessionVisibleProperty); }
            set { this.SetValue(SessionVisibleProperty, value); }
        }

        public Visibility CallVisible
        {
            get { return (Visibility)this.GetValue(CallVisibleProperty); }
            set { this.SetValue(CallVisibleProperty, value); }
        }

        public AccountModel Account
        {
            get { return (AccountModel)this.GetValue(AccountProperty); }
            set { this.SetValue(AccountProperty, value); }
        }

        public SessionModel Session
        {
            get { return (SessionModel)this.GetValue(SessionProperty); }
            set { this.SetValue(SessionProperty, value); }
        }

        public bool Connected
        {
            get { return (bool)this.GetValue(ConnectedProperty); }
            set { this.SetValue(ConnectedProperty, value); }
        }

        #endregion

        #region init

        public AppModel()
        {
            AccountVisible = Visibility.Collapsed;
            SessionVisible = Visibility.Visible;
            CallVisible = Visibility.Collapsed;
            Account = new AccountModel();
            Session = new SessionModel();
        }

        #endregion

        #region commands

        private DelegateCommand _viewAccountCommand;
        public ICommand ViewAccountCommand
        {
            get
            {
                if (_viewAccountCommand == null)
                {
                    _viewAccountCommand = new DelegateCommand(ViewAccount);
                }
                return _viewAccountCommand;
            }
        }

        private DelegateCommand _saveAccountCommand;
        public ICommand SaveAccountCommand
        {
            get
            {
                if (_saveAccountCommand == null)
                {
                    _saveAccountCommand = new DelegateCommand(SaveAccount);
                }
                return _saveAccountCommand;
            }
        }

        private DelegateCommand _connectSessionCommand;
        public ICommand ConnectSessionCommand
        {
            get
            {
                if (_connectSessionCommand == null)
                {
                    _connectSessionCommand = new DelegateCommand(ConnectSession);
                }
                return _connectSessionCommand;
            }
        }

        private DelegateCommand _startSessionCommand;
        public ICommand StartSessionCommand
        {
            get
            {
                if (_startSessionCommand == null)
                {
                    _startSessionCommand = new DelegateCommand(StartSession);
                }
                return _startSessionCommand;
            }
        }

        private DelegateCommand _disconnectSession;
        public ICommand DisconnectSessionCommand
        {
            get
            {
                if (_disconnectSession == null)
                {
                    _disconnectSession = new DelegateCommand(DisconnectSession);
                }
                return _disconnectSession;
            }
        }        

        #endregion

        #region command actions

        public void ViewAccount()
        {
            AccountVisible = Visibility.Visible;
            SessionVisible = Visibility.Collapsed;
        }

        public void SaveAccount()
        {
            AccountVisible = Visibility.Collapsed;
            SessionVisible = Visibility.Visible;
        }

        public void ConnectSession()
        {
            CallVisible = Visibility.Visible;
            SessionVisible = Visibility.Collapsed;
            Session.Connect(Account);
        }

        public void StartSession()
        {
            CallVisible = Visibility.Visible;
            SessionVisible = Visibility.Collapsed;
            Session.Connect(Account);
        }

        public void DisconnectSession()
        {
            CallVisible = Visibility.Collapsed;
            SessionVisible = Visibility.Visible;
            Session.Disconnect(Account);
        }

        #endregion
    }
}

using System.Windows;
using TYW.SDK.Models;

namespace TYW.SDK.Sample.ViewModels
{
    public class AccountModel : DependencyObject
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof(string), typeof(AccountModel));

        public static readonly DependencyProperty ClientIDProperty = DependencyProperty.Register(
            "ClientID", typeof(string), typeof(AccountModel));

        public static readonly DependencyProperty ClientSecretProperty = DependencyProperty.Register(
            "ClientSecret", typeof(string), typeof(AccountModel));

        public string Account
        {
            get { return (string)this.GetValue(NameProperty); }
            set { this.SetValue(NameProperty, value); }
        }

        public string ClientID
        {
            get { return (string)this.GetValue(ClientIDProperty); }
            set { this.SetValue(ClientIDProperty, value); }
        }

        public string ClientSecret
        {
            get { return (string)this.GetValue(ClientSecretProperty); }
            set { this.SetValue(ClientSecretProperty, value); }
        }

        public AccountModel()
        {
            this.Account = "mdsomerfield";
            this.ClientID = "mark";
            this.ClientSecret = "Darjeeling";
        }

        public TywiAccount GetTywiAccount()
        {
            TywiAccount account = new TywiAccount()
            {
                ClientId = this.ClientID,
                ClientSecret = this.ClientSecret,
                Name = this.Account
            };
            return account;
        }
    }
}

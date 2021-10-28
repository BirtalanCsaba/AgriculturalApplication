using AgriculturalAplication.EventModels;
using AgriculturalAplication.Helpers;
using Caliburn.Micro;
using System;
using System.Threading.Tasks;

namespace AgriculturalAplication.ViewModels
{
    public class SignUpFormViewModel : Screen
    {

        #region Properties

        public string Username { get; set; }

        public string Email { get; set; }

        public static string Password { get; set; }

        public static string RePassword { get; set; }

        #region ErrorMessages

        public bool UsernameIsErrorVisible
        {
            get
            {
                bool output = false;

                if(UsernameErrorMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _usernameErrorMessage;

        public string UsernameErrorMessage
        {
            get { return _usernameErrorMessage; }
            set
            {
                _usernameErrorMessage = value;
                NotifyOfPropertyChange(() => UsernameIsErrorVisible);
                NotifyOfPropertyChange(() => UsernameErrorMessage);
            }
        }

        public bool EmailIsErrorVisible
        {
            get
            {
                bool output = false;

                if (EmailErrorMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _emailErrorMessage;

        public string EmailErrorMessage
        {
            get { return _emailErrorMessage; }
            set
            {
                _emailErrorMessage = value;
                NotifyOfPropertyChange(() => EmailIsErrorVisible);
                NotifyOfPropertyChange(() => EmailErrorMessage);
            }
        }

        public bool PasswordIsErrorVisible
        {
            get
            {
                bool output = false;

                if (PasswordErrorMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _passwordErrorMessage;

        public string PasswordErrorMessage
        {
            get { return _passwordErrorMessage; }
            set
            {
                _passwordErrorMessage = value;
                NotifyOfPropertyChange(() => PasswordIsErrorVisible);
                NotifyOfPropertyChange(() => PasswordErrorMessage);
            }
        }

        public bool RePasswordIsErrorVisible
        {
            get
            {
                bool output = false;

                if (RePasswordErrorMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _rePasswordErrorMessage;

        public string RePasswordErrorMessage
        {
            get { return _rePasswordErrorMessage; }
            set
            {
                _rePasswordErrorMessage = value;
                NotifyOfPropertyChange(() => RePasswordIsErrorVisible);
                NotifyOfPropertyChange(() => RePasswordErrorMessage);
            }
        }


        #endregion

        #endregion


        #region CaliburnMicroFunctions

        private IAPIHelper _apiHelper;
        private IEventAggregator _events;

        public SignUpFormViewModel(IEventAggregator events, IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        protected override void OnActivate()
        {
            Username = Email = Password = RePassword = "";
            UsernameErrorMessage = EmailErrorMessage = PasswordErrorMessage = RePasswordErrorMessage = "";
        }

        #endregion

        #region Buttons

        public void BackButton()
        {
            _events.PublishOnUIThread(new BackToLoginPageEventModel());
            TryClose();
        }

        public async Task Register()
        {
            bool ok = true;

            if (String.IsNullOrEmpty(Username))
            {
                ok = false;
                UsernameErrorMessage = "* Username Required !";
            }
            else UsernameErrorMessage = "";

            if (String.IsNullOrEmpty(Email))
            {
                ok = false;
                EmailErrorMessage = "* Email Required !";
            }
            else if (!Email.Contains("@"))
            {
                EmailErrorMessage = "* Bad Email format !";
                ok = false;
            }
            else EmailErrorMessage = "";

            if (String.IsNullOrEmpty(Password))
            {
                ok = false;
                PasswordErrorMessage = "* Password Required !";
            }
            else if (Password.Length < 6)
            {
                PasswordErrorMessage = "* Too Short. Must be at least 6 characters !";
                ok = false;
            }
            else PasswordErrorMessage = "";

            if (String.IsNullOrEmpty(RePassword))
            {
                ok = false;
                RePasswordErrorMessage = "* RePassword Required !";
            }
            else if (!Password.Equals(RePassword))
            {
                RePasswordErrorMessage = "* Password do not match !";
                ok = false;
            }
            else RePasswordErrorMessage = "";

            if(ok)
            {
                await _apiHelper.CreateUser(Username,Email,Password);
                BackButton();
            }
        }

        #endregion
    }
}

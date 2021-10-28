using AgriculturalAplication.EventModels;
using AgriculturalAplication.Helpers;
using AgriculturalAplication.TemporaryUserData;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriculturalAplication.ViewModels
{
    public class EditAccountViewModel : Screen
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

                if (UsernameErrorMessage.Length > 0)
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
        IEventAggregator _events;
        IAPIHelper _apiHelper;

        public EditAccountViewModel(IAPIHelper apiHelper,IEventAggregator events)
        {
            _events = events;
            _apiHelper = apiHelper;
        }

        protected override void OnActivate()
        {
            Username = "";
            Email = "";
            Password = "";
            RePassword = "";

            UsernameErrorMessage = "";
            EmailErrorMessage = "";
            PasswordErrorMessage = "";
            RePasswordErrorMessage = "";
        }

        #endregion

        #region Buttons

        public void BackButton()
        {
            _events.PublishOnUIThread(new BackToMainFormEventModel());
            TryClose();
        }

        public async Task ApplyChanges()
        {
            bool ok = true;

            if(!String.IsNullOrEmpty(Email) && !Email.Contains("@"))
            {
                ok = false;
                EmailErrorMessage = "* Invalid email format !";
            }

            if (!String.IsNullOrEmpty(Password))
            {
                if (String.IsNullOrEmpty(RePassword))
                {
                    ok = false;
                    RePasswordErrorMessage = "* Re Password is required !";
                }
                else if (!Password.Equals(RePassword))
                {
                    ok = false;
                    PasswordErrorMessage = "* Password don`t match !";
                }
                else
                {
                    RePasswordErrorMessage = "";
                    PasswordErrorMessage = "";
                }
            }

            if (ok)
            {
                if(!String.IsNullOrEmpty(Username))
                {
                    await _apiHelper.UpdateUsername(UserInf.UserId.ToString(),Username);
                }

                if (!String.IsNullOrEmpty(Email))
                {
                    await _apiHelper.UpdateEmail(UserInf.UserId.ToString(), Email);
                }

                if(!String.IsNullOrEmpty(Password))
                {
                    await _apiHelper.UpdatePassword(UserInf.UserId.ToString(), Password);
                }

                BackButton();
            }
        }

        #endregion
    }
}

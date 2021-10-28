using AgriculturalAplication.EventModels;
using AgriculturalAplication.Helpers;
using AgriculturalAplication.TemporaryUserData;
using Caliburn.Micro;
using System;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AgriculturalAplication.ViewModels
{
    public class LoginPageViewModel : Screen
    {
        #region Properties

        public string Username { get; set; }
        public static string Password { get; set; }

        public bool IsErrorVisible
        {
            get
            {
                bool output = false;

                if (ErrorMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        private BitmapImage _connectionImage;

        public BitmapImage ConnectionImage
        {
            get { return _connectionImage; }
            set
            {
                _connectionImage = value;
                NotifyOfPropertyChange(() => ConnectionImage);
            }
        }

        private string _connectionText;

        public string ConnectionText
        {
            get { return _connectionText; }
            set
            {
                _connectionText = value;
                NotifyOfPropertyChange(() => ConnectionText);
            }
        }

        #endregion

        #region CaliburnMicroFunctions

        private IAPIHelper _apiHelper;
        private IEventAggregator _events;
        private bool IsServerOnline;
        private DispatcherTimer dispatcherTimer;
        private string ConnString = ConfigurationManager.AppSettings["ServerAddress"];

        public LoginPageViewModel(IEventAggregator events,IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        protected override void OnActivate()
        {
            Username = "";
            Password = "";
            ErrorMessage = "";

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
        }


        protected override void OnDeactivate(bool close)
        {
            dispatcherTimer.Stop();
        }
        #endregion

        #region Buttons

        public async Task Login()
        {
            if(IsServerOnline)
            {
                bool ok = true;

                if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
                {
                    ErrorMessage = "* All fields must be filled !";
                    ok = false;
                }
                else ErrorMessage = "";

                if (ok)
                {
                    var result = await _apiHelper.Authenticate(Username, Password);
                    if (result)
                    {
                        //salvez userid
                        UserInf.UserId = await _apiHelper.GetUserId(Username, Password);
                        ErrorMessage = "";
                        _events.PublishOnUIThread(new LogOnEventModel());
                        TryClose();
                    }
                    else ErrorMessage = "* Username or Password is wrong !";
                }
            }
            else ErrorMessage = "* Server is offline !";
        }

        public void SignUp()
        {
            if (IsServerOnline)
            {
                _events.PublishOnUIThread(new SignUpEventModel());
                TryClose();
            }
            else ErrorMessage = "* Server is offline !";
        }

        #endregion

        #region HelperFunctions

        private bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (PingHost(ConnString))
            {
                ConnectionImage = new BitmapImage(new Uri(@"pack://application:,,,/Resources/Other/CheckSign.png"));
                ConnectionText = "Server ONLINE";
                IsServerOnline = true;
            }
            else
            {
                ConnectionImage = new BitmapImage(new Uri(@"pack://application:,,,/Resources/Other/WrongSign.png"));
                ConnectionText = "Server OFFLINE";
                IsServerOnline = false;
            }
        }

        #endregion
    }
}

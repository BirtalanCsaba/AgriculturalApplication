using AgriculturalAplication.Helpers;
using AgriculturalAplication.Models;
using AgriculturalAplication.TemporaryUserData;
using Caliburn.Micro;
using System.Threading;
using System.Windows;
using static AgriculturalAplication.Models.MainFormModel;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Threading;
using System;

namespace AgriculturalAplication.ViewModels
{
    public class SensorsFormViewModel : Screen
    {
        #region Properties

        public int TemperatureProgress { get; set; }

        public int HumidityProgress { get; set; }

        public int LuminosityProgress { get; set; }

        #region SensorValues

        private int _tempMinValue;

        public int TempMinValue
        {
            get { return _tempMinValue; }
            set
            {
                _tempMinValue = value;
                NotifyOfPropertyChange(() => TempMinValue);
            }
        }

        private int _tempMaxValue;

        public int TempMaxValue
        {
            get { return _tempMaxValue; }
            set
            {
                _tempMaxValue = value;
                NotifyOfPropertyChange(() => TempMaxValue);
            }
        }

        private int _humMinValue;

        public int HumMinValue
        {
            get { return _humMinValue; }
            set
            {
                _humMinValue = value;
                NotifyOfPropertyChange(() => HumMinValue);
            }
        }

        private int _humMaxValue;

        public int HumMaxValue
        {
            get { return _humMaxValue; }
            set
            {
                _humMaxValue = value;
                NotifyOfPropertyChange(() => HumMaxValue);
            }
        }

        private int _lumMinValue;

        public int LumMinValue
        {
            get { return _lumMinValue; }
            set
            {
                _lumMinValue = value;
                NotifyOfPropertyChange(() => LumMinValue);
            }
        }

        private int _lumMaxValue;

        public int LumMaxValue
        {
            get { return _lumMaxValue; }
            set
            {
                _lumMaxValue = value;
                NotifyOfPropertyChange(() => LumMaxValue);
            }
        }

        #endregion

        #region TempColorGradients

        private Color _tempColorGrad1;

        public Color TempColorGrad1
        {
            get { return _tempColorGrad1; }
            set
            {
                _tempColorGrad1 = value;
                NotifyOfPropertyChange(() => TempColorGrad1);
            }
        }

        private Color _tempColorGrad2;

        public Color TempColorGrad2
        {
            get { return _tempColorGrad2; }
            set
            {
                _tempColorGrad2 = value;
                NotifyOfPropertyChange(() => TempColorGrad2);
            }
        }

        private Color _tempColorGrad3;

        public Color TempColorGrad3
        {
            get { return _tempColorGrad3; }
            set
            {
                _tempColorGrad3 = value;
                NotifyOfPropertyChange(() => TempColorGrad3);
            }
        }

        #endregion

        #region HumColorGradients

        private Color _humColorGrad1;

        public Color HumColorGrad1
        {
            get { return _humColorGrad1; }
            set
            {
                _humColorGrad1 = value;
                NotifyOfPropertyChange(() => HumColorGrad1);
            }
        }

        private Color _humColorGrad2;

        public Color HumColorGrad2
        {
            get { return _humColorGrad2; }
            set
            {
                _humColorGrad2 = value;
                NotifyOfPropertyChange(() => HumColorGrad2);
            }
        }

        private Color _humColorGrad3;

        public Color HumColorGrad3
        {
            get { return _humColorGrad3; }
            set
            {
                _humColorGrad3 = value;
                NotifyOfPropertyChange(() => HumColorGrad3);
            }
        }

        #endregion

        #region LumColorGradients

        private Color _lumColorGrad1;

        public Color LumColorGrad1
        {
            get { return _lumColorGrad1; }
            set
            {
                _lumColorGrad1 = value;
                NotifyOfPropertyChange(() => LumColorGrad1);
            }
        }

        private Color _lumColorGrad2;

        public Color LumColorGrad2
        {
            get { return _lumColorGrad2; }
            set
            {
                _lumColorGrad2 = value;
                NotifyOfPropertyChange(() => LumColorGrad2);
            }
        }

        private Color _lumColorGrad3;

        public Color LumColorGrad3
        {
            get { return _lumColorGrad3; }
            set
            {
                _lumColorGrad3 = value;
                NotifyOfPropertyChange(() => LumColorGrad3);
            }
        }

        #endregion

        #region DangerText

        private string _tempDangerText;

        public string TempDangerText
        {
            get { return _tempDangerText; }
            set
            {
                _tempDangerText = value;
                NotifyOfPropertyChange(() => TempDangerText);
            }
        }

        private string _humDangerText;

        public string HumDangerText
        {
            get { return _humDangerText; }
            set
            {
                _humDangerText = value;
                NotifyOfPropertyChange(() => HumDangerText);
            }
        }

        private string _lumDangerText;

        public string LumDangerText
        {
            get { return _lumDangerText; }
            set
            {
                _lumDangerText = value;
                NotifyOfPropertyChange(() => LumDangerText);
            }
        }

        #endregion

        public bool IsTimerAlive { get; set; }

        #endregion

        #region CaliburnMicroFunctions
        private IAPIHelper _apiHelper;
        private Project Proj;
        private UserSettingsModel Settings;
        private FlowersDataModel _plant;
        public static DispatcherTimer dispatcherTimer;

        public SensorsFormViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        protected override async void OnActivate()
        {
            Proj = UserInf.ActiveProject;
            Settings = await _apiHelper.GetSettings(UserInf.UserId.ToString());
            TempMinValue = Settings.TempMinValue;
            TempMaxValue = Settings.TempMaxValue;
            HumMinValue = Settings.HumMinValue;
            HumMaxValue = Settings.HumMaxValue;
            LumMinValue = Settings.LumMinValue;
            LumMaxValue = Settings.LumMaxValue;

            //get sensor values

            IsTimerAlive = true;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
        }

        protected override void OnDeactivate(bool close)
        {
            dispatcherTimer.Stop();
        }
        #endregion

        #region HelperFunctions

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (IsTimerAlive)
            {
                TemperatureProgress = await _apiHelper.GetTemperatureById(Proj.ProductId);
                HumidityProgress = await _apiHelper.GetHumidityById(Proj.ProductId);
                LuminosityProgress = await _apiHelper.GetLuminosityById(Proj.ProductId);

                KeyValuePair<int, int> selected = await _apiHelper.GetSelectedPlant(UserInf.ActiveProject.ProductId.ToString());

                _plant = await _apiHelper.GetPlantValuesById(selected.Value, selected.Key);

                if (TemperatureProgress < _plant.Temperature - 5 || TemperatureProgress > _plant.Temperature + 5)
                {
                    TempMakeDanger();
                    TempDangerText = "! Warning";
                }
                else
                {
                    TempColorGrad1 = Color.FromRgb(153, 209, 123);
                    TempColorGrad2 = Color.FromRgb(135, 181, 105);
                    TempColorGrad3 = Color.FromRgb(89, 175, 112);
                    TempDangerText = "";
                }

                if (HumidityProgress < _plant.Humidity - 5 || HumidityProgress > _plant.Humidity + 5)
                {
                    HumMakeDanger();
                    HumDangerText = "! Warning";
                }
                else
                {
                    HumColorGrad1 = Color.FromRgb(124, 154, 218);
                    HumColorGrad2 = Color.FromRgb(95, 136, 176);
                    HumColorGrad3 = Color.FromRgb(86, 102, 170);
                    HumDangerText = "";
                }

                if (LuminosityProgress < _plant.Luminosity - 5 || LuminosityProgress > _plant.Luminosity + 5)
                {
                    LumMakeDanger();
                    LumDangerText = "! Warning";
                }
                else
                {
                    LumColorGrad1 = Color.FromRgb(218, 218, 124);
                    LumColorGrad2 = Color.FromRgb(176, 172, 95);
                    LumColorGrad3 = Color.FromRgb(162, 170, 86);
                    LumDangerText = "";
                }

                NotifyOfPropertyChange(() => TemperatureProgress);
                NotifyOfPropertyChange(() => HumidityProgress);
                NotifyOfPropertyChange(() => LuminosityProgress);
            }
            else
            {
                dispatcherTimer.Stop();
                IsTimerAlive = true;
            }
        }

        private void TempMakeDanger()
        {
            TempColorGrad1 = Color.FromRgb(218, 128, 124);
            TempColorGrad2 = Color.FromRgb(176, 95, 95);
            TempColorGrad3 = Color.FromRgb(170, 86, 86);
        }

        private void HumMakeDanger()
        {
            HumColorGrad1 = Color.FromRgb(218, 128, 124);
            HumColorGrad2 = Color.FromRgb(176, 95, 95);
            HumColorGrad3 = Color.FromRgb(170, 86, 86);
        }

        private void LumMakeDanger()
        {
            LumColorGrad1 = Color.FromRgb(218, 128, 124);
            LumColorGrad2 = Color.FromRgb(176, 95, 95);
            LumColorGrad3 = Color.FromRgb(170, 86, 86);
        }

        #endregion
    }
}

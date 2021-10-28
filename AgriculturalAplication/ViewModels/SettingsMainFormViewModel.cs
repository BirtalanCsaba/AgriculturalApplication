using AgriculturalAplication.EventModels;
using AgriculturalAplication.Helpers;
using AgriculturalAplication.Models;
using AgriculturalAplication.TemporaryUserData;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static AgriculturalAplication.Models.MainFormModel;

namespace AgriculturalAplication.ViewModels
{
    public class SettingsMainFormViewModel : Screen
    {
        #region Properties

        #region Settings

        private int _tempMinValue = -50;

        public int TempMinValue
        {
            get { return _tempMinValue; }
            set
            {
                _tempMinValue = value;
                NotifyOfPropertyChange(() => TempMinValue);
            }
        }

        private int _tempMaxValue = 50;

        public int TempMaxValue
        {
            get { return _tempMaxValue; }
            set
            {
                _tempMaxValue = value;
                NotifyOfPropertyChange(() => TempMaxValue);
            }
        }

        private int _humMinValue = 0;

        public int HumMinValue
        {
            get { return _humMinValue; }
            set
            {
                _humMinValue = value;
                NotifyOfPropertyChange(() => HumMinValue);
            }
        }

        private int _humMaxValue = 100;

        public int HumMaxValue
        {
            get { return _humMaxValue; }
            set
            {
                _humMaxValue = value;
                NotifyOfPropertyChange(() => HumMaxValue);
            }
        }

        private int _lumMinValue = 0;

        public int LumMinValue
        {
            get { return _lumMinValue; }
            set
            {
                _lumMinValue = value;
                NotifyOfPropertyChange(() => LumMinValue);
            }
        }

        private int _lumMaxValue = 200;

        public int LumMaxValue
        {
            get { return _lumMaxValue; }
            set
            {
                _lumMaxValue = value;
                NotifyOfPropertyChange(() => LumMaxValue);
            }
        }

        private BindableCollection<ComboBoxItems> _refreshTime;

        public BindableCollection<ComboBoxItems> RefreshTime
        {
            get { return _refreshTime; }
            set
            {
                _refreshTime = value;
                NotifyOfPropertyChange(() => RefreshTime);
            }
        }

        private ComboBoxItems _cbSelected;

        public ComboBoxItems CBSelected
        {
            get { return _cbSelected; }
            set
            {
                _cbSelected = value;
                NotifyOfPropertyChange(() => CBSelected);
            }
        }

        #endregion

        #region InfoText

        public bool SensorIsInfoVisible
        {
            get
            {
                bool output = false;

                if (SensorInfoMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _sensorInfoMessage;

        public string SensorInfoMessage
        {
            get { return _sensorInfoMessage; }
            set
            {
                _sensorInfoMessage = value;
                NotifyOfPropertyChange(() => SensorIsInfoVisible);
                NotifyOfPropertyChange(() => SensorInfoMessage);
            }
        }

        private Color _sensorInfoTextColor;

        public Color SensorInfoTextColor
        {
            get { return _sensorInfoTextColor; }
            set
            {
                _sensorInfoTextColor = value;
                NotifyOfPropertyChange(() => SensorInfoTextColor);
            }
        }

        public bool ChartIsInfoVisible
        {
            get
            {
                bool output = false;

                if (ChartInfoMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _chartInfoMessage;

        public string ChartInfoMessage
        {
            get { return _chartInfoMessage; }
            set
            {
                _chartInfoMessage = value;
                NotifyOfPropertyChange(() => ChartIsInfoVisible);
                NotifyOfPropertyChange(() => ChartInfoMessage);
            }
        }

        private Color _chartInfoTextColor;

        public Color ChartInfoTextColor
        {
            get { return _chartInfoTextColor; }
            set
            {
                _chartInfoTextColor = value;
                NotifyOfPropertyChange(() => ChartInfoTextColor);
            }
        }

        public bool PlantIsInfoVisible
        {
            get
            {
                bool output = false;

                if (PlantInfoMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _plantInfoMessage;

        public string PlantInfoMessage
        {
            get { return _plantInfoMessage; }
            set
            {
                _plantInfoMessage = value;
                NotifyOfPropertyChange(() => PlantIsInfoVisible);
                NotifyOfPropertyChange(() => PlantInfoMessage);
            }
        }

        #endregion

        #endregion

        #region CaliburnMicroFunctions
        private UserSettingsModel Settings;
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;

        public SettingsMainFormViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        protected override async void OnActivate()
        {
            SensorInfoMessage = "";
            ChartInfoMessage = "";
            PlantInfoMessage = "";

            Settings = await _apiHelper.GetSettings(UserInf.UserId.ToString());
            TempMinValue = Settings.TempMinValue;
            TempMaxValue = Settings.TempMaxValue;
            HumMinValue = Settings.HumMinValue;
            HumMaxValue = Settings.HumMaxValue;
            LumMinValue = Settings.LumMinValue;
            LumMaxValue = Settings.LumMaxValue;

            //combobox
            CreateComboBoxItems();
        }

        #endregion

        #region Buttons

        public async Task SaveSensorChanges()
        {
            if (TempMinValue < TempMaxValue && HumMinValue < HumMaxValue && LumMinValue < LumMaxValue)
            {
                await _apiHelper.SaveSensorSettingsChanges(UserInf.UserId.ToString(), UserInf.ActiveProject.ProductId,
                TempMinValue, TempMaxValue, HumMinValue, HumMaxValue, LumMinValue, LumMaxValue);
                SensorInfoTextColor = Color.FromRgb(106, 185, 69);
                SensorInfoMessage = "* Settings saved !";
            }
            else
            {
                SensorInfoTextColor = Color.FromRgb(216, 56, 56);
                SensorInfoMessage = "* The first value can`t be greater than the second one !";
            }
        }

        public async Task SaveChartChanges()
        {
            if (CBSelected != null)
            {
                await _apiHelper.ChangeDelayTime(UserInf.ActiveProject.ProductId, CBSelected.Value);
                ChartInfoTextColor = Color.FromRgb(106, 185, 69);
                ChartInfoMessage = "* Settings saved !";
            }
            else
            {
                ChartInfoTextColor = Color.FromRgb(216, 56, 56);
                ChartInfoMessage = "* No item selected !";
            }
        }

        public async Task DefaultPlant()
        {
            await _apiHelper.SendPlant(UserInf.ActiveProject.ProductId, 0, 0);
            PlantInfoMessage = "* Settings saved !";
        }

        public void ChoosePlant()
        {
            _events.PublishOnUIThread(new PlantsFormEventModel());
            TryClose();
        }

        #endregion

        #region HelperFunctions

        private void CreateComboBoxItems()
        {
            RefreshTime = new BindableCollection<ComboBoxItems>
            {
                new ComboBoxItems
                {
                    Name = "10 seconds",
                    Value = 10_000
                },
                new ComboBoxItems
                {
                    Name = "30 seconds",
                    Value = 30_000
                },
                new ComboBoxItems
                {
                    Name = "1 minute",
                    Value = 60_000
                },
                new ComboBoxItems
                {
                    Name = "5 minutes",
                    Value = 300_000
                },
                new ComboBoxItems
                {
                    Name = "30 minutes",
                    Value = 1800000
                },
                new ComboBoxItems
                {
                    Name = "1 hour",
                    Value = 3_600_000
                },
                new ComboBoxItems
                {
                    Name = "6 hours",
                    Value = 21_600_000
                },
                new ComboBoxItems
                {
                    Name = "12 hours",
                    Value = 43_200_000
                },
                new ComboBoxItems
                {
                    Name = "1 day",
                    Value = 86_400_000
                },
            };
        }

        #endregion
    }
}

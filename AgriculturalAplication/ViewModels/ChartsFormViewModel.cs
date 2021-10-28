using AgriculturalAplication.Helpers;
using AgriculturalAplication.TemporaryUserData;
using Caliburn.Micro;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using static AgriculturalAplication.Models.MainFormModel;
using static AgriculturalAplication.Models.ProductChartModel;

namespace AgriculturalAplication.ViewModels
{
    public class ChartsFormViewModel : Screen
    {
        #region Properties

        #region Temperature

        private List<TemperatureModel> _temperatureList;

        public List<TemperatureModel> TemperatureList
        {
            get { return _temperatureList; }
            set
            {
                _temperatureList = value;
                NotifyOfPropertyChange(() => TemperatureList);
            }
        }

        public List<int> TemperatureValue { get; set; }

        public List<DateTime> TemperatureTime { get; set; }

        #endregion

        #region Humidity

        private List<HumidityModel> _humidityList;

        public List<HumidityModel> HumidityList
        {
            get { return _humidityList; }
            set
            {
                _humidityList = value;
                NotifyOfPropertyChange(() => HumidityList);
            }
        }

        public List<int> HumidityValue { get; set; }

        public List<DateTime> HumidityTime { get; set; }

        #endregion

        #region Luminosity

        private List<LuminosityModel> _luminosityList;

        public List<LuminosityModel> LuminosityList
        {
            get { return _luminosityList; }
            set
            {
                _luminosityList = value;
                Application.Current.Dispatcher.Invoke(delegate
                {
                    NotifyOfPropertyChange(() => LuminosityList);
                });

            }
        }

        public List<int> LuminosityValue { get; set; }

        public List<DateTime> LuminosityTime { get; set; }

        #endregion

        #region SoilHumidity

        private List<SoilHumidityModel> _soilHumList;

        public List<SoilHumidityModel> SoilHumList
        {
            get { return _soilHumList; }
            set
            {
                _soilHumList = value;
                NotifyOfPropertyChange(() => SoilHumList);
            }
        }

        public List<int> SoilHumidityValue { get; set; }

        public List<DateTime> SoilHumidityTime { get; set; }

        #endregion

        #region Graph

        private SeriesCollection _series;

        public SeriesCollection Series
        {
            get { return _series; }
            set
            {
                _series = value;
                NotifyOfPropertyChange(() => Series);
            }
        }

        private String[] _labels;

        public String[] Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                NotifyOfPropertyChange(() => Labels);
            }
        }

        #endregion

        #region ButtonSwitch

        private bool IsTemperatureGraph;

        private bool IsHumidityGraph;

        private bool IsLuminosityGraph;

        private bool IsSoilHumidityGraph;

        #endregion

        public bool IsTimerAlive { get; set; }
        #endregion

        #region CaliburnMicroFunctions

        private IAPIHelper _apiHelper;
        private Project Proj;
        public static DispatcherTimer dispatcherTimer;

        public ChartsFormViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        protected override void OnActivate()
        {
            Proj = UserInf.ActiveProject;

            IsTimerAlive = true;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            IsTemperatureGraph = true;
            dispatcherTimer.Start();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);

        }

        protected override void OnDeactivate(bool close)
        {
            dispatcherTimer.Stop();
        }

        #endregion

        #region Buttons

        public void TemperatureButton()
        {
            TemperatureList = null;
            MakeFalseGraphs();
            IsTemperatureGraph = true;
        }

        public void HumidityButton()
        {
            HumidityList = null;
            MakeFalseGraphs();
            IsHumidityGraph = true;
        }

        public void LuminosityButton()
        {
            LuminosityList = null;
            MakeFalseGraphs();
            IsLuminosityGraph = true;
        }

        public void SoilHumidityButton()
        {
            SoilHumList = null;
            MakeFalseGraphs();
            IsSoilHumidityGraph = true;
        }

        #endregion

        #region HelperFunctions

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if(IsTimerAlive)
            {
                if (IsTemperatureGraph)
                {
                    List<TemperatureModel> aux = TemperatureList;

                    TemperatureList = await _apiHelper.GetTemperatureList(Proj.ProductId.ToString());

                    if (aux != null)
                    {
                        if (aux.Count < TemperatureList.Count)
                        {
                            CreateTemperatureGraph();
                        }
                    }
                    else
                    {
                        CreateTemperatureGraph();
                    }
                }
                else if (IsHumidityGraph)
                {
                    List<HumidityModel> aux = HumidityList;

                    HumidityList = await _apiHelper.GetHumidityList(Proj.ProductId.ToString());

                    if (aux != null)
                    {
                        if (aux.Count < HumidityList.Count)
                        {
                            CreateHumidityGraph();
                        }
                    }
                    else
                    {
                        CreateHumidityGraph();
                    }
                }
                else if (IsLuminosityGraph)
                {
                    List<LuminosityModel> aux = LuminosityList;

                    LuminosityList = await _apiHelper.GetLuminosityList(Proj.ProductId.ToString());

                    if (aux != null)
                    {
                        if (aux.Count < LuminosityList.Count)
                        {
                            CreateLuminosityGraph();
                        }
                    }
                    else
                    {
                        CreateLuminosityGraph();
                    }
                }
                else if (IsSoilHumidityGraph)
                {
                    List<SoilHumidityModel> aux = SoilHumList;

                    SoilHumList = await _apiHelper.GetSoilHumidityList(Proj.ProductId.ToString());

                    if (aux != null)
                    {
                        if (aux.Count < SoilHumList.Count)
                        {
                            CreateSoilHumidityGraph();
                        }
                    }
                    else
                    {
                        CreateSoilHumidityGraph();
                    }
                }
            }
            else
            {
                dispatcherTimer.Stop();
                IsTimerAlive = true;
            }
            
            
        }

        #region CreateGraphs

        private void CreateTemperatureGraph()
        {
            TemperatureValue = TemperatureList.Select(a => a.Temperature).ToList();
            TemperatureTime = TemperatureList.Select(a => a.DateTimePost).ToList();

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = TemperatureValue.AsChartValues()
                }
            };

            Labels = TemperatureTime.Select(a => a.ToString()).ToArray();
        }

        private void CreateHumidityGraph()
        {
            HumidityValue = HumidityList.Select(a => a.Humidity).ToList();
            HumidityTime = HumidityList.Select(a => a.DateTimePost).ToList();

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = HumidityValue.AsChartValues()
                },
            };

            Labels = HumidityTime.Select(a => a.ToString()).ToArray();
        }

        private void CreateLuminosityGraph()
        {
            LuminosityValue = LuminosityList.Select(a => a.Luminosity).ToList();
            LuminosityTime = LuminosityList.Select(a => a.DateTimePost).ToList();

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = LuminosityValue.AsChartValues()
                },
            };

            Labels = LuminosityTime.Select(a => a.ToString()).ToArray();
        }

        private void CreateSoilHumidityGraph()
        {
            SoilHumidityValue = SoilHumList.Select(a => a.SoilHumidity).ToList();
            SoilHumidityTime = SoilHumList.Select(a => a.DateTimePost).ToList();

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = SoilHumidityValue.AsChartValues()
                },
            };

            Labels = SoilHumidityTime.Select(a => a.ToString()).ToArray();
        }

        #endregion

        private void MakeFalseGraphs()
        {
            IsTemperatureGraph = IsHumidityGraph = IsLuminosityGraph = IsSoilHumidityGraph = false;
        }

        #endregion
    }
}

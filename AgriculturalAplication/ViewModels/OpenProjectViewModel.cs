using AgriculturalAplication.EventModels;
using AgriculturalAplication.Helpers;
using AgriculturalAplication.Models;
using AgriculturalAplication.TemporaryUserData;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static AgriculturalAplication.Models.MainFormModel;

namespace AgriculturalAplication.ViewModels
{
    public class OpenProjectViewModel : Conductor<object>
    {
        #region Properties
        private string _projectName;

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                NotifyOfPropertyChange(() => ProjectName);
            }
        }

        private string _projectDescription;

        public string ProjectDescription
        {
            get { return _projectDescription; }
            set
            {
                _projectDescription = value;
                NotifyOfPropertyChange(() => ProjectDescription);
            }
        }

        private string _productDescription;

        public string ProductDescription
        {
            get { return _productDescription; }
            set
            {
                _productDescription = value;
                NotifyOfPropertyChange(() => ProductDescription);
            }
        }

        private BitmapImage _waterPlant;

        public BitmapImage WaterPlant
        {
            get { return _waterPlant; }
            set
            {
                _waterPlant = value;
                NotifyOfPropertyChange(() => WaterPlant);
            }
        }

        private string _waterPlantText;

        public string WaterPlantText
        {
            get { return _waterPlantText; }
            set
            {
                _waterPlantText = value;
                NotifyOfPropertyChange(() => WaterPlantText);
            }
        }

        private string _selectedPlant;

        public string SelectedPlant
        {
            get { return _selectedPlant; }
            set
            {
                _selectedPlant = value;
                NotifyOfPropertyChange(() => SelectedPlant);
            }
        }


        #endregion

        #region CaliburnMicroFunctions
        private IAPIHelper _apiHelper;
        private IEventAggregator _events;
        private Project Proj;
        DispatcherTimer dispatcherTimer;

        private SensorsFormViewModel _sensorVM;
        private ChartsFormViewModel _chartVM;
        private SettingsFormViewModel _settVM;
        private SimpleContainer _container;

        public OpenProjectViewModel(IAPIHelper apiHelper, IEventAggregator events, SimpleContainer container,
            SensorsFormViewModel SensorVM, ChartsFormViewModel ChartVM, SettingsFormViewModel SettVM)
        {
            _apiHelper = apiHelper;
            _events = events;
            _container = container;

            _sensorVM = SensorVM;
            _chartVM = ChartVM;
            _settVM = SettVM;
        }

        protected override async void OnActivate()
        {
            Proj = UserInf.ActiveProject;
            ProjectName = Proj.Name;
            ProjectDescription = Proj.Description;

            _sensorVM.IsTimerAlive = true;
            ActivateItem(_sensorVM);

            ProductDescription = await _apiHelper.GetProductDescriptionById(Proj.ProductId);
            SelectedPlant = await _apiHelper.GetSelectedPlantName(Proj.ProductId);

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

        #region Buttons

        public void Sensors()
        {
            ActivateItem(_sensorVM);
        }

        public void Settings()
        {
            ActivateItem(_settVM);
        }

        public void Charts()
        {
            _chartVM.IsTimerAlive = true;
            ActivateItem(_chartVM);
        }

        public void BackButton()
        {
            dispatcherTimer.Stop();
            _sensorVM.IsTimerAlive = false;
            _chartVM.IsTimerAlive = false;
            _events.PublishOnUIThread(new BackToMainFormEventModel());
            TryClose();
        }

        #endregion

        #region HelperFunctions

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            int isWater = await _apiHelper.GetSoilHumById(Proj.ProductId);

            if (isWater == 1)
            {
                WaterPlant = new BitmapImage(new Uri(@"pack://application:,,,/Resources/Other/CheckGreen.png", UriKind.Absolute));
                WaterPlantText = "Your plant is watered !";
            }
            else
            {
                WaterPlant = new BitmapImage(new Uri(@"pack://application:,,,/Resources/Other/PlantWatering.png", UriKind.Absolute));
                WaterPlantText = "Water your plant !";
            }
        }
        #endregion
    }
}

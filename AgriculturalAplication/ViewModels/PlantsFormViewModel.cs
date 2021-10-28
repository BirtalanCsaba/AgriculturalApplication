using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using AgriculturalAplication.EventModels;
using AgriculturalAplication.Helpers;
using AgriculturalAplication.Models;
using AgriculturalAplication.TemporaryUserData;
using Caliburn.Micro;

namespace AgriculturalAplication.ViewModels
{
    public class PlantsFormViewModel : Screen
    {
        #region Properties

        private BindableCollection<FlowersDataModel> _flowers;

        public BindableCollection<FlowersDataModel> Flowers
        {
            get { return _flowers; }
            set
            {
                _flowers = value;
                NotifyOfPropertyChange(() => Flowers);
            }
        }

        private BindableCollection<VegetablesDataModel> _vegetables;

        public BindableCollection<VegetablesDataModel> Vegetables
        {
            get { return _vegetables; }
            set
            {
                _vegetables = value;
                NotifyOfPropertyChange(() => Vegetables);
            }
        }

        private string _applySettingsButtonText;

        public string ApplySettingsButtonText
        {
            get { return _applySettingsButtonText; }
            set
            {
                _applySettingsButtonText = value;
                NotifyOfPropertyChange(() => ApplySettingsButtonText);
            }
        }

        private Color _applySettingsButtonColor;

        public Color ApplySettingsButtonColor
        {
            get { return _applySettingsButtonColor; }
            set
            {
                _applySettingsButtonColor = value;
                NotifyOfPropertyChange(() => ApplySettingsButtonColor);
            }
        }



        public FlowersDataModel _flower { get; set; }
        public VegetablesDataModel _vegetable { get; set; }

        bool IsVegetable, IsFlower;

        #endregion

        #region CaliburnMicroFunctions

        IAPIHelper _apiHelper;
        IEventAggregator _events;

        public PlantsFormViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        protected override async void OnInitialize()
        {
            //Vegetables
            List<VegetablesDataModel> _vegetables = await _apiHelper.GetVegetables();
            Vegetables = new BindableCollection<VegetablesDataModel>();
            foreach(VegetablesDataModel v in _vegetables)
            {
                Vegetables.Add(v);
            }

            //Flowers
            List<FlowersDataModel> _flowers = await _apiHelper.GetFlowers();
            Flowers = new BindableCollection<FlowersDataModel>();
            foreach(FlowersDataModel f in _flowers)
            {
                Flowers.Add(f);
            }
        }

        protected override void OnActivate()
        {
            ApplySettingsButtonColor = Color.FromRgb(184, 104, 104);
            ApplySettingsButtonText = "ApplySettings";
        }

        #endregion

        #region Buttons

        public void BackButton()
        {
            _events.PublishOnUIThread(new BackToMainSettingsEventModel());
            TryClose();
        }

        public void ItemSelectVegetables(dynamic eventArgs)
        {
            ApplySettingsButtonColor = Color.FromRgb(184, 104, 104);
            ApplySettingsButtonText = "ApplySettings";
            _vegetable = eventArgs.AddedItems[0];
            IsVegetable = true;
            IsFlower = false;
        }

        public void ItemSelectFlowers(dynamic eventArgs)
        {
            ApplySettingsButtonColor = Color.FromRgb(184, 104, 104);
            ApplySettingsButtonText = "ApplySettings";
            _flower = eventArgs.AddedItems[0];
            IsVegetable = false;
            IsFlower = true;
        }

        public async Task ApplySettings()
        {
            if (IsFlower != false || IsVegetable != false)
            {
                if (IsFlower)
                {
                    await _apiHelper.SendPlant(UserInf.ActiveProject.ProductId, 2, _flower.Id);
                }
                else if (IsVegetable)
                {
                    await _apiHelper.SendPlant(UserInf.ActiveProject.ProductId, 1, _vegetable.Id);
                }
                ApplySettingsButtonColor = Color.FromRgb(115, 203, 65);
                ApplySettingsButtonText = "Settings Applied";
                _events.PublishOnUIThread(new UpdateSelectedPlantEventModel());
            }
            else
            {
                ApplySettingsButtonColor = Color.FromRgb(184, 104, 104);
                ApplySettingsButtonText = "No Item Selected";
            }

        }

        #endregion
    }
}

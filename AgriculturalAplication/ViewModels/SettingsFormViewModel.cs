using AgriculturalAplication.EventModels;
using Caliburn.Micro;

namespace AgriculturalAplication.ViewModels
{
    public class SettingsFormViewModel : Conductor<object>, IHandle<BackToMainSettingsEventModel>, IHandle<PlantsFormEventModel>
    {
        private SettingsMainFormViewModel _mainSettVM;
        private PlantsFormViewModel _plantVM;

        private IEventAggregator _events;
        private SimpleContainer _container;

        public SettingsFormViewModel(IEventAggregator events, SimpleContainer container,
            SettingsMainFormViewModel MainSettVM, PlantsFormViewModel PlantVm)
        {
            _events = events;
            _events.Subscribe(this);

            _mainSettVM = MainSettVM;
            _plantVM = PlantVm;

            _container = container;
        }

        protected override void OnActivate()
        {
            ActivateItem(_mainSettVM);
        }

        public void Handle(BackToMainSettingsEventModel message)
        {
            ActivateItem(_mainSettVM);
        }

        public void Handle(PlantsFormEventModel message)
        {
            ActivateItem(_plantVM);
        }
    }
}

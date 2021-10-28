using AgriculturalAplication.EventModels;
using AgriculturalAplication.Helpers;
using AgriculturalAplication.TemporaryUserData;
using Caliburn.Micro;

namespace AgriculturalAplication.ViewModels
{
    public class ShellViewModel : Conductor<object>, 
        IHandle<LogOnEventModel>, IHandle<OpenProjectEventModel>, 
        IHandle<BackToMainFormEventModel>, IHandle<EditProjectEventModel>,
        IHandle<BackToMainFormFromEditEventModel>, IHandle<SignUpEventModel>,
        IHandle<BackToLoginPageEventModel>, IHandle<UpdateSelectedPlantEventModel>,
        IHandle<EditAccountEventModel>
    {
        private LoginPageViewModel _loginVM;
        private MainFormViewModel _mainVM;
        private OpenProjectViewModel _openVM;
        private EditFormViewModel _editVM;
        private SignUpFormViewModel _signupVM;
        private EditAccountViewModel _editAccVM;

        private IAPIHelper _apiHelper;
        private IEventAggregator _events;
        private SimpleContainer _container;

        public ShellViewModel(LoginPageViewModel loginVM, MainFormViewModel mainVM,
            OpenProjectViewModel openVM, SimpleContainer container, EditFormViewModel editVM,
            SignUpFormViewModel signupVM,EditAccountViewModel editAccVM, IEventAggregator events, 
            IAPIHelper apiHelper)
        {
            _events = events;
            _events.Subscribe(this);
            _apiHelper = apiHelper;

            _mainVM = mainVM;
            _loginVM = loginVM;
            _openVM = openVM;
            _editVM = editVM;
            _signupVM = signupVM;
            _editAccVM = editAccVM;

            _container = container;

            ActivateItem(_loginVM);
        }

        public void Handle(LogOnEventModel message)
        {
            ActivateItem(_mainVM);
        }

        public void Handle(OpenProjectEventModel message)
        {
            ActivateItem(_openVM);
        }

        public void Handle(BackToMainFormEventModel message)
        {
            ActivateItem(_mainVM);
        }

        public void Handle(EditProjectEventModel message)
        {
            ActivateItem(_editVM);
        }

        public void Handle(BackToMainFormFromEditEventModel message)
        {
            ActivateItem(_mainVM);
        }

        public void Handle(SignUpEventModel message)
        {
            ActivateItem(_signupVM);
        }

        public void Handle(BackToLoginPageEventModel message)
        {
            ActivateItem(_loginVM);
        }

        public async void Handle(UpdateSelectedPlantEventModel message)
        {
            _openVM.SelectedPlant = await _apiHelper.GetSelectedPlantName(UserInf.ActiveProject.ProductId);
        }

        public void Handle(EditAccountEventModel message)
        {
            ActivateItem(_editAccVM);
        }
    }
}

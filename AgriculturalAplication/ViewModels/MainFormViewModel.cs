using AgriculturalAplication.EventModels;
using AgriculturalAplication.Helpers;
using AgriculturalAplication.Models;
using AgriculturalAplication.TemporaryUserData;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using static AgriculturalAplication.Models.MainFormModel;

namespace AgriculturalAplication.ViewModels
{
    public class MainFormViewModel : Screen
    {
        #region Properties

        private string _infBox;

        public string InfBox
        {
            get { return _infBox; }
            set
            {
                _infBox = value;
                NotifyOfPropertyChange(() => InfBox);
            }
        }

        private BindableCollection<Project> _Projects;

        public BindableCollection<Project> Projects
        {
            get { return _Projects; }
            set
            {
                _Projects = value;
                NotifyOfPropertyChange(() => Projects);
            }
        }

        private BitmapImage _projImg;

        public BitmapImage ProjectImage
        {
            get { return _projImg; }
            set
            {
                _projImg = value;
                NotifyOfPropertyChange(() => ProjectImage);
            }
        }

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

        #region ProductKey

        private string _key1;

        public string Key1
        {
            get { return _key1; }
            set
            {
                _key1 = value;
                NotifyOfPropertyChange(() => Key1);
            }
        }

        private string _key2;

        public string Key2
        {
            get { return _key2; }
            set
            {
                _key2 = value;
                NotifyOfPropertyChange(() => Key2);
            }
        }

        private string _key3;

        public string Key3
        {
            get { return _key3; }
            set
            {
                _key3 = value;
                NotifyOfPropertyChange(() => Key3);
            }
        }

        private string _key4;

        public string Key4
        {
            get { return _key4; }
            set
            {
                _key4 = value;
                NotifyOfPropertyChange(() => Key4);
            }
        }

        private string _key5;

        public string Key5
        {
            get { return _key5; }
            set
            {
                _key5 = value;
                NotifyOfPropertyChange(() => Key5);
            }
        }

        private string _key6;

        public string Key6
        {
            get { return _key6; }
            set
            {
                _key6 = value;
                NotifyOfPropertyChange(() => Key6);
            }
        }

        #endregion

        #region ErrorMessages

        public bool ProjNameIsErrorVisible
        {
            get
            {
                bool output = false;

                if (ProjNameErrorMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _projNameErrorMessage;

        public string ProjNameErrorMessage
        {
            get { return _projNameErrorMessage; }
            set
            {
                _projNameErrorMessage = value;
                NotifyOfPropertyChange(() => ProjNameIsErrorVisible);
                NotifyOfPropertyChange(() => ProjNameErrorMessage);
            }
        }

        public bool ProjDescIsErrorVisible
        {
            get
            {
                bool output = false;

                if (ProjDescErrorMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _projDescErrorMessage;

        public string ProjDescErrorMessage
        {
            get { return _projDescErrorMessage; }
            set
            {
                _projDescErrorMessage = value;
                NotifyOfPropertyChange(() => ProjDescIsErrorVisible);
                NotifyOfPropertyChange(() => ProjDescErrorMessage);
            }
        }

        public bool ProdKeyIsErrorVisible
        {
            get
            {
                bool output = false;

                if (ProdKeyErrorMessage.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        private string _prodKeyErrorMessage;

        public string ProdKeyErrorMessage
        {
            get { return _prodKeyErrorMessage; }
            set
            {
                _prodKeyErrorMessage = value;
                NotifyOfPropertyChange(() => ProdKeyIsErrorVisible);
                NotifyOfPropertyChange(() => ProdKeyErrorMessage);
            }
        }

        #endregion

        private string _accountEmail;

        public string AccountEmail
        {
            get { return _accountEmail; }
            set
            {
                _accountEmail = value;
                NotifyOfPropertyChange(() => AccountEmail);
            }
        }


        #endregion

        #region CaliburnFunctions

        private IAPIHelper _apiHelper;
        private IEventAggregator _events;
        private IWindowManager _manager;

        public MainFormViewModel(IEventAggregator events,IAPIHelper apiHelper, IWindowManager manager)
        {
            _apiHelper = apiHelper;
            _events = events;
            _manager = manager;
        }

        protected override async void OnActivate()
        {
            AccountEmail = await _apiHelper.GetUserEmail(UserInf.UserId.ToString());

            List<Project> p = await _apiHelper.GetProjects(UserInf.UserId.ToString());
            Projects = new BindableCollection<Project>();
            foreach (Project proj in p)
            {
                if (proj.Image == null)
                {
                    proj.BMImage = new BitmapImage(new Uri("pack://application:,,,/Resources/Other/plant.jpg"));
                }
                else
                {
                    proj.BMImage = ToImage(proj.Image);
                }
                Projects.Add(proj);
            }
        }

        #endregion

        #region ShowProjectButtons

        public void Click_StartButton(Project p)
        {
            UserInf.ActiveProject = p;

            _events.PublishOnUIThread(new OpenProjectEventModel());
            TryClose();
        }

        public void Click_EditButton(Project p)
        {
            UserInf.ActiveProject = p;

            _events.PublishOnUIThread(new EditProjectEventModel());
            TryClose();
        }

        public async Task Click_DeleteButton(Project p)
        {
            await _apiHelper.DeleteProject(UserInf.UserId.ToString(),p.Name);
            await _apiHelper.DeleteSettings(UserInf.UserId.ToString(), p.ProductId);
            OnActivate();
        }

        #endregion

        #region ButtonHoverShowLabel

        public void MouseEnter_StartButton()
        {
            InfBox = "Open the project.";
        }

        public void MouseEnter_EditButton()
        {
            InfBox = "Edit the project.";
        }

        public void MouseEnter_DeleteButton()
        {
            InfBox = "Delete the project.";
        }

        public void MouseLeave()
        {
            InfBox = String.Empty;
        }

        #endregion

        #region ProjectCreationForm

        public void HandleLinkClick(object sender, RoutedEventArgs e)
        {
            Hyperlink hl = (Hyperlink)sender;
            string navigateUri = hl.NavigateUri.ToString();
            Process.Start(new ProcessStartInfo(navigateUri));
            e.Handled = true;
        }

        public void SelectImageButton()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ProjectImage = new BitmapImage(new Uri(op.FileName));
            }
        }

        public async Task CreateProject()
        {
            bool ok = true;

            if (String.IsNullOrEmpty(ProjectName))
            {
                ok = false;
                ProjNameErrorMessage = "* Project name required !";
            }
            else ProjNameErrorMessage = "";

            if (String.IsNullOrEmpty(ProjectDescription))
            {
                ok = false;
                ProjDescErrorMessage = "* Project description required !";
            }
            else ProjDescErrorMessage = "";

            if (String.IsNullOrEmpty(Key1) || String.IsNullOrEmpty(Key2) || String.IsNullOrEmpty(Key3) ||
                String.IsNullOrEmpty(Key4) || String.IsNullOrEmpty(Key5) || String.IsNullOrEmpty(Key6))
            {
                ok = false;
                ProdKeyErrorMessage = "* All the fields are required !";
            }
            else ProdKeyErrorMessage = "";

            if(ok)
            {
                string ProdKey = Key1 + Key2 + Key3 + Key4 + Key5 + Key6;
                Bitmap img = null;

                if (await _apiHelper.ValidateKey(ProdKey))
                {
                    if(!await _apiHelper.CheckIfKeyIsAvailable(ProdKey))
                    {
                        ProdKeyErrorMessage = "";
                        string ProdId = await _apiHelper.GetProductIdByKey(ProdKey);
                        if (!await _apiHelper.CheckIfProjectExist(ProdId))
                        {
                            if (ProjectImage != null)
                            {
                                img = ResizeImage(BitmapImageToBitmap(ProjectImage), new System.Drawing.Size(50, 80));
                                await _apiHelper.CreateProjectWithImage(UserInf.UserId.ToString(),
                                    ProdId, ProjectName, ProjectDescription, ImageToByte(img));
                                await _apiHelper.CreateSettings(UserInf.UserId.ToString(), ProdId);
                            }
                            else
                            {
                                await _apiHelper.CreateProjectWithoutImage(UserInf.UserId.ToString(),
                                    ProdId, ProjectName, ProjectDescription);
                                await _apiHelper.CreateSettings(UserInf.UserId.ToString(), ProdId);
                            }
                            ClearOutForm();
                            OnActivate();
                        }
                        else MessageBox.Show("Project Already Exists");
                    }
                    else
                    {
                        ProdKeyErrorMessage = "* Product key is already in use !";
                    }
                }
                else
                {
                    ProdKeyErrorMessage = "* Invalid product key !";
                }
            }
        }

        public void ClearOutForm()
        {
            ProjectName = String.Empty;
            ProjectDescription = String.Empty;
            Key1 = Key2 = Key3 = Key4 = Key5 = Key6 = String.Empty;
            ProjectImage = null;
        }

        #endregion

        #region ManuButtons

        public void SignOut()
        {
            _events.PublishOnUIThread(new BackToLoginPageEventModel());
            TryClose();
        }

        public void EditAccount()
        {
            _events.PublishOnUIThread(new EditAccountEventModel());
            TryClose();
        }

        public void Help()
        {
            _manager.ShowDialog(new HelpFormViewModel());
        }

        #endregion

        #region HelperFunctions

        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public Byte[] ImageToByte(Bitmap imageSource)
        {
            MemoryStream stream = new MemoryStream();
            imageSource.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            return stream.ToArray();
        }

        private Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public Bitmap ResizeImage(Bitmap imgToResize, System.Drawing.Size size)
        {
            return new Bitmap(imgToResize, size);
        }

        #endregion
    }
}

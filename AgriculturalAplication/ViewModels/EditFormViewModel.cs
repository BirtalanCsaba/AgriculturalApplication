using AgriculturalAplication.EventModels;
using AgriculturalAplication.Helpers;
using AgriculturalAplication.TemporaryUserData;
using Caliburn.Micro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static AgriculturalAplication.Models.MainFormModel;
using Color = System.Windows.Media.Color;

namespace AgriculturalAplication.ViewModels
{
    public class EditFormViewModel : Screen
    {
        #region Properties

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

        BitmapImage TempImg;

        #endregion

        #region CaliburnMicroFunctions

        private IAPIHelper _apiHelper;
        private IEventAggregator _events;
        private Project Proj;

        public EditFormViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        protected override void OnActivate()
        {
            ProjectName = "";
            ProjectDescription = "";

            ApplySettingsButtonColor = Color.FromRgb(60,119,224);
            ApplySettingsButtonText = "Apply Settings";

            Proj = UserInf.ActiveProject;
            TempImg = new BitmapImage(new Uri(@"pack://application:,,,/Resources/BackgroundImages/EditBackground.jpg", UriKind.Absolute));
            ProjectImage = TempImg;
        }
        #endregion

        #region Buttons

        public void ImageChoose()
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

        public async void Submit()
        {
            if(ProjectImage != TempImg)
            {
                await _apiHelper.EditProject(Proj.ProductId, ProjectName, ProjectDescription,
                Convert.ToBase64String(ImageToByte(ResizeImage(BitmapImageToBitmap(ProjectImage), new Size(50, 80)))));
            }
            else await _apiHelper.EditProject(Proj.ProductId, ProjectName, ProjectDescription, String.Empty);

            ApplySettingsButtonColor = Color.FromRgb(115, 203, 65);
            ApplySettingsButtonText = "Settings Applied";
        }

        public void BackButton()
        {
            _events.PublishOnUIThread(new BackToMainFormEventModel());
            TryClose();
        }

        #endregion

        #region HelperFunctions

        public Byte[] ImageToByte(Bitmap imageSource)
        {
            MemoryStream stream = new MemoryStream();
            imageSource.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            return stream.ToArray();
        }

        public Bitmap ResizeImage(Bitmap imgToResize, System.Drawing.Size size)
        {
            return new Bitmap(imgToResize, size);
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

        #endregion
    }
}

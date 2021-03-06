using AgriculturalAplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AgriculturalAplication.Views
{
    /// <summary>
    /// Interaction logic for SignUpFormView.xaml
    /// </summary>
    public partial class SignUpFormView : UserControl
    {
        public SignUpFormView()
        {
            InitializeComponent();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SignUpFormViewModel.Password = Password.Password;
        }

        private void RePassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SignUpFormViewModel.RePassword = RePassword.Password;
        }
    }
}

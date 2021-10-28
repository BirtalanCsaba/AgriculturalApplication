using AgriculturalAplication.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace AgriculturalAplication.Views
{
    /// <summary>
    /// Interaction logic for EditAccountView.xaml
    /// </summary>
    public partial class EditAccountView : UserControl
    {
        public EditAccountView()
        {
            InitializeComponent();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EditAccountViewModel.Password = Password.Password;
        }

        private void RePassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EditAccountViewModel.RePassword = RePassword.Password;
        }
    }
}

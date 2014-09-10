using FirstFloor.ModernUI.Windows.Controls;
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

namespace ModernUINavigationApp1.Pages
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : UserControl
    {
        Admin admin;
        public SignIn()
        {
            InitializeComponent();
        }

        private bool signIn(String login, String password)
        {
            admin = new Admin(login, password);
            if (!admin.authorization())
            {
                ModernDialog.ShowMessage("Wrong login or password!", "Error", MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        private void _signInBtn()
        {
            SignInBtn.Visibility = System.Windows.Visibility.Hidden;
            if (signIn(loginBox.Text, passwordBox.Password))
            {
                MainWindow main = new MainWindow(admin);
                Visibility = System.Windows.Visibility.Collapsed;
                Application.Current.MainWindow.Visibility = System.Windows.Visibility.Collapsed;
                main.Show();

            }
            else
            {
                SignInBtn.Visibility = System.Windows.Visibility.Visible;
                passwordBox.Password = "";
            }
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _signInBtn();
                e.Handled = true;
            }
        }

        private void loginBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _signInBtn();
                e.Handled = true;
            }
        }

        private void SignInBtn_Click_1(object sender, RoutedEventArgs e)
        {
            _signInBtn();
        }
    }
}

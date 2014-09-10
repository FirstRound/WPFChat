using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : UserControl
    {
        Admin admin;
        public SignUp()
        {
            InitializeComponent();
        }

        private bool signUp(String login, String email, String password)
        {
            if (!isValidEmail(email))
            {
                ModernDialog.ShowMessage("Wrong e-mail!", "Error", MessageBoxButton.OK);
                return false;
            }
            admin = new Admin(login, password);
            if (!admin.registration(email))
            {
                ModernDialog.ShowMessage("Wrong login or password!", "Error", MessageBoxButton.OK);
                return false;
            }
            return true;
        }

        private void _signUpBtn()
        {
            SignUpBtn.Visibility = System.Windows.Visibility.Hidden;
            if (signUp(loginBox.Text, emailBox.Text, passwordBox.Password))
            {
                MainWindow main = new MainWindow(admin);
                Visibility = System.Windows.Visibility.Collapsed;
                Application.Current.MainWindow.Visibility = System.Windows.Visibility.Collapsed;
                main.Show();

            }
            else
            {
                SignUpBtn.Visibility = System.Windows.Visibility.Visible;
                passwordBox.Password = "";
            }
        }

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            _signUpBtn();
        }

        private void passwordBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _signUpBtn();
                e.Handled = true;
            }
        }

        private void emailBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _signUpBtn();
                e.Handled = true;
            }
        }

        private void loginBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _signUpBtn();
                e.Handled = true;
            }
        }
        public static bool isValidEmail(String email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }
    }
}

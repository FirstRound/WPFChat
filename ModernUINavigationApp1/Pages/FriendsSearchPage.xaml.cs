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
    /// Interaction logic for FriendsManagerPage.xaml
    /// </summary>
    public partial class FriendsSearchPage : UserControl
    {
        FriendsViewList friends_list_view = new FriendsViewList();
        ActionController _chat = new ActionController(MainWindow.admin);
        public FriendsSearchPage()
        {
            InitializeComponent();
            FriendMetroView.DataContext = friends_list_view;
            hideResultText();
            hideUserInfo();
            userPic.Source = (new ImageSourceConverter()).ConvertFromString("pack://application:,,,/Resources/UserPic.png") as ImageSource;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            searchBntClick();
        }

        private void searchBntClick()
        {
            okMsgLabel.Visibility = System.Windows.Visibility.Hidden;
            errorMsgLabel.Visibility = System.Windows.Visibility.Hidden;
            friends_list_view.Clear();
            friends_list_view.AddRange(new ActionController(MainWindow.admin).findUserByName(userNameBox.Text));
            FriendMetroView.DataContext = friends_list_view;
            showResultText();
            if (FriendMetroView.SelectedIndex > -1)
            {
                showUserInfo();
            }
            else
            {
                hideUserInfo();
            }
        }

        private void addFriendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FriendMetroView.SelectedIndex > -1)
            {
                User friend = friends_list_view.getUser(FriendMetroView.SelectedIndex);
                if (_chat.addFriend(friend))
                {
                    okMsgLabel.Content = "OK";
                    friends_list_view.deleteUser(FriendMetroView.SelectedIndex);
                    hideUserInfo();
                    showResultText();
                }
                else
                {
                    errorMsgLabel.Content = "Error!";
                    showUserInfo();
                }
            }
        }

        private void FriendMetroView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            okMsgLabel.Visibility = System.Windows.Visibility.Hidden;
            errorMsgLabel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void showUserInfo()
        {
            //messageLabel.Visibility = System.Windows.Visibility.Visible;
            addFriendBtn.Visibility = System.Windows.Visibility.Visible;
            //messageToFriendBox.Visibility = System.Windows.Visibility.Visible;
            userPic.Visibility = System.Windows.Visibility.Visible;
            //nameInfoLabel.Visibility = System.Windows.Visibility.Visible;
            //statusInfoLabel.Visibility = System.Windows.Visibility.Visible;
            //ageInfoLabel.Visibility = System.Windows.Visibility.Visible;
            //emailInfoLabel.Visibility = System.Windows.Visibility.Visible;
        }

        private void hideUserInfo()
        {
            //messageLabel.Visibility = System.Windows.Visibility.Collapsed;
            addFriendBtn.Visibility = System.Windows.Visibility.Collapsed;
            //messageToFriendBox.Visibility = System.Windows.Visibility.Collapsed;
            userPic.Visibility = System.Windows.Visibility.Collapsed;
            //nameInfoLabel.Visibility = System.Windows.Visibility.Collapsed;
            //statusInfoLabel.Visibility = System.Windows.Visibility.Collapsed;
            //ageInfoLabel.Visibility = System.Windows.Visibility.Collapsed;
            //emailInfoLabel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void showResultText()
        {
            int count = friends_list_view.Count;
            if (count < 1)
            {
                searchResultLabel.Visibility = System.Windows.Visibility.Collapsed;
                searchNoResultLabel.Visibility = System.Windows.Visibility.Visible;
                searchNoResultLabel.Content = "Your search returned no results";
            }
            else
            {
                if (count == 1)
                {
                    searchResultLabel.Content = "1 person found";
                }
                else
                {
                    searchResultLabel.Content = count + " people found";
                }
                searchNoResultLabel.Visibility = System.Windows.Visibility.Collapsed;
                searchResultLabel.Visibility = System.Windows.Visibility.Visible;
                //messageLabel.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void hideResultText()
        {
            searchResultLabel.Visibility = System.Windows.Visibility.Collapsed;
            searchNoResultLabel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void FriendMetroView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (FriendMetroView.SelectedIndex > -1)
            {
                User u = friends_list_view.getUser(FriendMetroView.SelectedIndex);
                //nameInfoLabel.Content += u.Name;
                //ageInfoLabel.Content += "20";
                //emailInfoLabel.Content += "email@email.com";
                //statusInfoLabel.Content += u.Status ? "On-line" : "Off-line";
                showUserInfo();
            }
        }

        private void userNameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                searchBntClick();
                e.Handled = true;
            }
        }
    }
}

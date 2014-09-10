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
    /// Interaction logic for FriendRequests.xaml
    /// </summary>
    public partial class FriendRequests : UserControl
    {
        FriendsViewList friends_list_view = new FriendsViewList();
        ActionController _chat = new ActionController(MainWindow.admin);
        public FriendRequests()
        {
            InitializeComponent();
            friends_list_view.AddRange(getFriendRequests());
            FriendMetroView.DataContext = friends_list_view;
            showResultText();
            hideUserInfo();
            userPic.Source = (new ImageSourceConverter()).ConvertFromString
                ("pack://application:,,,/Resources/UserPic.png") as ImageSource;
        }

        private List<User> getFriendRequests()
        {
            return _chat.getFriendRequests();
        }

        private void showUserInfo()
        {
            addFriendBtn.Visibility = System.Windows.Visibility.Visible;
            userPic.Visibility = System.Windows.Visibility.Visible;
            nameLabel.Visibility = System.Windows.Visibility.Visible;
        }

        private void hideUserInfo()
        {
            addFriendBtn.Visibility = System.Windows.Visibility.Collapsed;
            userPic.Visibility = System.Windows.Visibility.Collapsed;
            nameLabel.Visibility = System.Windows.Visibility.Collapsed;
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
            }
        }

        private void hideResultText()
        {
            searchResultLabel.Visibility = System.Windows.Visibility.Collapsed;
            searchNoResultLabel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void FriendMetroView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FriendMetroView.SelectedIndex > -1)
            {
                User u = friends_list_view.getUser(FriendMetroView.SelectedIndex);
                nameLabel.Content = u.Name;
                showUserInfo();
            }
        }

        private void addFriendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (FriendMetroView.SelectedIndex > -1)
            {
                User friend = friends_list_view.getUser(FriendMetroView.SelectedIndex);
                if (_chat.confirmFriendship(friend))
                {
                    okMsgLabel.Content = "OK";
                    friends_list_view.deleteUser(FriendMetroView.SelectedIndex);
                    hideUserInfo();
                    showResultText();
                    MainWindow.haveNewFriend = true;
                }
                else
                {
                    errorMsgLabel.Content = "Error!";
                    showUserInfo();
                }
            }
        }
    }
}

using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace ModernUINavigationApp1.Pages
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class ChatPage : UserControl
    {
        private List<Message> _messages = new List<Message>();
        private List<User> _friends = new List<User>();
        private ActionController _chat = new ActionController();
        FriendsViewList friends_list_view = new FriendsViewList();
        Admin _admin;
        public ChatPage()
        {
            _admin = MainWindow.admin;
            InitializeComponent();
            _chat.setAdminUser(_admin);
            _friends = _chat.getFriends();
            _chat.loadMessageHistory();
            _setListBoxSource();
            friends_list_view.AddRange(_friends);
            Thread messageUpdater = new Thread(_updateInputMessages);
            messageUpdater.IsBackground = true;
            messageUpdater.Start();
            Thread friendsUpdater = new Thread(_updateFriendsSource);
            friendsUpdater.IsBackground = true;
            friendsUpdater.Start();
            Thread friendsIncomeUpdater = new Thread(_updateIncomeFriendsSource);
            friendsIncomeUpdater.IsBackground = true;
            friendsIncomeUpdater.Start();
            //AppearanceManager.Current.AccentColor = System.Windows.Media.Colors.Green;
        }


        private void _updateMessageListBox()
        {
            /*
                MessageBox.DataSource = null;
                MessageBox.DataSource = _messages;
                MessageBox.DisplayMember = "Text";
             * */
            //friends_list_view.changeIconToUnread(FriendMetroView.SelectedIndex);
            if (_messages != null)
            {
                MessageView.SelectedIndex = MessageView.Items.Count - 1;
                int padding = 0;
                MessageView.Items.Clear();
                for (int i = padding; i < _messages.Count; i++)
                {
                    if (_messages[i].SenderID != _admin.Id)
                    {
                        MessageView.Items.Add("User: " + _messages[i].Text);
                        continue;
                    }
                    MessageView.Items.Add("Admin: " + _messages[i].Text);
                }
                var border = (Border)VisualTreeHelper.GetChild(MessageView, 0);
                var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }

        }

        private void _setListBoxSource()
        {   /*
            MessageView.DataSource = null;
            Friends.DataSource = _friends;
            Friends.DisplayMember = "Name";
            MessageView.DataSource = _messages;
            MessageView.DisplayMember = "Text";
            */
            FriendMetroView.DataContext = friends_list_view;

            foreach (Message msg in _messages)
            {
                MessageView.Items.Add(msg.Text);
            }
        }

        private void _updateInputMessages()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (_chat.updateNewMessages())
                {
                    Action<ListBox> action = (_message) => _updateMessageListBox();
                    List<Message> list = _chat.getMessages();
                    if (list != null)
                    {
                        _messages = list;
                        int a = 3;
                        if (this.MessageView.Dispatcher.CheckAccess())
                        {
                            action(MessageView);
                        }
                        else
                        {
                            this.MessageView.Dispatcher.BeginInvoke(DispatcherPriority.Normal, action, MessageView);
                        }
                    }
                }
            }
        }

        // BEGIN PRIVATE METHODS
        public void updateFriendsSource()
        {
            int index = FriendMetroView.SelectedIndex;
            _friends.Clear();
            _friends = _chat.getFriends();
            friends_list_view.AddRange(_friends);
            _chat.loadMessageHistory();
            _setListBoxSource();
            _updateMessageListBox();
            if (index > -1)
                FriendMetroView.SelectedIndex = index;
        }

        private void _updateFriendsSource()
        {
            while (true)
            {
                Thread.Sleep(2000);
                if (MainWindow.haveNewFriend)
                {
                    MainWindow.haveNewFriend = false;
                    Action<ListBox> action = (_friends) => updateFriendsSource();
                    if (this.FriendMetroView.Dispatcher.CheckAccess())
                    {
                        action(FriendMetroView);
                    }
                    else
                    {
                        this.FriendMetroView.Dispatcher.BeginInvoke(DispatcherPriority.Normal, action, FriendMetroView);
                    }
                }
            }
        }

        private void _updateIncomeFriendsSource()
        {
            while (true)
            {
                Thread.Sleep(50000);
                    Action<ListBox> action = (_friends) => updateFriendsSource();
                    if (this.FriendMetroView.Dispatcher.CheckAccess())
                    {
                        action(FriendMetroView);
                    }
                    else
                    {
                        this.FriendMetroView.Dispatcher.BeginInvoke(DispatcherPriority.Normal, action, FriendMetroView);
                    }
            }
        }
        private void _sendMessage()
        {
            if (MessageField.Text.ToString() != "")
            {
                Message message = new Message();
                message.Text = MessageField.Text.ToString();
                message.SenderID = _admin.Id;
                message.Date = DateTime.Now.ToUniversalTime();
                _chat.sendMessage(message);
                _messages = _chat.getMessages();
                MessageField.Text = "";
                _updateMessageListBox();
            }
        }

        private void MessageField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _sendMessage();
                e.Handled = true;
            }
        }

        private void SendBtn_Click_1(object sender, RoutedEventArgs e)
        {
            _sendMessage();
        }

        private void MessageView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageView.SelectedIndex = -1;
        }

        private void FriendMetroView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FriendMetroView.SelectedIndex > -1)
            {
                _chat.setCurrentFriend(_friends[FriendMetroView.SelectedIndex]);
                List<Message> list = _chat.getMessages();
                if (list != null)
                {
                    _messages = list;
                    _updateMessageListBox();
                }
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ModernUINavigationApp1
{
    public class ActionController
    {
        // BEGIN PEIVATE VARS
        private Admin _admin;
        private User _current_friend;
        private Dictionary<int, List<Message>> _inbox = new Dictionary<int, List<Message>>();
        private Dictionary<int, List<Message>> _outbox = new Dictionary<int, List<Message>>();
        private Request _request;
        private List<Message> _messages = new List<Message>();
        private List<User> _friends = new List<User>();
        // END  PRIVATE VARS

        // BIGIN PUBLIC METHODS

        public ActionController(Admin admin = null)
        {
            if (admin != null)
            {
                setAdminUser(admin);
            }
        }

        public bool setAdminUser(Admin admin)
        {
            if (admin != null)
            {
                _admin = admin;
                _request = new Request(_admin, "http://cryptochat.esy.es/");
                return true;
            }
            else
                return false;
        }

        public bool addFriend(User u)
        {
            _current_friend = u;
           return _request.addFriend(u);
        }

        public List<User> getFriendRequests()
        {
            return _request.getFriendRequrests();
        }

        public bool confirmFriendship(User user)
        {
            _current_friend = user;
            return _request.confirfFriendRequest(user);
        }

        public int CurrentUserId
        {
            get
            {
                return _current_friend.Id;
            }
        }

        public bool sendMessage(Message message)
        {
            if (_current_friend != null)
            {
                message.AddresseeID = _current_friend.Id;
                _admin.LastMessageTime = message.UnixDate;
                _outbox[_current_friend.Id].Add(message);
                _request.sendMessage(message);
                return true;
            }
            else
                return false;
        }

        // Отправленные сообщения не получают дату, поэтому остаются в верху списка _message.
        // Решить проблему с присвоением даты

        public List<Message> getMessages()
        {
            if (_current_friend != null && _messages.Count != (_inbox[_current_friend.Id].Count + _outbox[_current_friend.Id].Count))
            {
                _messages.Clear();
                _messages.AddRange(_inbox[_current_friend.Id]);
                _messages.AddRange(_outbox[_current_friend.Id]);
                _messages.Sort(delegate(Message x, Message y)
                {
                    return (x.UnixDate).CompareTo(y.UnixDate);
                });
                return _messages;
            }
            else
                return null;
        }

        public List<User> getFriends()
        {
            _friends = _request.getFriends();
            return _friends;
        }

        public void loadMessageHistory()
        {
            _initMessageContainers();
            updateNewMessages();
        }

        public void setCurrentFriend(User friend)
        {
            _current_friend = friend;
        }

        public bool updateNewMessages()
        {
            return _distributeMessages(_request.getNewMessages());
        }

        public List<User> findUserByName(String name)
        {
            return _request.findFriends(name);
        }

        // END   PUBLIC METHODS

        // BEGIN PRIVATE METHODS

        private List<Message> _loadUserMessages(User user)
        {
            return new List<Message>();
        }

        private void _initMessageContainers()
        {
            if (_friends != null)
            {
                _inbox = new Dictionary<int, List<Message>>();
                _outbox = new Dictionary<int, List<Message>>();
                foreach (User u in _friends)
                {
                    _inbox.Add(u.Id, new List<Message>());
                    _outbox.Add(u.Id, new List<Message>());
                }
            }
        }

        private bool _distributeMessages(List<Message> list)
        {
            if (list == null)
                return false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].SenderID != _admin.Id)
                {
                    _inbox[list[i].SenderID].Add(list[i]);
                }
                else
                {
                    _outbox[list[i].AddresseeID].Add(list[i]);
                }
            }
            foreach (User u in _friends)
            {
                foreach (Message m in list)
                {
                    if (u.Id == m.SenderID)
                    {
                        u.NewMessage = true;
                        break;
                    }
                }
            }
            return true;
        }
        // END PRIVATE METHODS

    }
}

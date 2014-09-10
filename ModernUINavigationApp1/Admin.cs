using System;
using System.Collections.Generic;
using System.Text;

namespace ModernUINavigationApp1

{
    public class Admin : User
    {
        // BEGIN PUBLIC VARS
        private List<User> _friends_list = new List<User>();
        protected String _password;
        private String _token;
        private long _last_message_time;
        // END PUBLIC VARS

        // BEGIN PUBLIC METHODS
        public Admin(String login = "", String password = "", int id = 0)
            : base(login, id)
        {
            _password = password;
        }

        public long LastMessageTime
        {
            get
            {
                return _last_message_time;
            }
            set
            {
                _last_message_time = value;
            }

        }

        public String Token
        {
            get;
            set;
        }

        public String Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public bool authorization()
        {
            bool answer = new Request(this, "http://cryptochat.esy.es/").authorization();
            if (answer)
            {
                return true;
            }
            else
                return false;
        }

        public List<User> getFriendsList()
        {
            return _friends_list;
        }
        public Dictionary<int, List<Message>> getAllNewMessages()
        {
            return new Dictionary<int, List<Message>>();
        }

        internal bool registration(String email)
        {
            bool answer = new Request(this, "http://cryptochat.esy.es/").registration(email);
            if (answer)
            {
                return true;
            }
            else
                return false; 
        }

        // END PUBLIC METHODS
        // BEGIN PRIVATE METHODS
        // END PRIVATE METHODS

    }
}

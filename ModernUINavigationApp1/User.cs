using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace ModernUINavigationApp1
{
    public class User
    {

        protected String _name;
        protected int _id;
        protected String _socket;
        protected bool _status;
        private bool _new_message;

        public User(String str = "", int id = 0, String socket = "", bool status = false)
        {
            _name = str;
            _id = id;
            _socket = socket;
            _status = status;
        }

        public bool Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        public bool NewMessage
        {
            get
            {
                return _new_message;
            }
            set
            {
                _new_message = value;
            }
        }

        public String Socket
        {
            get
            {
                return _socket;
            }
            set
            {
                _socket = value;
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }




    }
}

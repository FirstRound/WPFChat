using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Web;

namespace ModernUINavigationApp1

{
    public class Message
    {
        private String _message;
        private DateTime _date;
        private int _sender_id;
        private int _addressee_id;
        private int _dialog_id;
        private int _status;
        private int _message_id;
        public Message(String str = "")
        {
            _message = str;
        }

        public int MessageID
        {
            get
            {
                return _message_id;
            }
            set
            {
                _message_id = value;
            }
        }

        public int DialogID
        {
            get
            {
                return _dialog_id;
            }
            set
            {
                _dialog_id = value;
            }
        }

        public int Status
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

        public int SenderID
        {
            get
            {
                return _sender_id;
            }
            set
            {
                _sender_id = value;
            }
        }

        public int AddresseeID
        {
            get
            {
                return _addressee_id;
            }
            set
            {
                _addressee_id = value;
            }
        }

        public String Text
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public long UnixDate
        {

            get
            {
                return _convertToUnixTimestamp(_date);
            }
            set
            {
                _date = _convertFromUnixTimestamp(value);
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
            }
        }



        public String EncodeText
        {
            get
            {
                return _encode();
            }
        }

        // BEGIN PRIVATE METHODS
        private String _encode()
        {
            return _message;
        }

        private DateTime _convertFromUnixTimestamp(long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime t = origin.AddSeconds(timestamp);
            return t;
        }

        private long _convertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return (long)(Math.Floor(diff.TotalSeconds));
        }
        // END PRIVATE METHODS
    }
}

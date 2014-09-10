using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;


namespace ModernUINavigationApp1
{
    class Request
    {
        // BEGIN PRIVATE VARS
        private String _url;
        private Admin _admin;
        // END PRIVATE VARS

        // BEGIN PUBLIC METHODS
        public Request(Admin admin, String URL = "")
        {
            _admin = admin;
            _url = URL;
        }

        public String URL
        {
            set
            {
                _url = value;
            }
            get
            {
                return _url;
            }
        }

        public List<User> getFriendRequrests()
        {
            String data = "admin_id=" + _admin.Id;
            String action = "&action=get_friend_requests";
            String token = "&token=" + _admin.Token;
            data += action + token;
            String answer = sendPost(data);
            if (answer != "404")
                return _parseFriendsList(answer);
            else
                return new List<User>();
        }

        public bool confirfFriendRequest(User user)
        {
            String data = "admin_id=" + _admin.Id + "&user_id=" + user.Id;
            String action = "&action=confirm_friendship";
            String token = "&token=" + _admin.Token;
            data += action + token;
            String answer = sendPost(data);
            if (answer != "404")
                return true;
            else
                return false;
            
        }

        public bool sendMessage(Message message)
        {
            String data = "text=" + message.Text + "&from_id=" + _admin.Id + "&to_id="
                + message.AddresseeID + "&dialog_id=" + message.DialogID + "&date=" + message.UnixDate;
            String action = "&action=send_message";
            String token = "&token=" + _admin.Token;
            data += action + token;
            String answer = sendPost(data);
            if (answer != "")
                return true;
            else
                return false;
        }

        public List<User> getFriends()
        {
            String data = "admin_id=" + _admin.Id;
            String action = "&action=get_friends";
            String token = "&token=" + _admin.Token;
            data += action + token;
            String answer = sendPost(data);
            if (answer != "404")
                return _parseFriendsList(answer);
            else
                return new List<User>();
        }

        public bool authorization()
        {
            String data = "login=" + _admin.Name + "&password=" + _admin.Password;
            String action = "&action=authorization";
            data += action;
            String answer = sendPost(data);
            if (answer != "404")
            {
                try
                {
                    _parseAdminData(answer);
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;

            }
            else
                return false;
        }

        public List<Message> getNewMessages(bool state=true)
        {
            if (state == false)
                _admin.LastMessageTime = 0;
            String data = "last_message_time=" + _admin.LastMessageTime + "&admin_id=" + _admin.Id;
            String action = "&action=get_new_messages";
            String token = "&token=" + _admin.Token;
            data += action + token;
            String answer = sendPost(data);
            List<Message> list = null;
            if (answer != "404")
            {
                list = _parseMessagesList(answer);
            }
            return list;
        }

        internal bool registration(String email)
        {
            String data = "login=" + _admin.Name + "&password=" + _admin.Password + "&email=" + email;
            String action = "&action=registration";
            data += action;
            String answer = sendPost(data);
            if (answer != "404")
            {
                try
                {
                    _parseAdminData(answer);
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;

            }
            else
                return false;
        }

        public List<User> findFriends(String name)
        {
            String data = "name=" + name + "&admin_id=" + _admin.Id;
            String action = "&action=get_friends_by_name";
            String token = "&token=" + _admin.Token;
            data += action + token;
            String answer = sendPost(data);
            if (answer != "404")
                return _parseFriendsList(answer);
            else
                return new List<User>();
        }

        public bool addFriend(User friend)
        {
            String data = "admin_id=" + _admin.Id + "&user_id=" + friend.Id;
            String action = "&action=add_new_friend";
            String token = "&token=" + _admin.Token;
            data += action + token;
            String answer = sendPost(data);
            if (answer != "404")
                return true;
            else
                return false;
        }

        // END PUBLIC METHODS

        // BEGIN PRIVATE METHODS
        private List<Message> _parseMessagesList(String data)
        {
            List<Message> list = new List<Message>();
            Dictionary<string, object> temp = SimpleJSON.treeParseJSON(data);
            try
            {
                foreach (Dictionary<string, object> o in temp.Values)
                {
                    Message m = new Message();
                    m.AddresseeID = Convert.ToInt32(o["to_id"].ToString());
                    m.SenderID = Convert.ToInt32(o["from_id"]);
                    m.Text = System.Text.RegularExpressions.Regex.Unescape((String)((Dictionary<string, object>)o)["text"]);
                    m.UnixDate = Convert.ToInt64(o["date"]);
                    m.DialogID = Convert.ToInt32(o["dialog_id"]);
                    m.Status = Convert.ToInt32(o["status"]);
                    m.MessageID = Convert.ToInt32(o["message_id"]);
                    if (_admin.LastMessageTime < m.UnixDate)
                    {
                        _admin.LastMessageTime = m.UnixDate;
                    }
                    list.Add(m);
                }
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Error", MessageBoxButton.OK);
            }
            return list;
        }

        private List<User> _parseFriendsList(String data)
        {
            List<User> list = new List<User>();
            Dictionary<string, object> dic = SimpleJSON.treeParseJSON(data);
            foreach (Dictionary<string, object> val in dic.Values)
            {
                list.Add(new User((String)val["name"], Convert.ToInt32((String)val["user_id"]), (String)val["socket"], true));
            }
            return list;
        }

        private Admin _parseAdminData(String data)
        {
            try
            {
                Dictionary<string, object> dic = SimpleJSON.treeParseJSON(data);
                _admin.Name = (String)((Dictionary<string, object>)dic["0"])["name"];
                _admin.Password = (String)((Dictionary<string, object>)dic["0"])["password"];
                _admin.Token = (String)((Dictionary<string, object>)dic["0"])["token"];
                _admin.Id = Convert.ToInt32((String)((Dictionary<string, object>)dic["0"])["user_id"]);
                _admin.Socket = (String)((Dictionary<string, object>)dic["0"])["socket"];
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return _admin;
        }

        private string sendPost(string Data)
        {
            string Out = "";
            try
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(_url);
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.GetEncoding("UTF-8").GetBytes(Data);
                req.ContentLength = sentData.Length;
                System.IO.Stream sendStream = req.GetRequestStream();
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
                System.Net.WebResponse res = req.GetResponse();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                Out = String.Empty;
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    Out += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Error", MessageBoxButton.OK);
            }
            return Out;
        }

        // END PRIVATE METHODS
    }
}
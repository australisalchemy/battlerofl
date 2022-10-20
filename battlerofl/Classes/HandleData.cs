using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Windows;

namespace battlerofl
{
    public static class HandleData
    {
        #region Variables

        private static string m_emailAddr;
        private static string m_uPassword;
        private static bool m_hasData;

        private static string m_AuthToken;
        private static string m_LoginToken;

        #endregion

        // internal use
        private static string handleEmail
        {
            get { return m_emailAddr; }
            set { m_emailAddr = value; }
        }

        private static string handlePassword
        {
            get { return m_uPassword; }
            set { m_uPassword = value; }
        }

        // for outside use!
        public static void handleLogin(string email, string password)
        {
            handleEmail = email;
            handlePassword = password;
            m_hasData = true;
        }

        public static string getEmail()
        {
            if (m_hasData)
                return handleEmail;
            else
                return null;
        }

        public static string getPassword()
        {
            if (m_hasData)
                return handlePassword;
            else
                return null;
        }

        public static string getAuthToken()
        {
            string JSON = "";
            Battlelog.fetchPage(ref JSON, "https://battlelog.battlefield.com/bf3/launcher/token/?callback=", false, "GET");
            JObject jObj = JObject.Parse(JSON.Substring(1, JSON.Length - 3));

            JToken authToken = (JToken)jObj["data"];
            m_AuthToken = authToken["authToken"].ToString();

            return m_AuthToken;
        }

        public static string getLoginToken()
        {
            string JSON = "";
            Battlelog.fetchPage(ref JSON, "https://battlelog.battlefield.com/bf3/launcher/token/?callback=", false, "GET");

            JObject jObj = JObject.Parse(JSON.Substring(1, JSON.Length - 3));

            JToken loginToken = (JToken)jObj["data"];
            m_LoginToken = loginToken["encryptedToken"].ToString();

            return m_LoginToken;
        }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Windows;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;

namespace battlerofl
{
    public static class Battlelog
    {
        #region Variables
        private static string m_loginURL = "https://battlelog.battlefield.com/bf3/gate/login/";
        private static string m_gateURL = "https://battlelog.battlefield.com/bf3/gate";
        private static string m_redirectURL = "http://battlelog.battlefield.com/bf3/";
        private static HttpWebRequest m_req = null;
        private static CookieContainer m_cookies = null;
        private static bool m_isConnected = false;
        private static bool m_isAuthenticated = false;
        public static string m_playerID = null;
        private static Log m_Log = new Log();
        #endregion

        #region Public Methods

        /// <summary>
        /// Fetch a web page
        /// </summary>
        /// <param name="htmlData">Where the outputted data will be stored</param>
        /// <param name="url">The URL you want to fetch</param>
        /// <param name="isHTML">Is the webpage HTML or JSON?</param>
        /// <returns>Stores the data in a variable</returns>
        public static string fetchPage(ref string htmlData, string url, bool isHTML, string method, string sendData = "", bool isAJAX = false)
        {
            try
            {
                // create the html request
                m_req = (HttpWebRequest)WebRequest.Create(url);

                // properties
                m_req.CookieContainer = m_cookies;
                m_req.Method = method;
                m_req.KeepAlive = true;
                m_req.Timeout = 16000; // 10s timeout

                // header info
                m_req.Accept = (isHTML ? "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8" : "application/json,text/javascript,*/*;q=0.01");
                m_req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.77 Safari/535.7";
                m_req.Referer = url;
                m_req.Headers = new WebHeaderCollection();
                m_req.Proxy = null;

                if (!isHTML)
                    ((NameValueCollection)m_req.Headers)["X-Requested-With"] = "XMLHttpRequest";

                if (isAJAX)
                    m_req.Headers["X-AjaxNavigation"] = "1";
           
                m_req.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.3");
                m_req.Headers.Add("Accept-Encoding:ISO-8859-1");
                m_req.Headers.Add("Accept-Language:en-US,en;q=0.8");
                m_req.Headers.Add("Cache-Control:max-age=0");

                if (method == "POST")
                {
                    // encode the dataz!
                    byte[] bytes = Encoding.ASCII.GetBytes(sendData);
                    m_req.ContentLength = bytes.Length;

                    // add the body of the request
                    Stream st = m_req.GetRequestStream();
                    st.Write(bytes, 0, bytes.Length);
                    st.Close(); // close the stream

                    // don't want a 301 error
                    m_req.AllowAutoRedirect = false;

                    StreamReader stR = new StreamReader(st, Encoding.ASCII);
                    string output = "";
                    output = stR.ReadToEnd();
                    return output;
                }
                else
                {      
                // response handling
                HttpWebResponse response = (HttpWebResponse)m_req.GetResponse();

                if (response == null)
                {
                    MessageBox.Show("Web request failed");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("Web request failed with HTTP " + response.StatusCode);
                }

                Stream data = null;
                if ((data = response.GetResponseStream()) == null)
                {
                    MessageBox.Show("Stream failed");
                }

                StreamReader read = null;
                if ((read = new StreamReader(data)) == null)
                {
                    MessageBox.Show("Unable to read the stream");
                }

                htmlData = read.ReadToEnd();
                data.Close();
                read.Close();
                }

            }
            catch (Exception e)
            {
                m_Log.ShowWindow();
                m_Log.AddEvent("Exception Message: " + e.Message + " Exception StackTrace: " + e.StackTrace, "fetchPage Exception");
            }
            return htmlData;
            }

        /// <summary>
        /// Posts data to a website.
        /// </summary>
        /// <param name="URL">The URL to post to</param>
        /// <param name="data">The content to post</param>
        /// <param name="contentType">The content type you wish to post as</param>
        /// <returns>Response from the web page</returns>
        public static string POSTData(string URL, string data, string contentType)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            req.Method = "POST";
            req.ContentType = contentType;
            req.ContentLength = data.Length;
            req.CookieContainer = m_cookies;

            Stream writer = req.GetRequestStream();

            byte[] datatosend = Encoding.UTF8.GetBytes(data);
            writer.Write(datatosend, 0, datatosend.Length);

            string result = "";

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader read = new StreamReader(responseStream, Encoding.UTF8);

            result = read.ReadToEnd();
            return result;
        }

        /// <summary>
        /// Returns a boolean of the connection status
        /// </summary>
        /// <returns>Returns true = connected | false = not connected</returns>
        public static bool isConnected()
        {
            return m_isConnected;
        }

        /// <summary>
        /// Returns a boolean of the authentication status
        /// </summary>
        /// <returns>Returns true = authenticated | false = not authenticated</returns>
        public static bool isAuthenticated()
        {
            return m_isAuthenticated;
        }

        /// <summary>
        /// Returns a boolean of the connection and authentication status
        /// </summary>
        /// <returns>Returns true = authenticated and connected | false = not authenticated or not connected</returns>
        public static bool isReady()
        {
            return isConnected() && isAuthenticated();
        }

        /// <summary>
        /// Connect to Battlelog Gate
        /// </summary>
        /// <returns>Returns true if connected, false if not</returns>
        public static bool doConnection()
        {
            try
            {
                // allow only one maximum connection to battlelog
                if (isConnected())
                {
                    return true;
                }

                // assign a cookie container
                m_cookies = new CookieContainer();

                // make the HTTP request
                m_req = (HttpWebRequest)WebRequest.Create(m_gateURL);

                m_req.CookieContainer = m_cookies;
                m_req.Method = "GET";
                m_req.KeepAlive = true;
                m_req.Timeout = 10000;

                m_req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";   // accept data
                m_req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.77 Safari/535.7"; // user client (IE, FF, CH)
                m_req.Referer = m_gateURL;  // referer URL
                m_req.Headers = new WebHeaderCollection();
                m_req.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.3"); // accept UTF8 charset
                m_req.Headers.Add("Accept-Encoding:gzip,deflate,sdch"); // accept gzip'd encoding
                m_req.Headers.Add("Accept-Language:en-US,en;q=0.8");    // en-US (or en-au for australians, keep it American)
                m_req.Headers.Add("Cache-Control:max-age=0");       // cache age and control values

                // connection response and validation
                HttpWebResponse resp = (HttpWebResponse)m_req.GetResponse();    // get the HTML response
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    m_isConnected = true; // it is connected
                    return true;
                }

                // connection failed :(
                MessageBox.Show("Failed to connect: " + resp.StatusCode + " " + resp.StatusDescription, "HTTP failure", MessageBoxButton.OK, MessageBoxImage.Error);
                m_isConnected = false;
            }
            catch (Exception ex)
            {
                m_Log.ShowWindow();
                m_Log.AddEvent("Exception Message: " + ex.Message + " Exception StackTrace: " + ex.StackTrace, "doConnection Exception");
            }
            return false; // return false by default (same as m_isConnected)
        }

        /// <summary>
        /// Authenticate the user login details with battlelog
        /// </summary>
        /// <returns>Returns true if authenticated, false if not</returns>
        public static bool doAuthentication()
        {
            try
            {
                // cannot authenticate if battelog isn't connected
                if (!isConnected())
                {
                    return false;
                }

                // allow only one authentication per instance (this)
                if (isAuthenticated())
                {
                    return true;
                }

                // assign the http request
                m_req = (HttpWebRequest)WebRequest.Create(m_loginURL);

                // set the cookie from the previous connection
                m_req.CookieContainer = m_cookies;
                m_req.Method = "POST";  // POST, not GET!
                m_req.Timeout = 10000; // 10s timeout
                m_req.KeepAlive = true; // keep the connection alive

                // HEADERS
                m_req.Referer = m_gateURL;
                m_req.Headers = new WebHeaderCollection();
                m_req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";   // accept
                m_req.ContentType = "application/x-www-form-urlencoded";    // content type
                m_req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.7 (KHTML, like Gecko) Chrome/16.0.912.77 Safari/535.7"; // user client (IE, FF, CH)
                m_req.Headers.Add("Accept-Charset:ISO-8859-1,utf-8;q=0.7,*;q=0.3"); // accept UTF8 charset
                m_req.Headers.Add("Accept-Encoding:gzip,deflate,sdch"); // accept gzip'd encoding
                m_req.Headers.Add("Accept-Language:en-US,en;q=0.8");    // en-US (or en-au for australians, keep it American)
                m_req.Headers.Add("Cache-Control:max-age=0");       // cache age and control values

                // encode the dataz!
                string data = "email=" + Uri.EscapeDataString(HandleData.getEmail()) + "&password=" + Uri.EscapeDataString(HandleData.getPassword()) + "&submit=" + Uri.EscapeDataString("Sign in") + "&redirect=";
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                m_req.ContentLength = bytes.Length;

                // add the body of the request
                Stream st = m_req.GetRequestStream();
                st.Write(bytes, 0, bytes.Length);
                st.Close(); // close the stream

                // don't want a 301 error
                m_req.AllowAutoRedirect = false;

                // auth works!
                HttpWebResponse resp = (HttpWebResponse)m_req.GetResponse();    // get the response
                string location = string.Empty;

                for (int i = 0; i < resp.Headers.Count; ++i)
                {
                    if (resp.Headers.Keys[i].Equals("Location"))
                    {
                        location = resp.Headers[i];
                    }
                }

                // DEBUG
               

                if (resp.StatusCode == HttpStatusCode.Redirect && location.Equals(m_redirectURL))
                {
                    m_isAuthenticated = true;
                    return true; // it is authenticated :)
                }

                // auth failed :(
                MessageBox.Show("Incorrect email or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                m_isAuthenticated = false;
            }
            catch (Exception ex)
            {
                m_Log.ShowWindow();
                m_Log.AddEvent("Exception Message: " + ex.Message + " Exception StackTrace: " + ex.StackTrace, "doAuthentication Exception");
            }
            return false; // return false by default
        }

        /// <summary>
        /// Retrieves the Battlelog post-check-sum value.
        /// </summary>
        /// <returns>(string) post-check-sum</returns>
        public static string getChecksum()
        {
            HtmlDocument document = new HtmlDocument();
            string refdat = "";
            fetchPage(ref refdat, @"http://battlelog.battlefield.com/bf3/", true, "GET");
            document.LoadHtml(refdat);


            // check if the doc node exists
            if (document.DocumentNode != null)
            {
                var nodes = document.DocumentNode.SelectNodes("//input").Where(x => x.Attributes["name"] != null && x.Attributes["name"].Value.Contains("post-check-sum"));

                // iterate through the nodes
                foreach (var htmlNode in nodes)
                {
                    // set the checksum
                    refdat = htmlNode.Attributes["value"].Value;
                }
            }

            // return the checksum
            return refdat;
        }

        #endregion
    }
}

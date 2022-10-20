using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Windows.Threading;

namespace battlerofl
{
    /// <summary>
    /// Interaction logic for friendsPage.xaml
    /// </summary>
    public partial class friendsPage : Page
    {

        DispatcherTimer m_ServerFullTimer = new DispatcherTimer();
        DispatcherTimer m_ChangingMapsTimer = new DispatcherTimer();
        ServerData sData = new ServerData();
        bool m_isServerFull = false;
        string m_GameId = "";

        public friendsPage()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {


        }


        private void getFriendsOnline()
        {
            string data = "";
            data = Battlelog.POSTData("http://battlelog.battlefield.com/bf3/comcenter/sync/", "post-check-sum=" + Battlelog.getChecksum(), "application/x-www-form-urlencoded");

            // handle the json
            JObject o = JObject.Parse(data);
            JToken token = o["data"];
            JArray users = (JArray)token["friendscomcenter"];

            for (int i = 0; i < users.Count; i++)
            {
                JObject friend = (JObject)users[i];

                if (friend["presence"]["isOnline"].ToString() == "True")
                {
                    if (friend["presence"]["isPlaying"].ToString() == "True")
                    {
                        FriendsListItem item = new FriendsListItem
                        {
                            friendName = friend["username"].ToString(),
                            friendStatus = "Online",
                            ingameServer = friend["presence"]["serverName"].ToString(),
                            ingameServerGUID = friend["presence"]["serverGuid"].ToString()
                        };
                        lstFriends.Items.Add(item);
                    }
                    else
                    {
                        FriendsListItem item = new FriendsListItem
                        {
                            friendName = friend["username"].ToString(),
                            friendStatus = "Online",
                            ingameServer = "Not in game"
                        };
                        lstFriends.Items.Add(item);
                    }
                }
                else
                {
                    FriendsListItem itemOffline = new FriendsListItem
                    {
                        friendName = friend["username"].ToString(),
                        friendStatus = "Offline",
                        ingameServer = ""
                    };
                    lstFriends.Items.Add(itemOffline);
                }
            }
        } // end of partial class

        public class FriendsListItem
        {
            public string friendName { get; set; }
            public string friendStatus { get; set; }
            public string ingameServer { get; set; }
            public string ingameServerGUID { get; set; }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            lstFriends.Items.Clear();
            getFriendsOnline();
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            // join on friend
            FriendsListItem friend = (FriendsListItem)lstFriends.SelectedItem;
            string format = string.Format("http://battlelog.battlefield.com/bf3/servers/show/{0}/", friend.ingameServerGUID);

            // get serverId
            string serverOutput = "";
            Battlelog.fetchPage(ref serverOutput, format, false, "GET", "", true);
            JObject json = JObject.Parse(serverOutput);
            JToken token = json["context"]["server"];

            // get the gameId
            string gameId = token["gameId"].ToString();

            sData.joinServer(gameId);

            if (SlotReserve.reserveSlot == SlotReserve.SlotReservation.JOINING_GAME)
            {
                Executable.ExecuteMP(PlayerInfo.playerData.playerPersonaID, gameId, HandleData.getAuthToken(), HandleData.getLoginToken());
                lblStatus.Content = "Joined game!";
                GC.Collect();
            }
            else if (SlotReserve.reserveSlot == SlotReserve.SlotReservation.SERVER_CHANGING_MAPS)
            {
                // start a timer
                m_ChangingMapsTimer.Tick += new EventHandler(m_Timer_Tick);
                m_ChangingMapsTimer.Interval = new TimeSpan(0, 0, 10);
                m_ChangingMapsTimer.Start();
                m_isServerFull = false;
                lblStatus.Content = "Server is changing maps..";
            }
            else if (SlotReserve.reserveSlot == SlotReserve.SlotReservation.SERVER_FULL)
            {
                // start a timer
                m_ServerFullTimer.Tick += new EventHandler(m_Timer_Tick);
                m_ServerFullTimer.Interval = new TimeSpan(0, 0, 10);
                m_ServerFullTimer.Start();
                m_isServerFull = true;
                lblStatus.Content = "Server is full, position in queue: " + sData.m_QueuePosition;
            }
            else if (SlotReserve.reserveSlot == SlotReserve.SlotReservation.TOO_MANY_ATTEMPTS)
            {
                lblStatus.Content = "Too many attempts!";
            }

        }

        void m_Timer_Tick(object sender, EventArgs e)
        {
            timerFunction(m_isServerFull);
        }

        /// <summary>
        /// Checks if the server has finished changing maps or if the server has a free slot.
        /// </summary>
        /// <param name="isServerFull">(bool) Are you checking for Server Full?</param>
        private void timerFunction(bool isServerFull)
        {
            sData.joinServer(m_GameId);

            if (SlotReserve.reserveSlot == SlotReserve.SlotReservation.JOINING_GAME)
            {
                Executable.ExecuteMP(PlayerInfo.playerData.playerPersonaID, m_GameId, HandleData.getAuthToken(), HandleData.getLoginToken());
                m_ServerFullTimer.Stop();
            }
            else
            {
                // continue this shit.
                if (isServerFull)
                {
                    lblStatus.Content = "Server is full, Position in queue: " + sData.m_QueuePosition;
                }
                else
                {
                    lblStatus.Content = "Server is changing maps...";
                }
            }
        }


    }
}

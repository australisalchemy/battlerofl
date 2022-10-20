using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Net;
using System.Windows.Threading;

namespace battlerofl
{
    public class ServerData
    {
        public int m_QueuePosition = 0;

        public string ServerSearch(string query, string region, string ranked, string punkbuster, string GameModeRotation, string PasswordProtection, string gameSize, string freeSlots, string gameMode, string maps, string game, string serverName, string maprotation, string presets, int count, int offset = 0)
        {
            string filterServers = string.Format("offset=0&count={0}&filtered=1&expand=1&gameexpansions={1}&useLocation=On&q=&gamepresets={2}&ranked={3}&punkbuster={4}&maprotation={5}&password={6}&useAdvanced=&gameSize={7}&slots={8}&gamemodes={9}&maps={10}&settings=&regions={11}&country=", count, game, presets, ranked, punkbuster, maprotation, PasswordProtection, gameSize, freeSlots, gameMode, maps, region);
            // URL format is as followed:
            /*
             * 0: count
             * 1: expansion
             * 2: presets
             * 3: ranked
             * 4: pb
             * 5: map rotation
             * 6: password
             * 7: game size
             * 8: free slots
             * 9: game modes
             * 10: maps
             * 11: regions
             * 
             */

            string returnData = "";
            Battlelog.fetchPage(ref returnData, "http://battlelog.battlefield.com/bf3/servers/getAutoBrowseServers/?" + filterServers, false, "GET");

            // lets handle it elsewhere, not in this class.. so we can actually output it to somewhere without fancy and tedious formatting. ;)
            return returnData;
        }

        /// <summary>
        /// Joins a server, and sets the reserve type.
        /// </summary>
        public void joinServer(string gameId)
        {

            //string postData = string.Format("{\"post-check-sum\":\"{0}\",\"groupPersonaIdList\":null}", Battlelog.getChecksum());
            string sendData = "{\"post-check-sum\":\"" + Battlelog.getChecksum() + "\",\"groupPersonaIdList\":null}";

            //JObject parse = JObject.Parse(postData);

            string url = string.Format("http://battlelog.battlefield.com/bf3/launcher/reserveslotbygameid/{0}/{1}", PlayerInfo.playerData.playerPersonaID, gameId);
            string output = "";
            output = Battlelog.POSTData(url, sendData, "application/json");

            JObject json = JObject.Parse(output);

            JToken jToken = json["data"];
            JToken jMessage = json["message"];

            string reservationType = "";
            //string message = "";

            reservationType = jToken["joinState"].ToString();
            //message = jMessage["message"].ToString();


            if (reservationType != "joinState")
            {
                switch (reservationType)
                {
                    case "JOINED_GAME":
                        //Executable.ExecuteMP(PlayerInfo.playerData.playerPersonaID, m_gameId, HandleData.getAuthToken(), HandleData.getLoginToken());
                        SlotReserve.reserveSlot = SlotReserve.SlotReservation.JOINING_GAME;
                        break;

                    case "QUEUE_SLOT":
                        SlotReserve.reserveSlot = SlotReserve.SlotReservation.SERVER_FULL;
                        int.TryParse(jToken["slot"].ToString(), out m_QueuePosition);
                        break;

                    case "TOO_MANY_ATTEMPTS":
                        SlotReserve.reserveSlot = SlotReserve.SlotReservation.TOO_MANY_ATTEMPTS;
                        break;
                }
            }
            else
            {
                SlotReserve.reserveSlot = SlotReserve.SlotReservation.SERVER_CHANGING_MAPS;
            }
        }
        }
    }

    public class Players
    {
        public string m_PlayerName { get; set; }
        public string m_PlayerPersona { get; set; }
    }

    public class Server
    {
        public string ServerName { get; set; }
        public string ServerGUID { get; set; }
        public string GameID { get; set; }
        public string MapName { get; set; }
        public string PlayersOnServer { get; set; }
        public string ServerIPAddr { get; set; }
        public string ServerPort { get; set; }
        public string ServerPing { get; set; }
        public string ServerPreset { get; set; }
        public string GameType { get; set; }

        // settings
        public string ShowHud { get; set; }
        public string TeamBalance { get; set; }
        public string BulletDmg { get; set; }
        public string MiniMap { get; set; }
        public string ThirdPersonCam { get; set; }
        public string Vehicles { get; set; }
        public string RegenHealth { get; set; }
        public string TeamKills { get; set; }
        public string KillCam { get; set; }
        public string MinimapSpotting { get; set; }
        public string HealthPercent { get; set; }
        public string ManDownTime { get; set; }
        public string FriendlyFire { get; set; }
        public string ThreeDSpotting { get; set; }
        public string EnemyNameTags { get; set; }
        public string IdleTime { get; set; }
        public string BanAfterKicks { get; set; }
        public string RespawnTime { get; set; }
        public string SpawnOnSquadL { get; set; }

        // map rotation
        public string MapRotation { get; set; }
        public string ModeRotation { get; set; }
        public string MapType { get; set; }
    }

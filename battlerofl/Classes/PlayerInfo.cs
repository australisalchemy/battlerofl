using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Runtime;
using System.Windows.Resources;
using Newtonsoft.Json.Linq;

namespace battlerofl
{
    public static class PlayerInfo
    {
        #region Variables
        private static string m_soldierName;
        private static Log m_Log = new Log();
        #endregion

        #region Data Structures/Enums
        /// <summary>
        /// Contains all the data members for fetching the soldier data
        /// </summary>
        public struct playerData
        {
            public static string userId;
            public static int playerKills;
            public static int playerDeaths;
            public static int playerRank;
            public static int playerScore;
            public static double playerSkill;
            public static double playerAccuracy;
            public static double playerQuits;
            public static double playerKDR;
            public static int playerScorePM;
            public static string playerClanTag;
            public static double playerTime;
            public static double playerLongestHS;
            public static int playerRepairs;
            public static int playerRevives;
            public static int playerResupplies;
            public static int playerKillAssists;
            public static string playerGUID;
            public static string playerLicense;
            public static long playeruserID;
            public static string playerPersonaID;
        }

        #endregion

        #region Public and Private Methods
        /// <summary>
        /// Resets the values of playerStats struct
        /// </summary>
        private static void resetStats()
        {
            playerData.playerKills = 0;
            playerData.playerDeaths = 0;
            playerData.playerRank = 0;
            playerData.playerScore = 0;
            playerData.playerSkill = 0;
            playerData.playerAccuracy = 0;
            playerData.playerQuits = 0;
            playerData.playerKDR = 0;
            playerData.playerScorePM = 0;
            playerData.playerClanTag = "";
            playerData.playerTime = 0;
            playerData.playerLongestHS = 0;
            playerData.playerRepairs = 0;
            playerData.playerResupplies = 0;
            playerData.playerRevives = 0;
            playerData.playerKillAssists = 0;
        }

        /// <summary>
        /// Fetches the Soldier Data from Battlelog
        /// </summary>
        public static void fetchSoldierData(string soldierName)
        {
            try
            {
                m_soldierName = Battlelog.m_playerID;
                string result = string.Empty;

                Battlelog.fetchPage(ref result, "http://battlelog.battlefield.com/bf3/user/" + m_soldierName, true, "GET");

                // extract the personal id
                Match plID = Regex.Match(result, @"bf3/soldier/" + Battlelog.m_playerID + @"/stats/(\d+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (!plID.Success)
                {
                    MessageBox.Show("Invalid Soldier name was used!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                string personalID = plID.Groups[1].Value.Trim();
                playerData.playerPersonaID = personalID;

                // follow link to players detailed stats :)
                Battlelog.fetchPage(ref result, "http://battlelog.battlefield.com/bf3/overviewPopulateStats/" + personalID + "/None/1/", false, "GET");

                // JSON

                JObject jObj = JObject.Parse(result);
                JToken jToken = jObj["data"]; // the data tree

                JToken jStats = (JObject)jToken["overviewStats"];
                JToken juserId = (JObject)jToken["user"];

                // parse the stats
                int.TryParse(jStats["kills"].ToString(), out playerData.playerKills);
                int.TryParse(jStats["deaths"].ToString(), out playerData.playerDeaths);
                int.TryParse(jStats["rank"].ToString(), out playerData.playerRank);
                int.TryParse(jStats["scorePerMinute"].ToString(), out playerData.playerScorePM);
                int.TryParse(jStats["totalScore"].ToString(), out playerData.playerScore);
                int.TryParse(jStats["repairs"].ToString(), out playerData.playerRepairs);
                int.TryParse(jStats["resupplies"].ToString(), out playerData.playerResupplies);
                int.TryParse(jStats["killAssists"].ToString(), out playerData.playerKillAssists);
                double.TryParse(jStats["accuracy"].ToString(), out playerData.playerAccuracy);
                double.TryParse(jStats["timePlayed"].ToString(), out playerData.playerTime);
                double.TryParse(jStats["elo"].ToString(), out playerData.playerSkill);
                double.TryParse(jStats["kdRatio"].ToString(), out playerData.playerKDR);
                double.TryParse(jStats["longestHeadshot"].ToString(), out playerData.playerLongestHS);
                double.TryParse(jStats["quitPercentage"].ToString(), out playerData.playerQuits);
                playerData.userId = juserId["userId"].ToString();
            }
            catch (Exception e)
            {
                m_Log.AddEvent("Exception Message: " + e.Message + " Exception StackTrace: " + e.StackTrace, "fetchSoldierData Exception");
            }
        }

        /// <summary>
        /// Converts the Players played time total from seconds to hours
        /// </summary>
        /// <returns>(string) Played Time in hours</returns>
        public static string playerPlayedTime()
        {
            double totalTime = Math.Round(playerData.playerTime / 60 / 60, 2);
            string data = string.Format("{0:00H.##M}", totalTime);
            string retData = data.Replace(".", " ");
            return retData;
        }

        /// <summary>
        /// Converts the quit percentage to 2 decimal places.
        /// </summary>
        /// <returns></returns>
        public static double playerQuits()
        {
            return Math.Round(playerData.playerQuits, 2);
        }

        /// <summary>
        /// Converts the Players accuracy to 2 decimal places
        /// </summary>
        /// <returns>Player Accuracy to 2 decimal places</returns>
        public static double playerAccuracy()
        {
            double accuracy = Math.Round(playerData.playerAccuracy, 2);
            return accuracy;
        }

        /// <summary>
        /// Converts the Players Kill Death Ratio into 3 decimal places
        /// </summary>
        /// <returns>Player KDR to 3 decimal places</returns>
        public static double playerKDR()
        {
            return Math.Round(playerData.playerKDR, 3);
        }

        /// <summary>
        /// Gets the players rank image.
        /// </summary>
        /// <param name="rank">The players rank</param>
        /// <param name="size">The image size</param>
        /// <returns>The location of the rank image</returns>
        public static string getRankImage(int rank, string size)
        {
            string[] validSizes = new string[4];
            validSizes[0] = "tiny";
            validSizes[1] = "small";
            validSizes[2] = "medium";
            validSizes[3] = "large";

            if (rank <= 45) {
            return "http://battlelog-cdn.battlefield.com/public/profile/bf3/stats/ranks/" + size + "/r" + rank + ".png";
            }
            else
            {
                return "http://battlelog-cdn.battlefield.com/public/profile/bf3/stats/ranks/" + size + "/ss1.png";
            }

        }

        /// <summary>
        /// Returns kit information
        /// </summary>
        /// <param name="kitName">The name of the kit (playerKit)</param>
        /// <param name="imageSize">The size of the image (string)</param>
        /// <returns>(string) Kit Image</returns>
        public static string getKitImage(string kit, string imgSize, string team)
        {
            string[] validKits = new string[4];
            string[] validTeam = new string[2];
            validKits[0] = "assault";
            validKits[1] = "recon";
            validKits[2] = "engineer";
            validKits[3] = "support";

            validTeam[0] = "us";
            validTeam[1] = "ru";

            foreach (string strName in validKits)
            {
                if (!validKits.Contains(strName))
                {
                    throw new Exception("Invalid kit name");
                }
            }

            foreach (string strTeam in validTeam)
            {
                if (!validTeam.Contains(strTeam))
                {
                    throw new Exception("Invalid team name");
                }
            }

            // return the kit image
            return "http://battlelog-cdn.battlefield.com/public/profile/kits/" + imgSize + "/bf3-"+ team + "-" + kit + ".png";
        }

        /// <summary>
        /// Returns the players total score
        /// </summary>
        /// <returns>Total score of the player, formatted</returns>
        public static string getTotalScore()
        {
            return string.Format("{0:### ### ###}", playerData.playerScore);
        }

        public static string handleServiceStarRanks(int rank)
        {
            if (rank == 46)
            {
                return "ss1";
            }
            else if (rank == 47)
            {
                return "ss2";
            }
            else if (rank == 48)
            {
                return "ss3";
            }
            else if (rank == 49)
            {
                return "ss4";
            }
            else if (rank == 50)
            {
                return "ss5";
            }
            else if (rank == 51)
            {
                return "ss6";
            }
            else if (rank == 52)
            {
                return "ss7";
            }
            return "";
        }

        #endregion
    }
}

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
using Newtonsoft.Json.Linq;
using System.Windows.Threading;
using System.Net;

namespace battlerofl
{
    /// <summary>
    /// Interaction logic for ServersPage.xaml
    /// </summary>
    public partial class ServersPage : Page
    {
        #region Variables
        private string m_MapNameHolder = "";
        private string m_preset = "";
        private string m_IPAddr = "";
        private string m_gameMode = "";
        private string m_sHud = "";
        private string m_sBal = "";
        private string m_sBulD = "";
        private string m_sMini = "";
        private string m_sCam = "";
        private string m_sVeh = "";
        private string m_sRegen = "";
        private string m_sTks = "";
        private string m_sKCam = "";
        private string m_sMSpt = "";
        private string m_sHPP = "";
        private string m_sManD = "";
        private string m_sFFir = "";
        private string m_s3DS = "";
        private string m_sNameT = "";
        private string m_sIdle = "";
        private string m_sTKK = "";
        private string m_sResp = "";
        private string m_sSLS = "";

        string[] gameSize = new string[5];
        string passwordProtection = "";
        string isRanked = "";
        string punkbuster = "";
        string gameModeRotation = "";
        string mapRotation = "";
        string[] gameXP = new string[3];
        string[] freeSlots = new string[5];
        string[] presets = new string[3];
        string[] gameMode = new string[6];
        string[] maps = new string[14];
        string filterByName = "";
        string region = "";
        public ServerData sData = new ServerData();
        ServerData aData = new ServerData();
        public JObject jObj;

        string m_gameId = "";
        string m_ServName = "";
        string m_SGUID = "";

        int m_positionInQueue = 0;

        bool m_isServerFull = false;

        DispatcherTimer m_ServerFullTimer = new DispatcherTimer();
        DispatcherTimer m_ChangingMapsTimer = new DispatcherTimer();

        #endregion

        public ServersPage()
        {
            InitializeComponent();
        }

        #region B2K Support
        private void chkB2K_Checked(object sender, RoutedEventArgs e)
        {
            // allow the user to find B2K maps
            chkOman.IsEnabled = true;
            chkSharqi.IsEnabled = true;
            chkWake.IsEnabled = true;
            chkStrike.IsEnabled = true;

            // allow the user to find B2K games
            gameXP[2] = "xpack1";
        }

        private void chkB2K_Unchecked(object sender, RoutedEventArgs e)
        {
            // do not allow the user to find B2K maps
            chkOman.IsEnabled = false;
            chkSharqi.IsEnabled = false;
            chkWake.IsEnabled = false;
            chkStrike.IsEnabled = false;
            gameXP[2] = "";
        }

        private void checkLicense()
        {
            if (PlayerInfo.playerData.playerLicense == "b2k")
            {
                chkB2K.IsEnabled = true;
            }
            else
            {
                chkB2K.IsEnabled = false;
            }
        }

        #endregion

        #region Handling

        #region Server Settings

        private void chkRanked_Checked(object sender, RoutedEventArgs e)
        {
            isRanked = "On";
        }

        private void chkPunkbuster_Checked(object sender, RoutedEventArgs e)
        {
            punkbuster = "On";
        }

        private void chkPunkbuster_Unchecked(object sender, RoutedEventArgs e)
        {
            punkbuster = "Off";
        }

        private void chkRanked_Unchecked(object sender, RoutedEventArgs e)
        {
            isRanked = "Off";
        }

        private void chkGameModeRotation_Checked(object sender, RoutedEventArgs e)
        {
            gameModeRotation = "On";
        }

        private void chkPasswordProtected_Checked(object sender, RoutedEventArgs e)
        {
            passwordProtection = "On";
        }

        private void chkMapRotation_Checked(object sender, RoutedEventArgs e)
        {
            mapRotation = "On";
        }

        private void chkPasswordProtected_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordProtection = "Off";
        }

        private void chkGameModeRotation_Unchecked(object sender, RoutedEventArgs e)
        {
            gameModeRotation = "Off";
        }

        private void chkMapRotation_Unchecked(object sender, RoutedEventArgs e)
        {
            mapRotation = "Off";
        }

        private void chkBF3_Checked(object sender, RoutedEventArgs e)
        {
            gameXP[1] = "default";
        }

        private void chkBF3_Unchecked(object sender, RoutedEventArgs e)
        {
            gameXP[1] = "";
        }

        #endregion

        #region Game Size

        private void chk16_Checked(object sender, RoutedEventArgs e)
        {
            gameSize[0] = "16";
        }

        private void chk24_Checked(object sender, RoutedEventArgs e)
        {
            gameSize[1] = "24";
        }

        private void chk32_Checked(object sender, RoutedEventArgs e)
        {
            gameSize[2] = "32";
        }

        private void chk64_Checked(object sender, RoutedEventArgs e)
        {
            gameSize[3] = "64";
        }

        private void chk32_Unchecked(object sender, RoutedEventArgs e)
        {
            gameSize[2] = "";
        }

        private void chk24_Unchecked(object sender, RoutedEventArgs e)
        {
            gameSize[1] = "";
        }

        private void chk16_Unchecked(object sender, RoutedEventArgs e)
        {
            gameSize[0] = "";
        }

        private void chk64_Unchecked(object sender, RoutedEventArgs e)
        {
            gameSize[3] = "";
        }

        #endregion

        #region Maps

        #region Vanilla Maps

        #region Checked
        private void chkGrandBazaar_Checked(object sender, RoutedEventArgs e)
        {
            maps[0] = "MP_001";
        }

        private void chkCaspian_Checked(object sender, RoutedEventArgs e)
        {
            maps[1] = "MP_007";
        }

        private void chkFirestorm_Checked(object sender, RoutedEventArgs e)
        {
            maps[2] = "MP_012";
        }

        private void chkNoshahr_Checked(object sender, RoutedEventArgs e)
        {
            maps[3] = "MP_017";
        }

        private void chkKharg_Checked(object sender, RoutedEventArgs e)
        {
            maps[4] = "MP_018";
        }

        private void chkOperationMetro_Checked(object sender, RoutedEventArgs e)
        {
            maps[5] = "MP_Subway";
        }

        private void chkTehran_Checked(object sender, RoutedEventArgs e)
        {
            maps[6] = "MP_003";
        }

        private void chkSeine_Checked(object sender, RoutedEventArgs e)
        {
            maps[7] = "MP_011";
        }

        private void chkDamavand_Checked(object sender, RoutedEventArgs e)
        {
            maps[8] = "MP_013";
        }
        #endregion

        #region Unchecked
        private void chkGrandBazaar_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[0] = "";
        }

        private void chkCaspian_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[1] = "";
        }

        private void chkFirestorm_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[2] = "";
        }

        private void chkNoshahr_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[3] = "";
        }

        private void chkKharg_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[4] = "";
        }

        private void chkOperationMetro_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[5] = "";
        }

        private void chkTehran_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[6] = "";
        }

        private void chkSeine_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[7] = "";
        }

        private void chkDamavand_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[8] = "";
        }
        #endregion

        #endregion

        #region B2K Maps

        private void chkStrike_Checked(object sender, RoutedEventArgs e)
        {
            maps[9] = "XP1_001";
        }

        private void chkSharqi_Checked(object sender, RoutedEventArgs e)
        {
            maps[10] = "XP1_003";
        }

        private void chkOman_Checked(object sender, RoutedEventArgs e)
        {
            maps[11] = "XP1_002";
        }

        private void chkWake_Checked(object sender, RoutedEventArgs e)
        {
            maps[12] = "XP1_004";
        }

        private void chkStrike_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[9] = "";
        }

        private void chkSharqi_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[10] = "";
        }

        private void chkOman_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[11] = "";
        }

        private void chkWake_Unchecked(object sender, RoutedEventArgs e)
        {
            maps[12] = "";
        }

        #endregion

        #endregion

        #region Free Slots

        private void chk1to5_Checked(object sender, RoutedEventArgs e)
        {
            freeSlots[0] = "1";
        }

        private void chk6to10_Checked(object sender, RoutedEventArgs e)
        {
            freeSlots[1] = "2";
        }

        private void chk10Plus_Checked(object sender, RoutedEventArgs e)
        {
            freeSlots[2] = "3";
        }

        private void chkEmpty_Checked(object sender, RoutedEventArgs e)
        {
            freeSlots[3] = "4";
        }

        private void chk1to5_Unchecked(object sender, RoutedEventArgs e)
        {
            freeSlots[0] = "";
        }

        private void chk6to10_Unchecked(object sender, RoutedEventArgs e)
        {
            freeSlots[1] = "";
        }

        private void chk10Plus_Unchecked(object sender, RoutedEventArgs e)
        {
            freeSlots[2] = "";
        }

        private void chkEmpty_Unchecked(object sender, RoutedEventArgs e)
        {
            freeSlots[3] = "";
        }

        #endregion

        #region Preset

        private void chkNormal_Checked(object sender, RoutedEventArgs e)
        {
            presets[0] = "normal";
        }

        private void chkHardcore_Checked(object sender, RoutedEventArgs e)
        {
            presets[1] = "hardcore";
        }

        private void chkInfantryOnly_Checked(object sender, RoutedEventArgs e)
        {
            presets[2] = "infantry";
        }

        private void chkInfantryOnly_Unchecked(object sender, RoutedEventArgs e)
        {
            presets[2] = "";
        }

        private void chkHardcore_Unchecked(object sender, RoutedEventArgs e)
        {
            presets[1] = "";
        }

        private void chkNormal_Unchecked(object sender, RoutedEventArgs e)
        {
            presets[0] = "";
        }

        #endregion

        #region Game Mode

        private void chkConquest_Checked(object sender, RoutedEventArgs e)
        {
            gameMode[0] = "conquest";
        }

        private void chkConquestLarge_Checked(object sender, RoutedEventArgs e)
        {
            gameMode[1] = "conquestlarge";
        }

        private void chkRush_Checked(object sender, RoutedEventArgs e)
        {
            gameMode[2] = "rush";
        }

        private void chkSquadDeathMatch_Checked(object sender, RoutedEventArgs e)
        {
            gameMode[3] = "sqdm";
        }

        private void chkSquadRush_Checked(object sender, RoutedEventArgs e)
        {
            gameMode[4] = "sqrush";
        }

        private void chkTDM_Checked(object sender, RoutedEventArgs e)
        {
            gameMode[5] = "teamdeathmatch";
        }

        private void chkTDM_Unchecked(object sender, RoutedEventArgs e)
        {
            gameMode[5] = "";
        }

        private void chkSquadRush_Unchecked(object sender, RoutedEventArgs e)
        {
            gameMode[4] = "";
        }

        private void chkSquadDeathMatch_Unchecked(object sender, RoutedEventArgs e)
        {
            gameMode[3] = "";
        }

        private void chkRush_Unchecked(object sender, RoutedEventArgs e)
        {
            gameMode[2] = "";
        }

        private void chkConquestLarge_Unchecked(object sender, RoutedEventArgs e)
        {
            gameMode[1] = "";
        }

        private void chkConquest_Unchecked(object sender, RoutedEventArgs e)
        {
            gameMode[0] = "";
        }


        #endregion

        #endregion

        private string ArgumentString(string[] arg)
        {
            return HttpUtility.UrlEncode(string.Join("|", arg).Trim(new char[] { '|' }));
        }

        public void assignInfo()
        {
            JToken serverData = jObj["data"];

            JArray jsAr = (JArray)serverData; // servers

            for (int i = 0; i < jsAr.Count; i++)
            {
                JObject jDetails = (JObject)jsAr[i];

                m_MapNameHolder = jDetails["map"].ToString();

                m_IPAddr = jDetails["ip"].ToString();
                m_gameMode = jDetails["mapMode"].ToString();

                // put the JSON into a variable somewhere
                /*ThreadStart t_AssignVars = new ThreadStart(AssignVariables);
                convertData = new Thread(t_AssignVars);
                convertData.Start(); */

                AssignVariables();

                Server vars = new Server
                {
                    ServerName = jDetails["name"].ToString(),
                    PlayersOnServer = jDetails["numPlayers"].ToString() + "/" + jDetails["maxPlayers"].ToString() + "[" + jDetails["numQueued"].ToString() + "]",
                    ServerGUID = jDetails["guid"].ToString(),
                    GameID = jDetails["gameId"].ToString(),
                    GameType = m_gameMode,
                    ServerIPAddr = m_IPAddr,
                    ServerPing = PingUtil.PingAddress(m_IPAddr),
                    ServerPreset = m_preset,
                    MapName = m_MapNameHolder
                };

                // add the items to the lists
                lsvServers.Items.Add(vars);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

            // check how many items are in the list
            if (lsvServers.Items.Count > 0)
            {
                lsvServers.Items.Clear();
            }
            else
            {
                // skip this whole line
            }

            queryBattlelog();
        }

        private void lsvServers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvServers.SelectedIndex > -1)
            {
                Server serv = (Server)lsvServers.SelectedItem;
                getMapRotation(serv.ServerGUID);
                AssignVariables();

                #region Settings
                serv.ShowHud = m_sHud;
                serv.ThreeDSpotting = m_s3DS;
                serv.BanAfterKicks = m_sTKK;
                serv.BulletDmg = m_sBulD;
                serv.EnemyNameTags = m_sNameT;
                serv.FriendlyFire = m_sFFir;
                serv.HealthPercent = m_sHPP;
                serv.IdleTime = m_sIdle;
                serv.KillCam = m_sKCam;
                serv.ManDownTime = m_sManD;
                serv.MiniMap = m_sMini;
                serv.MinimapSpotting = m_sMSpt;
                serv.RegenHealth = m_sRegen;
                serv.RespawnTime = m_sResp;
                serv.SpawnOnSquadL = m_sSLS;
                serv.TeamBalance = m_sBal;
                serv.TeamKills = m_sTks;
                serv.ThirdPersonCam = m_sCam;
                serv.Vehicles = m_sVeh;
                serv.ServerPreset = m_preset;
                #endregion

                lblIPreset.Content = "Preset: " + serv.ServerPreset;
                lblHud.Content = "HUD: " + serv.ShowHud;
                lblRespawnTime.Content = "Respawn Time: " + serv.RespawnTime + "%";
                lblBulletDamage.Content = "Bullet Damage: " + serv.BulletDmg + "%";
                lblMinimap.Content = "Show Minimap: " + serv.MiniMap;
                lblThirdPersonCam.Content = "Third Person Cam: " + serv.ThirdPersonCam;
                lblVehicles.Content = "Vehicles: " + serv.Vehicles;
                lblRegenHealth.Content = "Regenerate Health: " + serv.RegenHealth;
                lblTeamKillsBeforeKick.Content = "TK's before kick: " + serv.TeamKills;
                lblKillcam.Content = "Killcam: " + serv.KillCam;
                lblMinimapSpotting.Content = "Minimap Spotting: " + serv.MinimapSpotting;
                lblHealthPercent.Content = "Health: " + serv.HealthPercent + "%";
                lblManDownTime.Content = "Man down time: " + serv.ManDownTime + "%";
                lblFriendlyFire.Content = "Friendly Fire: " + serv.FriendlyFire;
                lbl3DSpotting.Content = "3D Spotting: " + serv.ThreeDSpotting;
                lblEnemyNames.Content = "Show enemy tags: " + serv.EnemyNameTags;
                lblIdleTime.Content = "Idle time: " + serv.IdleTime;
                lblBanAfterKicks.Content = "Ban after kicks: " + serv.BanAfterKicks;
                lblSquadLeaderSpawn.Content = "Spawn on SL only: " + serv.SpawnOnSquadL;
                m_gameId = serv.GameID;
                m_ServName = serv.ServerName;
                m_SGUID = serv.ServerGUID;
                btnJoin.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                // do nothing bruz
                btnJoin.Visibility = System.Windows.Visibility.Hidden;
                lsvMaps.Items.Clear();
            }
        }

        private void AssignVariables()
        {
            #region Converters

            // maps
            if (m_MapNameHolder == "MP_001")
            {
                m_MapNameHolder = "Grand Bazaar";
            }
            else if (m_MapNameHolder == "MP_003")
            {
                m_MapNameHolder = "Tehran Highway";
            }
            else if (m_MapNameHolder == "MP_007")
            {
                m_MapNameHolder = "Caspian Border";
            }
            else if (m_MapNameHolder == "MP_011")
            {
                m_MapNameHolder = "Seine Crossing";
            }
            else if (m_MapNameHolder == "MP_012")
            {
                m_MapNameHolder = "Operation Firestorm";
            }
            else if (m_MapNameHolder == "MP_013")
            {
                m_MapNameHolder = "Damavand Peak";
            }
            else if (m_MapNameHolder == "MP_017")
            {
                m_MapNameHolder = "Noshahr Canals";
            }
            else if (m_MapNameHolder == "MP_018")
            {
                m_MapNameHolder = "Kharg Island";
            }
            else if (m_MapNameHolder == "MP_Subway")
            {
                m_MapNameHolder = "Operation Metro";
            }
            else if (m_MapNameHolder == "XP1_001")
            {
                m_MapNameHolder = "Strike at Karkand";
            }
            else if (m_MapNameHolder == "XP1_002")
            {
                m_MapNameHolder = "Gulf of Oman";
            }
            else if (m_MapNameHolder == "XP1_003")
            {
                m_MapNameHolder = "Sharqi Peninsula";
            }
            else if (m_MapNameHolder == "XP1_004")
            {
                m_MapNameHolder = "Wake Island";
            }

            // presets
            if (m_preset == "1")
            {
                m_preset = "Normal";
            }
            else if (m_preset == "2")
            {
                m_preset = "Hardcore";
            }
            else if (m_preset == "4")
            {
                m_preset = "Infantry Only";
            }
            else if (m_preset == "8")
            {
                m_preset = "Custom";
            }

            if (m_gameMode == "1")
            {
                m_gameMode = "Conquest";
            }
            else if (m_gameMode == "2")
            {
                m_gameMode = "Rush";
            }
            else if (m_gameMode == "4")
            {
                m_gameMode = "Squad Rush";
            }
            else if (m_gameMode == "8")
            {
                m_gameMode = "Squad Deathmatch";
            }
            else if (m_gameMode == "16")
            {
                m_gameMode = "Conquest Assault";
            }
            else if (m_gameMode == "32")
            {
                m_gameMode = "Team Deathmatch";
            }
            else if (m_gameMode == "64")
            {
                m_gameMode = "Conquest (Large)";
            }

            // server settings
            if (m_s3DS == "1")
            {
                m_s3DS = "On";
            }
            else
            {
                m_s3DS = "Off";
            }
            if (m_sFFir == "1")
            {
                m_sFFir = "On";
            }
            else
            {
                m_sFFir = "Off";
            }

            if (m_sHud == "1")
            {
                m_sHud = "On";
            }
            else
            {
                m_sHud = "Off";
            }

            if (m_sMini == "1")
            {
                m_sMini = "On";
            }
            else
            {
                m_sMini = "Off";
            }

            if (m_sSLS == "1")
            {
                m_sSLS = "On";
            }
            else
            {
                m_sSLS = "Off";
            }

            if (m_sBal == "1")
            {
                m_sBal = "On";
            }
            else
            {
                m_sBal = "Off";
            }

            if (m_sCam == "1")
            {
                m_sCam = "On";
            }
            else
            {
                m_sCam = "Off";
            }

            if (m_sNameT == "1")
            {
                m_sNameT = "On";
            }
            else
            {
                m_sNameT = "Off";
            }
            if (m_sVeh == "1")
            {
                m_sVeh = "On";
            }
            else
            {
                m_sVeh = "Off";
            }

            if (m_sMSpt == "1")
            {
                m_sMSpt = "On";
            }
            else
            {
                m_sMSpt = "Off";
            }

            if (m_sRegen == "1")
            {
                m_sRegen = "On";
            }
            else
            {
                m_sRegen = "Off";
            }
            #endregion
        }

        private void queryBattlelog()
        {
            int count = 0;
            int.TryParse(txtCount.Text, out count);
            // query variables
            filterByName = txtServerName.Text;

            #region Regions
            if (cboRegion.Text == "Oceania")
            {
                region = "OC";
            }
            else if (cboRegion.Text == "North America")
            {
                region = "NAm";
            }
            else if (cboRegion.Text == "Europe")
            {
                region = "EU";
            }
            else if (cboRegion.Text == "South America")
            {
                region = "SAm";
            }
            else if (cboRegion.Text == "Asia")
            {
                region = "Asia";
            }
            #endregion

            // assign it to a json object
            jObj = JObject.Parse(sData.ServerSearch("", region, isRanked, punkbuster, gameModeRotation, passwordProtection, ArgumentString(gameSize), ArgumentString(freeSlots), ArgumentString(gameMode), ArgumentString(maps), ArgumentString(gameXP), filterByName, mapRotation, ArgumentString(presets), count));
            // only use argumentstring method for gamesize, maps, game mode, preset, game, and free slots

            assignInfo();
        }

        private void btnJoin_Click(object sender, RoutedEventArgs e)
        {
            sData.joinServer(m_gameId);
            if (SlotReserve.reserveSlot == SlotReserve.SlotReservation.JOINING_GAME)
            {
                Executable.ExecuteMP(PlayerInfo.playerData.playerPersonaID, m_gameId, HandleData.getAuthToken(), HandleData.getLoginToken());
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
                lblStatus.Content = "Server is full, position in queue: " + m_positionInQueue;
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
            sData.joinServer(m_gameId);

            if (SlotReserve.reserveSlot == SlotReserve.SlotReservation.JOINING_GAME)
            {
                Executable.ExecuteMP(PlayerInfo.playerData.playerPersonaID, m_gameId, HandleData.getAuthToken(), HandleData.getLoginToken());
                m_ServerFullTimer.Stop();
            }
            else
            {
                // continue this shit.
                if (isServerFull)
                {
                    lblStatus.Content = "Server is full, Position in queue: " + m_positionInQueue;
                }
                else
                {
                    lblStatus.Content = "Server is changing maps...";
                }
            }
        }

        private void getMapRotation(string serverGUID)
        {
            string output = "";
            string urlFormat = string.Format("http://battlelog.battlefield.com/bf3/servers/show/{0}/", serverGUID);
            Battlelog.fetchPage(ref output, urlFormat, false, "GET", "", true);

            JObject json = JObject.Parse(output);
            JToken serverSettings = json["context"]["server"]["settings"];
            JToken otherSettings = json["context"]["server"];

            #region Server Settings
            m_preset = otherSettings["preset"].ToString();
            m_sHud = serverSettings["vhud"].ToString();
            m_sBal = serverSettings["vaba"].ToString();
            m_sBulD = serverSettings["vbdm"].ToString();
            m_sMini = serverSettings["vmin"].ToString();
            m_sCam = serverSettings["v3ca"].ToString();
            m_sVeh = serverSettings["vvsa"].ToString();
            m_sRegen = serverSettings["vrhe"].ToString();
            m_sTks = serverSettings["vtkc"].ToString();
            m_sKCam = serverSettings["vkca"].ToString();
            m_sMSpt = serverSettings["vmsp"].ToString();
            m_sHPP = serverSettings["vshe"].ToString();
            m_sManD = serverSettings["vpmd"].ToString();
            m_sFFir = serverSettings["vffi"].ToString();
            m_s3DS = serverSettings["v3sp"].ToString();
            m_sNameT = serverSettings["vnta"].ToString();
            m_sIdle = serverSettings["vnit"].ToString();
            m_sResp = serverSettings["vprt"].ToString();
            m_sTKK = serverSettings["vtkk"].ToString();
            m_sSLS = serverSettings["osls"].ToString();
            #endregion

        }

        private void chkLANSupport_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}
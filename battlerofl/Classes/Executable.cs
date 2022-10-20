using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows;

namespace battlerofl
{
    public static class Executable
    {
        // variables
        public static string m_authToken = HandleData.getAuthToken();
        public static string m_loginToken = HandleData.getLoginToken();
        private static Random m_portRandom = new Random();
        private static Log m_Log = new Log();
        private static readonly string m_mpCommandLineArgsFormat = "-webMode MP -Origin_NoAppFocus -onlineEnvironment prod -loginToken {0} -AuthToken {1} -requestState State_ClaimReservation -requestStateParams \"<data levelmode=\\\"mp\\\" personaref=\\\"{2}\\\" gameid=\\\"{3}\\\" putinsquad=\\\"true\\\"></data>\" +clientport {4}";
        private static string m_executablePath = "C:\\Program Files (x86)\\Origin Games\\Battlefield 3\\bf3.exe";
        private static readonly string m_spCommandLineArgsFormat = "-webMode SP -Origin_NoAppFocus -onlineEnvironment prod -loginToken {0} -AuthToken {1} -requestState State_ResumeCampaign -requestStateParams \"<data levelmode=\"sp\" personaref=\"{2}\"></data>";

        // methods
        public static void ExecuteMP(string personaID, string serverId, string AuthToken, string LoginToken)
        {
            if (LoginToken.Length > 0 || AuthToken.Length > 0)
            {
                // execute the executable
                string arguments = string.Format(m_mpCommandLineArgsFormat, new object[]
                {
                    LoginToken,
                    AuthToken,
                    personaID,
                    serverId,
                    m_portRandom.Next(1, 65000)
                });
                try
                {
                    Process.Start(m_executablePath, arguments);
                }
                catch (Exception e)
                {
                    m_Log.ShowWindow();
                    m_Log.AddEvent("Exception Message: " + e.Message + " Exception StackTrace: " + e.StackTrace, "ExecuteMP Exception");
                }
            }
            else
            {
                throw new Exception("Tokens haven't been set.");
            }
        }

        public static bool ExecuteSP(string personaID, string AuthToken, string LoginToken)
        {
            bool flag = false;

            if (LoginToken.Length > 0 || AuthToken.Length > 0)
            {
                // execute the executable
                string arguments = string.Format(m_spCommandLineArgsFormat, new object[]
                {
                    LoginToken,
                    AuthToken,
                    personaID,
                });
                try
                {
                    process = Process.Start(m_executablePath, arguments);
                    flag = true;
                }
                catch (Exception e)
                {
                    m_Log.ShowWindow();
                    m_Log.AddEvent("Exception Message: " + e.Message + " Exception StackTrace: " + e.StackTrace, "ExecuteSP Exception");
                }
                return flag;
            }
            else
            {
                throw new Exception("Tokens haven't been set.");
            }
        }

        public static Process process { get; private set; }
    }
}

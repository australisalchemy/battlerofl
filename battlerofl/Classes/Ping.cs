using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace battlerofl
{
    static class PingUtil
    {
        public static string PingAddress(string ipaddr)
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send(ipaddr);

            long returnTime = reply.RoundtripTime + 10; // plus 10 overhead, since i have no idea what battlelog actually pings the servers with

            return returnTime.ToString();
        }
    }
}

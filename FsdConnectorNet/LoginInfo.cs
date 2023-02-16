using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FsdConnectorNet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LoginInfo
    {
        private string cid;
        private string password;
        private string callsign;
        private string realName;
        private PilotRating rating;
        private string hostName;
        private ushort port;
    
        public LoginInfo(string cid, string password, string callsign, string realName, PilotRating rating, string hostName, ushort port = 6809)
        {
            this.cid = cid;
            this.password = password;
            this.callsign = callsign;
            this.realName = realName;
            this.rating = rating;
            this.hostName = hostName;
            this.port = port;
        }
    }
}

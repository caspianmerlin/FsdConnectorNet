using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FsdConnectorNet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct LoginInfo
    {
        public string cid;
        public string password;
        public string callsign;
        public string realName;
        public PilotRatingType rating;
        public string hostName;
        public ushort port;
        public ProtocolRevision protocolRevision;
        public RadioFrequency radioFrequency;
    
        public LoginInfo(string cid, string password, string callsign, string realName, PilotRatingType rating, string hostName, ProtocolRevision protocolRevision, (ushort, ushort) radioFrequency, ushort port = 6809)
        {
            this.cid = cid;
            this.password = password;
            this.callsign = callsign;
            this.realName = realName;
            this.rating = rating;
            this.hostName = hostName;
            this.port = port;
            this.protocolRevision = protocolRevision;
            this.radioFrequency = new RadioFrequency()
            {
                left = radioFrequency.Item1,
                right = radioFrequency.Item2,
            };
        }
    }
}

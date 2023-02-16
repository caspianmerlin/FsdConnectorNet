using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FsdConnectorNet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ClientInfo
    {
        private string clientName;
        private ushort clientId;
        private string privateKey;
        private uint versionMajor;
        private uint versionMinor;
        private uint versionRevision;

        public ClientInfo(string clientName, ushort clientId, string privateKey, uint versionMajor, uint versionMinor, uint versionRevision)
        {
            this.clientName = clientName;
            this.clientId = clientId;
            this.privateKey = privateKey;
            this.versionMajor = versionMajor;
            this.versionMinor = versionMinor;
            this.versionRevision = versionRevision;
        }
    }

}

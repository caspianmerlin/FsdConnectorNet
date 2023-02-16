using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FsdConnectorNet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct PlaneInfo
    {
        private string equipment;
        private string airline;
        private string livery;

        public PlaneInfo(string equipment)
        {
            this.equipment = equipment;
            this.airline = "";
            this.livery = "";
        }

        public PlaneInfo(string equipment, string airline)
        {
            this.equipment = equipment;
            this.airline = airline;
            this.livery = "";
        }

        public PlaneInfo(string equipment, string airline, string livery)
        {
            this.equipment = equipment;
            this.airline = airline;
            this.livery = livery;
        }
    }
}

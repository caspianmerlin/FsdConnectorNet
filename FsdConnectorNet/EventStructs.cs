using System;
using System.Runtime.InteropServices;

namespace FsdConnectorNet
{

    [StructLayout(LayoutKind.Sequential)]
    public struct RadioFrequency
    {
        public ushort left;
        public ushort right;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct TwoStringStruct
    {
        public IntPtr stringA;
        public IntPtr stringB;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct FrequencyMessageFfi
    {
        public RadioFrequency freq;
        public IntPtr from;
        public IntPtr message;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FlightPlanMessageFfi
    {
        public IntPtr callsign;
        public FlightPlanFfi flightPlan;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FlightPlanFfi
    {
        public FlightRules flightRules;
        public IntPtr aircraftType;
        public ushort filedTas;
        public IntPtr origin;
        public ushort estDepTime;
        public ushort actDepTime;
        public uint cruiseLevel;
        public IntPtr destination;
        public byte hoursEnroute;
        public byte minutesEnroute;
        public byte hoursFuel;
        public byte minutesFuel;
        public IntPtr alternate;
        public IntPtr remarks;
        public IntPtr route;
    }



}
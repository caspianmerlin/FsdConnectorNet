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


}
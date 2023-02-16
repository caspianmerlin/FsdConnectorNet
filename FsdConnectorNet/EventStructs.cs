using System;
using System.Runtime.InteropServices;

namespace FsdConnectorNet
{

    [StructLayout(LayoutKind.Sequential)]
    struct RadioFrequency
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

    struct FrequencyMessageFfi
    {
        public RadioFrequency freq;
        public IntPtr from;
        public IntPtr message;
    }


}
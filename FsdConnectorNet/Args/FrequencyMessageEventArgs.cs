using System;
using System.Collections.Generic;
using System.Text;

namespace FsdConnectorNet.Args
{
    public class FrequencyMessageEventArgs : EventArgs
    {
        public (ushort, ushort) Frequency { get; set; }
        public string From { get; set; }
        public string Message { get; set; }

    }
}

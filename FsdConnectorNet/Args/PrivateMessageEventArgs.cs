using System;
using System.Collections.Generic;
using System.Text;

namespace FsdConnectorNet.Args
{
    public class PrivateMessageEventArgs : EventArgs
    {
        public string From { get; set; }
        public string Message { get; set; }

    }
}
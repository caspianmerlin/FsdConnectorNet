using System;
using System.Collections.Generic;
using System.Text;

namespace FsdConnectorNet
{
    public class AircraftEngine
    {
        public bool IsRunning { get; private set; }
        public bool IsReversing { get; private set; }

        public AircraftEngine(bool isRunning, bool isReversing) { this.IsRunning = isRunning; this.IsReversing = isReversing; }
    }
}

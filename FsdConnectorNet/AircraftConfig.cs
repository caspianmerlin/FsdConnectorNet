using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FsdConnectorNet
{

    [StructLayout(LayoutKind.Sequential)]
    public struct AircraftConfig
    {
        private bool strobeOn;
        private bool landingOn;
        private bool taxiOn;
        private bool beaconOn;
        private bool navOn;
        private bool logoOn;

        private bool gearDown;
        private int flapsPct;
        private bool spoilersOut;

        public bool onGround;

        private bool engine1Exists;
        private bool engine1On;
        private bool engine1Reversing;

        private bool engine2Exists;
        private bool engine2On;
        private bool engine2Reversing;

        private bool engine3Exists;
        private bool engine3On;
        private bool engine3Reversing;

        private bool engine4Exists;
        private bool engine4On;
        private bool engine4Reversing;

        public AircraftConfig(bool strobeOn, bool landingOn, bool taxiOn, bool beaconOn, bool navOn, bool logoOn, bool gearDown, int flapsPct, bool spoilersOut,
            bool onGround, AircraftEngine engineOne = null, AircraftEngine engineTwo = null, AircraftEngine engineThree = null, AircraftEngine engineFour = null)
        {
            this.strobeOn = strobeOn;
            this.landingOn= landingOn;
            this.taxiOn= taxiOn;
            this.beaconOn= beaconOn;
            this.navOn= navOn;
            this.logoOn= logoOn;
            this.gearDown= gearDown;
            this.flapsPct= flapsPct;
            this.spoilersOut= spoilersOut;
            this.onGround= onGround;
            this.engine1Exists = engineOne != null;
            this.engine2Exists = engineTwo != null;
            this.engine3Exists= engineThree != null;
            this.engine4Exists = engineFour != null;

            this.engine1On = false;
            this.engine2On = false;
            this.engine3On = false;
            this.engine4On = false;

            this.engine1Reversing = false;
            this.engine2Reversing = false;
            this.engine3Reversing = false;
            this.engine4Reversing = false;

            if (engine1Exists)
            {
                this.engine1On = engineOne.IsRunning;
                this.engine1Reversing = engineOne.IsReversing;
            }
            if (engine2Exists)
            {
                this.engine2On = engineTwo.IsRunning;
                this.engine2Reversing = engineTwo.IsReversing;
            }
            if (engine3Exists)
            {
                this.engine3On = engineThree.IsRunning;
                this.engine3Reversing = engineThree.IsReversing;
            }
            if (engine4Exists)
            {
                this.engine4On = engineFour.IsRunning;
                this.engine4Reversing = engineFour.IsReversing;
            }

        }
    }
}

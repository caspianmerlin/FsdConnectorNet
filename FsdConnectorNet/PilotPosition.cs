using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FsdConnectorNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PilotPosition
    {
        private TransponderMode transponderMode;
        private ushort squawkCode;
        private double latitude;
        private double longitude;
        private double trueAltitudeFt;
        private double altitudeAglFt;
        private double pressureAltitudeFt;
        private double groundSpeedKts;
        private double pitchDeg;
        private double bankDeg;
        private double headingDeg;
        private bool onGround;
        private double xVelocity;
        private double yVelocity;
        private double zVelocity;
        private double pitchRadPerSec;
        private double headingRadPerSec;
        private double bankRadPerSec;
        private double noseGearAngleDeg;

        public PilotPosition(TransponderModeType transponderMode, ushort squawkCode, double latitude, double longitude, double trueAltitudeFt, double altitudeAglFt, double pressureAltitudeFt,
            double groundSpeedKts, double pitchDeg, double bankDeg, double headingDeg, bool onGround, double xVelocity = 0.0, double yVelocity = 0.0, double zVelocity = 0.0,
            double pitchRadPerSec = 0.0, double headingRadPerSec = 0.0, double bankRadPerSec = 0.0, double noseGearAngleDeg = 0.0)
        {
            this.transponderMode = transponderMode;
            this.squawkCode = squawkCode;
            this.latitude = latitude;
            this.longitude = longitude;
            this.trueAltitudeFt = trueAltitudeFt;
            this.altitudeAglFt = altitudeAglFt;
            this.pressureAltitudeFt= pressureAltitudeFt;
            this.groundSpeedKts= groundSpeedKts;
            this.pitchDeg= pitchDeg;
            this.bankDeg= bankDeg;
            this.headingDeg= headingDeg;
            this.onGround= onGround;
            this.xVelocity= xVelocity;
            this.yVelocity= yVelocity;
            this.zVelocity= zVelocity;
            this.pitchRadPerSec= pitchRadPerSec;
            this.headingRadPerSec= headingRadPerSec;
            this.bankRadPerSec= bankRadPerSec;
            this.noseGearAngleDeg= noseGearAngleDeg;
        }
    }
}

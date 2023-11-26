using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;

namespace FsdConnectorNet
{


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct FlightPlan
    {
        [JsonInclude]
        public FlightRules flightRules;
        [JsonInclude]
        public string aircraftType;
        [JsonInclude]
        public ushort filedTas;
        [JsonInclude]
        public string origin;
        [JsonInclude]
        public ushort estimatedDepTime;
        [JsonInclude]
        public ushort actualDepTime;
        [JsonInclude]
        public uint cruiseLevel;
        [JsonInclude]
        public string destination;
        [JsonInclude]
        public byte hoursEnroute;
        [JsonInclude]
        public byte minutesEnroute;
        [JsonInclude]
        public byte hoursFuel;
        [JsonInclude]
        public byte minutesFuel;
        [JsonInclude]
        public string alternate;
        [JsonInclude]
        public string remarks;
        [JsonInclude]
        public string route;


        public static FlightPlan FromFfiStruct(ref FlightPlanFfi ffi)
        {
            return new FlightPlan()
            {
                flightRules = ffi.flightRules,
                aircraftType = Marshal.PtrToStringAnsi(ffi.aircraftType),
                filedTas = ffi.filedTas,
                origin = Marshal.PtrToStringAnsi(ffi.origin),
                estimatedDepTime = ffi.estDepTime,
                actualDepTime = ffi.actDepTime,
                cruiseLevel = ffi.cruiseLevel,
                destination = Marshal.PtrToStringAnsi(ffi.destination),
                hoursEnroute = ffi.hoursEnroute,
                minutesEnroute = ffi.minutesEnroute,
                hoursFuel = ffi.hoursFuel,
                minutesFuel = ffi.minutesFuel,
                alternate = Marshal.PtrToStringAnsi(ffi.alternate),
                remarks = Marshal.PtrToStringAnsi(ffi.remarks),
                route = Marshal.PtrToStringAnsi(ffi.route),
            };
        }

        public static FlightPlan ParseFromEsScenarioFile(string fpString)
        {
            //$FPEZY75MX : *A : I : A320 : 420 : LEMG : : : 34000 : EGAA : 00 : 00 : 0 : 0 : : : BLN3C BLN UN865 DELOG SALCO BHD STU DUB N34 BELZU
            var fields = fpString.Split(':');
            if (fields.Length < 17)
            {
                throw new FlightPlanException(FlightPlanErrorType.NotEnoughFields);
            }

            if (fields[0].Length > 10)
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidCallsign);
            }

            string callsign = fields[0].Substring(3).ToUpperInvariant();

            FlightRules flightRules;
            switch (fields[2])
            {
                case "I":
                    flightRules = FlightRules.IFR;
                    break;
                case "V":
                    flightRules = FlightRules.VFR;
                    break;
                case "D":
                    flightRules = FlightRules.DVFR;
                    break;
                case "S":
                    flightRules = FlightRules.SVFR;
                    break;
                default:
                    throw new FlightPlanException(FlightPlanErrorType.InvalidFlightRules);
            }

            if (fields[3].Length < 2)
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidAircraftType);
            }
            string aircraftType = fields[3].ToUpperInvariant();

            ushort filedTas = 0;
            if (!ushort.TryParse(fields[4], out filedTas))
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidTrueAirspeed);
            }

            if (fields[5].Length < 3 || fields[5].Length > 4)
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidAirportCode);
            }

            string origin = fields[5].ToUpperInvariant();

            ushort etd = 0, atd = 0;
            if (fields[6].Length == 0)
            {
                fields[6] = "0";
            }
            if (fields[7].Length == 0)
            {
                fields[7] = "0";
            }
            if (!ushort.TryParse(fields[6], out etd))
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidTime);
            }
            if (!ushort.TryParse(fields[7], out atd))
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidTime);
            }

            uint cruiseLevel;
            bool flightLevel = fields[8].StartsWith("FL");
            string cruiseLevelString;
            if (flightLevel) { 
                if (fields[8].Length < 4)
                {
                    throw new FlightPlanException(FlightPlanErrorType.InvalidCruiseLevel);
                }
                cruiseLevelString = fields[8].Substring(2);
            } else
            {
                cruiseLevelString = fields[8];
            }
            if (!uint.TryParse(cruiseLevelString, out cruiseLevel))
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidCruiseLevel);
            }
            if (flightLevel)
            {
                cruiseLevel *= 100;
            }

            if (fields[9].Length < 3 || fields[9].Length > 4)
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidAirportCode);
            }

            string destination = fields[9].ToUpperInvariant();


            byte hoursEnr, minsEnr, hoursFuel, minsFuel;
            for (int i = 10; i < 14; i++)
            {
                if (fields[i].Length == 0)
                {
                    fields[i] = "0";
                }
            }
            if (!byte.TryParse(fields[10], out hoursEnr))
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidTime);
            }
            if (!byte.TryParse(fields[11], out minsEnr))
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidTime);
            }
            if (!byte.TryParse(fields[12], out hoursFuel))
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidTime);
            }
            if (!byte.TryParse(fields[13], out minsFuel))
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidTime);
            }
            if (minsEnr > 59 || minsFuel > 59)
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidTime);
            }

            if (fields[14].Length > 4)
            {
                throw new FlightPlanException(FlightPlanErrorType.InvalidAirportCode);
            }

            string alternate = fields[14].ToUpperInvariant();
            string remarks = fields[15].ToUpperInvariant();
            string route = fields[16].ToUpperInvariant();


            return new FlightPlan()
            {
                flightRules = flightRules,
                aircraftType = aircraftType,
                filedTas = filedTas,
                origin = origin,
                estimatedDepTime = etd,
                actualDepTime = atd,
                cruiseLevel = cruiseLevel,
                destination = destination,
                hoursEnroute = hoursEnr,
                minutesEnroute = minsEnr,
                hoursFuel = hoursFuel,
                minutesFuel = minsFuel,
                alternate = alternate,
                remarks = remarks,
                route = route,

            };

        }

    }
}

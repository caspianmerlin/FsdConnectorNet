using System;
using System.Collections.Generic;
using System.Text;

namespace FsdConnectorNet
{
    public class FlightPlanException : Exception
    {
        public FlightPlanErrorType ErrorType;
        public FlightPlanException()
        {
            this.ErrorType = FlightPlanErrorType.Unknown;
        }

        public FlightPlanException(string message) : base(message)
        {
            this.ErrorType = FlightPlanErrorType.Unknown;
        }

        public FlightPlanException(string message, Exception innerException) : base(message, innerException)
        {
            this.ErrorType = FlightPlanErrorType.Unknown;
        }

        public FlightPlanException(FlightPlanErrorType errorType)
        {
            this.ErrorType = errorType;
        }
    }
}

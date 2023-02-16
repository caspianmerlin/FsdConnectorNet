
namespace FsdConnectorNet
{
    public enum PilotRatingType
    {
        Student = 1,
        VFR,
        IFR,
        Instructor,
        Supervisor,
    }

    public enum TransponderModeType
    {
        Standby,
        ModeC,
        Ident,
    }


    public enum DisconnectionReason
    {
        Unknown,
        NetworkError,
        DisconnectRequestedByUser,
        AlreadyConnected,
        ServerAuthenticationError,
        TimedOut,
        Killed,
        CallsignInUse,
        InvalidCallsign,
        AlreadyRegistered
    }

}
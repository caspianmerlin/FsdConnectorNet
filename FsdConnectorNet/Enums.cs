
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

    public enum ProtocolRevision
    {
        /// Used on legacy FSD servers. If the FSD server is a privately run one, it is most likely using this version
        Classic = 9,
        /// Deprecated - used on VATSIM prior to the introduction of client authentication
        VatsimNoAuth = 10,
        /// Used on VATSIM servers until 2022
        VatsimAuth = 100,
        /// VATSIM Velocity - used on VATSIM servers since 2022
        Vatsim2022 = 101,
    }

}



enum EventType
{
    None,
    Connected,
    SyntaxError,
    InvalidSourceCallsign,
    NoSuchCallsign, //String
    NoFlightPlan, //String
    NoWeatherProfile, //String
    InvalidControl,
    OtherFsdError, //String
    ServerHeartbeat,

    TextMessage, // 2 strings
    FlightPlanReceived,
    FrequencyMessage,
    AtcChannelMessage,
    BroadcastMessage,
    ServerMessage,

    //Disconnection reasons
    Unknown,
    JwtServerError,
    NetworkError, // 1 String
    DisconnectRequestedByUser,
    UnauthorisedServer,
    ServerAuthTimeout,
    TimedOut,
    InvalidHostname,
    Killed, //String, String
    CallsignInUse,



    // Fatal FSD errors
    InvalidCidPassword,
    InvalidProtocolRevision,
    RequestedLevelTooHigh,
    ServerFull,
    CertificateSuspended,
    InvalidPositionForRating,
    UnauthorisedClient,
    AuthTimeOut,
    Other,
    InvalidCallsign,
    AlreadyRegistered,
    //String
}
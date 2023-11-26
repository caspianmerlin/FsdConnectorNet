using FsdConnectorNet.Args;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace FsdConnectorNet
{
    public class Connection : IDisposable
    {
        // FFI stuff

        private IntPtr _connectionHandle;
        private Stopwatch _stopwatch;

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr connection_new();

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void connection_connect(IntPtr ptr, ClientInfo clientInfo, LoginInfo loginInfo, PilotPosition aircraftPosition, AircraftConfig aircraftConfig, PlaneInfo planeInfo);


        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void update_position(IntPtr ptr, PilotPosition position);


        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool is_connected(IntPtr ptr);

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_string(IntPtr ptr);

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern TwoStringStruct get_two_string_struct(IntPtr ptr);

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern FrequencyMessageFfi get_freq_msg_struct(IntPtr ptr);

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool connection_free(IntPtr ptr);

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr poll_events(IntPtr ptr, ref int eventType);

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void send_frequency_message(IntPtr ptr, RadioFrequency frequency, [MarshalAs(UnmanagedType.LPStr)] string message);

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void send_flight_plan(IntPtr ptr, FlightPlan flightPlan);
        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void request_flight_plan(IntPtr ptr);

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern FlightPlanMessageFfi get_flight_plan_message(IntPtr ptr);

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_flight_plan_message_struct(FlightPlanMessageFfi message);

        [DllImport("pilot_client_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void send_private_message(IntPtr ptr, [MarshalAs(UnmanagedType.LPStr)] string recipient, [MarshalAs(UnmanagedType.LPStr)] string message);



        private CancellationTokenSource _cts;

        private bool disposedValue;
        private LoginInfo? _loginInfo;

        // Event handlers

        // Connection status
        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public FlightPlan? CurrentFlightPlan { get; private set; }


        public event EventHandler<FrequencyMessageEventArgs> FrequencyMessageReceived;
        public event EventHandler<PrivateMessageEventArgs> PrivateMessageReceived;






        public bool IsConnected
        {
            get
            {
                return is_connected(this._connectionHandle);
                
            }
        }

        public Connection()
        {
            _cts = new CancellationTokenSource();
            this._connectionHandle= connection_new();
            this._stopwatch = Stopwatch.StartNew();
        }

        public void Connect(ClientInfo clientInfo, LoginInfo loginInfo, PilotPosition aircraftPosition, AircraftConfig aircraftConfig, PlaneInfo planeInfo)
        {
            if (IsConnected) { return; }
            _loginInfo = loginInfo;
            connection_connect(this._connectionHandle, clientInfo, loginInfo, aircraftPosition, aircraftConfig, planeInfo);
            ThreadPool.QueueUserWorkItem(new WaitCallback(PollThread), _cts.Token);
        }

        public void UpdatePosition(PilotPosition position)
        {
            update_position(this._connectionHandle, position);
        }

        public void SendFrequencyMessage((ushort, ushort) frequency, string message)
        {
            RadioFrequency freq = new RadioFrequency()
            {
                left = frequency.Item1,
                right = frequency.Item2,
            };
            send_frequency_message(this._connectionHandle, freq, message);
        }

        public void SendPrivateMessage(string to, string message)
        {
            send_private_message(this._connectionHandle, to, message);
        }

        public void SendFlightPlan(FlightPlan? flightPlan)
        {
            if (flightPlan.HasValue)
            {
                this.CurrentFlightPlan = flightPlan;
                send_flight_plan(this._connectionHandle, flightPlan.Value);
            }
        }



        private void PollThread(object obj)
        {
            CancellationToken token = (CancellationToken)obj;

            while (!token.IsCancellationRequested)
            {
                if (this._stopwatch.ElapsedMilliseconds > 2000) {
                    request_flight_plan(this._connectionHandle);
                    this._stopwatch.Restart();
                }
                bool shouldDisconnect = false;
                int eventInt = 0;
                IntPtr ptr = poll_events(this._connectionHandle, ref eventInt);
                EventType eventType = (EventType)eventInt;

                switch (eventType)
                {
                    case EventType.Connected:
                        Connected?.Invoke(this, EventArgs.Empty);
                        break;
                    case EventType.NetworkError:
                        shouldDisconnect= true;
                        Disconnected?.Invoke(this, EventArgs.Empty);
                        free_string(ptr);
                        break;
                    case EventType.Killed:
                        shouldDisconnect = true;
                        Disconnected?.Invoke(this, EventArgs.Empty);
                        TwoStringStruct tts = get_two_string_struct(ptr);
                        free_string(tts.stringA);
                        free_string(tts.stringB);
                        break;



                    case EventType.CallsignInUse:
                    case EventType.InvalidCallsign:
                    case EventType.AlreadyRegistered:
                    case EventType.InvalidCidPassword:
                    case EventType.InvalidProtocolRevision:
                    case EventType.RequestedLevelTooHigh:
                    case EventType.ServerFull:
                    case EventType.CertificateSuspended:
                    case EventType.InvalidPositionForRating:
                    case EventType.UnauthorisedClient:
                    case EventType.AuthTimeOut:
                    case EventType.Unknown:
                    case EventType.JwtServerError:
                    case EventType.DisconnectRequestedByUser:
                    case EventType.UnauthorisedServer:
                    case EventType.ServerAuthTimeout:
                    case EventType.TimedOut:
                    case EventType.InvalidHostname:
                        shouldDisconnect = true;
                        Disconnected?.Invoke(this, EventArgs.Empty);
                        break;
                    case EventType.SyntaxError:
                    case EventType.InvalidSourceCallsign:
                    case EventType.InvalidControl:
                        break;
                    case EventType.NoSuchCallsign:
                        free_string(ptr);
                        break;
                    case EventType.NoFlightPlan:
                        free_string(ptr);
                        break;
                    case EventType.NoWeatherProfile:
                        free_string(ptr);
                        break;
                    case EventType.OtherFsdError:
                        free_string(ptr);
                        break;
                    case EventType.ServerHeartbeat:
                        break;
                    case EventType.FlightPlanReceived:
                        FlightPlanMessageFfi flightPlanMessageFfi = get_flight_plan_message(ptr);
                        string callsign = Marshal.PtrToStringAnsi(flightPlanMessageFfi.callsign);
                        FlightPlan flightPlan = FlightPlan.FromFfiStruct(ref flightPlanMessageFfi.flightPlan);
                        free_flight_plan_message_struct(flightPlanMessageFfi);
                        if (callsign == this._loginInfo?.callsign)
                        {
                            this.CurrentFlightPlan = flightPlan;
                        }
                        break;
                    case EventType.TextMessage:
                        TwoStringStruct tts2 = get_two_string_struct(ptr);
                        string from_a= Marshal.PtrToStringAnsi(tts2.stringA);
                        string message_a = Marshal.PtrToStringAnsi(tts2.stringB);
                        PrivateMessageEventArgs privateMessageEventArgs = new PrivateMessageEventArgs()
                        {
                            From = from_a,
                            Message = message_a,
                        };
                        free_string(tts2.stringA);
                        free_string(tts2.stringB);
                        PrivateMessageReceived?.Invoke(this, privateMessageEventArgs);
                        break;
                    case EventType.FrequencyMessage:
                        FrequencyMessageFfi freqMsg = get_freq_msg_struct(ptr);
                        string from = Marshal.PtrToStringAnsi(freqMsg.from);
                        string message = Marshal.PtrToStringAnsi(freqMsg.message);
                        FrequencyMessageEventArgs freqMsgeventArgs = new FrequencyMessageEventArgs()
                        {
                            Frequency = (freqMsg.freq.left, freqMsg.freq.right),
                            From = from,
                            Message = message,
                        };
                        free_string(freqMsg.from);
                        free_string(freqMsg.message);
                        FrequencyMessageReceived?.Invoke(this, freqMsgeventArgs);
                        break;
                    case EventType.AtcChannelMessage:
                        TwoStringStruct atcChannelMsg = get_two_string_struct(ptr);
                        free_string(atcChannelMsg.stringA);
                        free_string(atcChannelMsg.stringB);
                        break;
                    case EventType.BroadcastMessage:
                        TwoStringStruct broadcastMsg = get_two_string_struct(ptr);
                        free_string(broadcastMsg.stringA);
                        free_string(broadcastMsg.stringB);
                        break;
                    case EventType.ServerMessage:
                        free_string(ptr);
                        break;
                }
                if (shouldDisconnect)
                {
                    break;
                }
                Thread.Sleep(50);
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cts.Cancel();
                    _cts.Dispose();
                    // TODO: dispose managed state (managed objects)
                }
                connection_free(this._connectionHandle);
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Connection()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

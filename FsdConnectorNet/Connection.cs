using System;
using System.Runtime.InteropServices;

namespace FsdConnectorNet
{
    public class Connection
    {
        // FFI stuff

        private IntPtr _connectionHandle;

        [DllImport("fsd_connector_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr new_connection();

        [DllImport("fsd_connector_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern int connecto(IntPtr ptr, ClientInfo clientInfo, LoginInfo loginInfo, PilotPosition aircraftPosition, AircraftConfig aircraftConfig, PlaneInfo planeInfo);

        [DllImport("fsd_connector_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_connection(IntPtr ptr);

        [DllImport("fsd_connector_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern void update_position(IntPtr ptr, PilotPosition position);

        
        [DllImport("fsd_connector_ffi", CallingConvention = CallingConvention.Cdecl)]
        private static extern int is_connected(IntPtr ptr);



        // Event handlers

        // Connection status
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler ConnectionFailed;
        public event EventHandler Killed;

        // Messages and flight plans
        public event EventHandler PrivateMessageReceived;
        public event EventHandler FrequencyMessageReceived;
        public event EventHandler BroadcastMessageReceived;
        public event EventHandler AtcChannelMessageReceived;
        public event EventHandler ServerMessageReceived;
        public event EventHandler FlightPlanReceived;

        public event EventHandler<String> NoFlightPlanFound;
        public event EventHandler<String> NoWeatherProfile;






        public bool IsConnected
        {
            get
            {
                return is_connected(this._connectionHandle) == 1;
                
            }
        }

        public Connection()
        {
            this._connectionHandle= new_connection();
        }

        public bool Connect(ClientInfo clientInfo, LoginInfo loginInfo, PilotPosition aircraftPosition, AircraftConfig aircraftConfig, PlaneInfo planeInfo)
        {

            return connecto(this._connectionHandle, clientInfo, loginInfo, aircraftPosition, aircraftConfig, planeInfo) == 1;

        }

        public void UpdatePosition(PilotPosition position)
        {

            update_position(this._connectionHandle, position);

        }

        ~Connection()
        {
            free_connection(this._connectionHandle);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceListenerServices.Services
{
    class ConnectSingleton
    {
        private static ConnectSingleton instance;
        public static bool IsConnected = false;

        private ConnectSingleton(bool isConnected)
        { IsConnected = isConnected; }

        public static ConnectSingleton getInstance(bool IsConnected)
        {
            if (instance == null)
                instance = new ConnectSingleton(IsConnected);
            return instance;
        }
    }
}

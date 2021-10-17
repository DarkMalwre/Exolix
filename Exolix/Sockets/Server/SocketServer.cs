using Exolix.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Exolix.Sockets.Server
{
    public class ServerNodeItem
    {
        public string User = "";

        public string Key = "";

        public string Host = "";

        public string? Port = null;
    }

    public class SocketServerSettings
    {
        public string Host = "localhost";

        public int? Port = null;

        public bool Secure = false;

        public ServerNodeItem[]? NodeList = null;
    }

    public class SocketServer : ServerEvents
    {
        private SocketServerSettings? Settings;
        private bool Running = false;
        private WebSocketServer? Server = null;

        /// <summary>
        ///     Create fast and easy to use TCP sockets
        /// </summary>
        /// <param name="settings">Settings for the server</param>
        public SocketServer(SocketServerSettings? settings = null)
        {
           if (settings == null)
           {
                Settings = new SocketServerSettings();
                return;
           }

            Settings = settings;
        }

        private void RunThreadLogic()
        {
            string prefix = "ws://";
            if (Settings!.Secure)
            {
                prefix = "wss://";
            }

            string suffix = "";
            if (Settings!.Port != null)
            {
                suffix = ":" + Settings!.Port;
            }

            string serverUrl = prefix + Settings!.Host + suffix;

            var server = new WebSocketServer(serverUrl);
            Server = server;
            server.AddWebSocketService("/", () => {
                CoreServerConnection coreConnection = new CoreServerConnection();
                coreConnection.Server = this;

                return coreConnection;
            });

            server.Start();
            Logger.KeepAlive(true);
        }

        public void Run()
        {
            if (Running)
            {
                throw new Exception("Server is already running");
            }

            Running = true;
            new Thread(new ThreadStart(RunThreadLogic)).Start();
        }

        public void Stop()
        {
            if (!Running)
            {
                throw new Exception("Server is not running");
            }

            Running = false;
            Server?.Stop();
            Logger.KeepAlive(false);
        }
    }

    public class EventCoreCommander { 
    }
}
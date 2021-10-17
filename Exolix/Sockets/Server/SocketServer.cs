using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Exolix.Sockets.Server
{
    public class SocketServerSettings
    {
        public string Host = "localhost";

        public int? Port = 80;

        public bool Secure = false;
    }

    public class SocketServer : Events
    {
        public SocketServerSettings? Settings;

        public SocketServer(SocketServerSettings? settings = null)
        {
           if (settings != null)
           {
                Settings = new SocketServerSettings();
                return;
           }

            Settings = settings;
        }

        private void RunThreadLogic()
        {
            Console.WriteLine($"{(Settings!.Secure ? "wss://" : "ws://")}{Settings.Host}{(Settings.Port != null ? ":" + Settings.Port : "")}");
            var server = new WebSocketServer($"{(Settings!.Secure ? "wss://" : "ws://")}{Settings.Host}{(Settings.Port != null ? ":" + Settings.Port : "")}");
            server.AddWebSocketService("/", () => {
                CoreServerConnection coreConnection = new CoreServerConnection();
                coreConnection.Server = this;

                return coreConnection;
            });

            server.Start();
            Console.ReadKey(true);
        }

        public void Run()
        {
            Thread thread = new Thread(new ThreadStart(RunThreadLogic));
            thread.Start();
        }
    }

    public class EventCoreCommander { 
    }
}
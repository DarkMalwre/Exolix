using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Exolix.Sockets.Server
{
    public class SocketServer : Events
    {
        public SocketServer()
        {
           
        }

        public void Run()
        {
            var server = new WebSocketServer("ws://localhost:8090");
            server.AddWebSocketService<CoreServerConnection>("/");

            server.Start();

            Console.ReadKey(true);
        }
    }
}
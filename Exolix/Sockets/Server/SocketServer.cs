﻿using System;
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

        private void RunThreadLogic()
        {
            var server = new WebSocketServer("ws://localhost:8090");
            server.AddWebSocketService("/", () => {
                CoreServerConnection coreConnection = new CoreServerConnection();
                coreConnection.Server = this;

                return coreConnection;
                ;
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
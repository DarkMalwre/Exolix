﻿using Exolix.Json;
using Newtonsoft.Json;
using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Exolix.Sockets.Server
{
    public class ServerConnection : ConnectionEvents
    {
        public string ID = "";

        public ServerConnection(CoreServerConnection coreConnection)
        {
            ID = coreConnection.ID;
        }
    }

    public class ConnectionMessage
    {
        public string? Channel
        {
            get;
            set;
        }

        public string? Data
        {
            get;
            set;
        }
    }

    public class CoreServerConnection: WebSocketBehavior
    {
        public SocketServer? Server;
        public ServerConnection? Connection;

        protected override void OnOpen()
        {
            ServerConnection connection = new ServerConnection(this);
            Connection = connection;

            foreach (var eventAction in Server!.OnOpenEvents.ToArray())
            {
                eventAction(connection);
            }
        }

        protected override void OnMessage(MessageEventArgs messageEvent)
        {
            try
            {
                ConnectionMessage parsedMessage = JsonHandler.Parse<ConnectionMessage>(messageEvent.Data);

                if (parsedMessage.Data != null && parsedMessage.Channel != null)
                {
                    foreach (var eventTuple in Connection!.OnMessageEvents.ToArray())
                    {
                        if (parsedMessage.Channel == eventTuple.Item2)
                        {
                            if (parsedMessage.Data is string)
                            {
                                eventTuple.Item1(parsedMessage.Data);
                            }
                        }
                    }
                }
            } catch (Exception error)
            {
                Console.Error.WriteLine(error.ToString().Pastel("#ff0055"));
                Sessions.CloseSession(ID);
            }
        }
    }
}

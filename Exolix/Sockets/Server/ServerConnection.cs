using Exolix.Json;
using Newtonsoft.Json;
using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.NetCore;
using WebSocketSharp.NetCore.Server;

namespace Exolix.Sockets.Server
{
    public class ServerConnectionMessage
    {
        public string? Channel = null;
        public string? Data = null;
    }

    public class ServerConnection : ConnectionEvents
    {
        public string ID = "";
        public IPAddress RemoteAddress;
        private CoreServerConnection CoreConnection;

        public ServerConnection(CoreServerConnection coreConnection)
        {
            RemoteAddress = coreConnection.Context.UserEndPoint.Address;
            ID = coreConnection.ID;
            CoreConnection = coreConnection;
        }

        public void Send<MessageType>(string channel, MessageType message)
        {
            try
            {
                string parsedMessage = JsonHandler.Stringify(message);
                CoreConnection.SendMessage(JsonHandler.Stringify(new ServerConnectionMessage
                {
                    Channel = channel,
                    Data = parsedMessage
                }));
            } catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void Close()
        {
            CoreConnection.CloseSession();
        }
    }

    public class ConnectionMessage
    {
        public string? Channel;

        public string? Data;
    }

    public class CoreServerConnection: WebSocketBehavior
    {
        public SocketServer? Server;
        public ServerConnection? Connection;

        protected override void OnClose(CloseEventArgs e)
        {
            foreach (var eventAction in Connection!.OnCloseEvents.ToArray())
            {
                eventAction();
            }
        }

        protected override void OnOpen()
        {
            ServerConnection connection = new ServerConnection(this);
            Connection = connection;

            foreach (var eventAction in Server!.OnOpenEvents.ToArray())
            {
                new Thread(new ThreadStart(() => { eventAction(connection); })).Start();
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
                                new Thread(new ThreadStart(() => { eventTuple.Item1(parsedMessage.Data); })).Start();
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

        public void SendMessage(string message)
        {
            Send(message);
        }

        public void CloseSession()
        {
            Sessions.CloseSession(ID);
        }
    }
}

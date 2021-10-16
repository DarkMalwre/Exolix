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

    public interface ConnectionMessage
    {
        string Channel
        {
            get;
        }

        string Data
        {
            get;
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
                ConnectionMessage parsedMessage = JsonDocument.Parse(messageEvent.Data);
                ConnectionMessage! parsedMessageNew = parsedMessage as ConnectionMessage;

                foreach (var eventTuple in Connection!.OnMessageEvents.ToArray())
                {

                    if (parsedMessageNew != null & parsedMessageNew?.Data != null & parsedMessageNew?.Channel != null)
                    {
                        eventTuple.Item1(parsedMessageNew!.Data.ToString());
                    }
                }
            } catch (Exception error)
            {
                Console.Error.WriteLine(error.ToString());
            }
        }
    }
}

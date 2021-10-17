using Exolix.Json;
using Exolix.Sockets.Server;
using Exolix.Terminal;
using Pastel;
using System;
using System.Net;
using System.Net.Sockets;
using WebSocketSharp.Server;

namespace ExolixTests
{
    public class MessageType
    {
        public string? Msg = "No Message Provided";
    }

    public class Tests
    {
        public class DemoMessage
        {
            public string? Message;
        }

        public static void Main(string[] args)
        {
            Animation.Start("Hosting new server");
            SocketServer server = new SocketServer(new SocketServerSettings
            {
                Port = 80
            });

            server.OnOpen((connection) =>
            {
                Logger.Info(" New Connection");

                connection.OnMessage("main", (msg) =>
                {
                    MessageType message = JsonHandler.Parse<MessageType>(msg);
                    Logger.Info(" Message: " + message.Msg);
                });
            });

            server.Run();
            Animation.Stop();
        }
    }

    public class HandleService: WebSocketBehavior {
        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            Logger.PrintDynamic("MSG: " + e.Data);
        }

        protected override void OnOpen()
        {
            Logger.PrintDynamic("New Conn: " + Context.QueryString["name"]);
        }
    }
}
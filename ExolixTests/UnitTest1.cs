using Exolix.Json;
using Exolix.Sockets.Client;
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
            Logger.Info("Starting API server");
            SocketServer server = new SocketServer(new SocketServerSettings
            {
                Port = 80
            });

            server.OnOpen((connection) =>
            {
                Logger.Info(" + New Connection");

                connection.OnMessage("main", (msg) =>
                {
                    MessageType message = JsonHandler.Parse<MessageType>(msg);
                    Logger.Info(" Message: " + message.Msg);
                    connection.Send("main", new MessageType
                    {
                        Msg = message.Msg,
                    });
                });

                connection.OnClose(() =>
                {
                    Logger.Info("- Connection Closed");
                });

                connection.Send("main", new MessageType
                {
                    Msg = "Heyy"
                });
            });

            server.Run();

            Logger.Success("Server started at port 80");

            //System.Threading.Thread.Sleep(500);

            Logger.Info("Connecting to server via client");

            SocketClient client = new SocketClient(new SocketClientSettings
            {
                Port = 80
            });

            client.OnOpen(() =>
            {
                Logger.Info("[ Client ] Connected to server");
            });

            client.OnMessage("main", (msg) =>
            {
                Logger.Info("[ Client ] Message: " + msg.TrimStart('"').TrimEnd('"'));
                MessageType message = JsonHandler.Parse<MessageType>(msg.TrimStart('"').TrimEnd('"'));
                Logger.Info("[ Client ] Message: " + message.Msg);
            });

            client.Run();

            Logger.Info("Done with actions");
        }

        public static void DoException(SocketServer server)
        {
            DoExceptionSec(server);
        }

        public static void DoExceptionSec(SocketServer server)
        {
            try
            {
                server.Run();
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex);
            }
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
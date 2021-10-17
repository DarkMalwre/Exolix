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
    public interface Typess
    {
        string Msg
        {
            get;
        }
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
            SocketServer server = new SocketServer();

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
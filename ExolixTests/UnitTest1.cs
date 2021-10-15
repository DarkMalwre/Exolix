using Exolix.Terminal;
using System;
using System.Net;
using System.Net.Sockets;
using WebSocketSharp.Server;

namespace ExolixTests
{
    public class Tests
    {
        public static void Main(string[] args)
        {
            Logger.PrintDynamic("hey\n");
            Logger.PrintDynamic("SErver\n");

            var server = new WebSocketServer("ws://localhost:8090");
            server.AddWebSocketService<HandleService>("/");

            server.Start();
            Console.ReadKey(true);

            server.Stop();
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
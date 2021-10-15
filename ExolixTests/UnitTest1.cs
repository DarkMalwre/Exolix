using Exolix.Sockets.Server;
using Exolix.Terminal;
using Pastel;
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
            Logger.PrintLine("Exolix".Pastel("#60cdff") + " Scratch Test | Socket Server".Pastel("#ffffff"));
            Logger.PrintLine("───────────────────────────────────".Pastel("#555555"));

            SocketServer server = new SocketServer();

            server.Run();
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
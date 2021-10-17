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
            Animation.Start("Connecting to Ethermine with query".Pastel("#999999") + " host=ethermine.net port=5555 protocol=TCP".Pastel("#ffffff"));
            System.Threading.Thread.Sleep(500);

            Animation.Stop("Successfully conected to Ethermine server");
            Animation.Start("Authenticating client tokens");

            System.Threading.Thread.Sleep(500);
            Animation.Stop("All tokens have been accepted by Ethermine");

            Animation.Start("Checking system block handlers");
            System.Threading.Thread.Sleep(500);

            Animation.Stop("Block handlers are ready");

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                Animation.Start("Looking for block");
                System.Threading.Thread.Sleep(1000);

                Animation.Stop("Finished mining through block");
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
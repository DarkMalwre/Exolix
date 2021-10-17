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
            Logger.PrintLineDynamic("Exolix".Pastel("#60cdff") + " Scratch | Socket Server".Pastel("#ffffff"));
            Logger.PrintLineDynamic("───────────────────────────────────────".Pastel("#555555"));

            SocketServer server = new SocketServer();

            server.OnOpen((conn) =>
            {
                Logger.PrintLineDynamic("[Test] New connection!");

                conn.OnMessage("max", (msg) =>
                {
                    DemoMessage message = JsonHandler.Parse<DemoMessage>(msg);
                    Logger.PrintLineDynamic($"[Test] New message! Message = {message.Message}");
                });
            });

            Animation.Start("Loading the ISS", new AnimationSettings());

            System.Threading.Thread.Sleep(300);
            Animation.Stop("Failed to load the ISS", "error");

            Animation.Start("Loading the ISS", new AnimationSettings());

            System.Threading.Thread.Sleep(1000);
            Animation.Stop("Failed to load the ISS", "warning");

            Animation.Start("Loading the ISS", new AnimationSettings());

            System.Threading.Thread.Sleep(1000);
            Animation.Stop("Successfuly loaded the ISS", "success");
            Animation.Start("Loading the ISS", new AnimationSettings());

            System.Threading.Thread.Sleep(1000);
            Animation.Stop("Failed to load the ISS", "error");

            //server.Run();

            Logger.AppDone();
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
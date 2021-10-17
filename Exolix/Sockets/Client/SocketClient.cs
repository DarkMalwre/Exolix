using Exolix.Json;
using Exolix.Sockets.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace Exolix.Sockets.Client
{
    public class SocketClientSettings
    {
        public string Host = "localhost";

        public bool Secure = false;

        public int? Port = null;
    }

    public class SocketClient : ClientEvents
    {
        private WebSocket? Socket;
        private SocketClientSettings Settings;
        private bool IsConnected = false;

        public SocketClient(SocketClientSettings? settings = null)
        {
            if (settings == null)
            {
                Settings = new SocketClientSettings();
                return;
            }

            Settings = settings;
        }

        private void RunThreadLogic()
        {
            string prefix = "ws://";
            if (Settings!.Secure)
            {
                prefix = "wss://";
            }

            string suffix = "";
            if (Settings!.Port != null)
            {
                suffix = ":" + Settings!.Port;
            }

            string serverUrl = prefix + Settings!.Host + suffix;
            Socket = new WebSocket(serverUrl);

            Socket.OnOpen += (sender, e) =>
            {
                foreach (var eventCallback in OnOpenEvents)
                {
                    eventCallback();
                }
            };

            Socket.OnMessage += (sender, e) =>
            {
                try
                {
                    ConnectionMessage parsedMessage = JsonHandler.Parse<ConnectionMessage>(e.Data);

                    if (parsedMessage.Channel != null && parsedMessage.Data != null && parsedMessage.Data is string)
                    {
                        foreach (var eventTuple in OnMessageEvents)
                        {
                            if (eventTuple.Item2 == parsedMessage.Channel) {
                                eventTuple.Item1(JsonHandler.Stringify(parsedMessage.Data));
                            }
                        }
                    }
                } catch (Exception ex)
                {
                    Terminal.Logger.ErrorException(ex);
                }
            };

            Socket.Connect();
            //Socket.Send(JsonHandler.Stringify(new
            //{
            //    Channel = "main",
            //    Data = JsonHandler.Stringify(new
            //    {
            //        Msg = "Hey server"
            //    })
            //}));
        }

        public void Run()
        {
            if (IsConnected)
            {
                throw new Exception("Client is already connected");
            }

            IsConnected = true;
            new Thread(new ThreadStart(RunThreadLogic)).Start();
            Terminal.Logger.KeepAlive(true);
        }
    }
}

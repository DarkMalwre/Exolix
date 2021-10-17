using Exolix.Json;
using Exolix.Sockets.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.NetCore;

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

            Socket.OnClose += (sender, e) =>
            {
                IsConnected = false;
                foreach (var eventCallback in OnCloseEvents)
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
                                eventTuple.Item1(parsedMessage.Data);
                            }
                        }
                    }
                } catch (Exception ex)
                {
                    Terminal.Logger.ErrorException(ex);
                }
            };

            //Socket.OnError += (sender, e) =>
            //{
            //    Terminal.Logger.Info(e.Message);

            //    foreach (var eventCallback in OnOpenFailEvents)
            //    {
            //        eventCallback();
            //    }
            //};

            Socket.Connect();
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

        public void Send<MessageType>(string channel, MessageType message)
        {
            Socket!.Send(JsonHandler.Stringify(new ConnectionMessage
            {
                Channel = channel,
                Data = JsonHandler.Stringify(message)
            }));
        }
    }
}

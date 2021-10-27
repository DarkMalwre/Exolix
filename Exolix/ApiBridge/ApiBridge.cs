using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.NetCore;

namespace Exolix.ApiBridge
{
    public class ApiBridgeSettings
    {
        public string Host = "localhost";
        public int? Port = null;
        public bool Secure = false;
    }

    public class ApiBridge
    {
        public ApiBridgeSettings Settings;
        public string ServerAddress = "";
        private WebSocket? Socket;
        private List<Action> OnOpenEvents = new List<Action>();
        private List<Tuple<string, Action<string>>> OnMessageEvents = new List<Tuple<string,Action<string>>>();

        public ApiBridge(ApiBridgeSettings? settings = null)
        {
            if (settings == null)
            {
                Settings = new ApiBridgeSettings();
                return;
            }

            Settings = settings;
        }

        private string BuildConnectAddress()
        {
            string protocol = "ws://";
            if (Settings.Secure)
            {
                protocol = "wss://";
            }

            string prefix = "";
            if (Settings.Port != null)
            {
                prefix = ":" + Settings.Port;
            }

            return protocol + Settings.Host + prefix;
        }

        public void Run()
        {
            ServerAddress = BuildConnectAddress();
            Socket = new WebSocket(ServerAddress);

            Socket.OnOpen += (sender, e) =>
            {

            };

            Socket.Connect();
        }

        public void OnOpen(Action action)
        {
            OnOpenEvents.Add(action);
        }

        public void TriggerOnOpenEvents()
        {
            foreach (var action in OnOpenEvents)
            {
                action();
            }
        }

        public void OnMessage(string channel, Action<string> action)
        {
            OnMessageEvents.Add(Tuple.Create(channel, action));
        }

        public void TriggerOnMessageEvents(string channel, string data)
        {
            foreach (var tuple in OnMessageEvents)
            {
                if (tuple.Item1 == channel)
                {
                    tuple.Item2(data);
                }
            }
        }
    }
}

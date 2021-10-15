using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.Sockets.Server
{
    public class Events
    {
        public List<Action<ServerConnection>> OnOpenEvents = new List<Action<ServerConnection>>();
        public List<Action> OnMessageEvents = new List<Action>();

        public void OnOpen(Action<ServerConnection> handler) {
            OnOpenEvents.Add(handler);
        }

        public void OnMessage(Action handler)
        {
            OnMessageEvents.Add(handler);
        }
    }
}

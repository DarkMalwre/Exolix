using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.Sockets.Server
{
    public class ServerEvents
    {
        public List<Action<ServerConnection>> OnOpenEvents = new List<Action<ServerConnection>>();

        public void OnOpen(Action<ServerConnection> handler) {
            OnOpenEvents.Add(handler);
        }
    }
}

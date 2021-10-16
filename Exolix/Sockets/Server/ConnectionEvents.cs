using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Exolix.Sockets.Server
{
    public class ConnectionEvents
    {
        public List<Tuple<Action<object>, string>> OnMessageEvents = new List<Tuple<Action<object>, string>>();
        
        public void OnMessage<MessageType>(string channel, Action<MessageType> handler)
        {
            Action<object>? newHandler = handler as Action<object>;

            if (newHandler != null)
            {
                OnMessageEvents.Add(Tuple.Create(newHandler, channel));
            }
        }
    }
}

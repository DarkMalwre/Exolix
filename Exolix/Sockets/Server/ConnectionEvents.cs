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
		public List<Tuple<Action<string>, string>> OnMessageEvents = new List<Tuple<Action<string>, string>>();
		public List<Action> OnCloseEvents = new List<Action>();

		public void OnMessage(string channel, Action<string> handler)
		{
			OnMessageEvents.Add(Tuple.Create(handler, channel));
		}

		public void OnClose(Action handler)
		{
			OnCloseEvents.Add(handler);
		}
	}
}

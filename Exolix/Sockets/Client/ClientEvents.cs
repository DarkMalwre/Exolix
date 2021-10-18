using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.Sockets.Client
{
	public class ClientEvents
	{
		public List<Action> OnOpenEvents = new List<Action>();
		//public List<Action> OnOpenFailEvents = new List<Action>();
		public List<Action> OnCloseEvents = new List<Action>();
		public List<Tuple<Action<string>, string>> OnMessageEvents = new List<Tuple<Action<string>, string>>();

		public void OnOpen(Action handler)
		{
			OnOpenEvents.Add(handler);
		}
		//public void OnOpenFail(Action handler)
		//{
		//    OnOpenEvents.Add(handler);
		//}

		public void OnClose(Action handler)
		{
			OnOpenEvents.Add(handler);
		}

		public void OnMessage(string channel, Action<string> handler)
		{
			OnMessageEvents.Add(Tuple.Create(handler, channel));
		}
	}
}

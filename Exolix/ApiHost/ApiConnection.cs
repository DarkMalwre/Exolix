using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.ApiHost
{
    public class ApiConnection
    {
        public string RemoteAddress = "";
        private IWebSocketConnection RealConnection;
        private List<Action<ApiConnection>> OnCloseEvents = new List<Action<ApiConnection>>();
		private List<Action<string>> OnMessageEvents = new List<Action<string>>();

		public ApiConnection(IWebSocketConnection connection)
        {
            RealConnection = connection;
        }

		public void OnClose(Action<ApiConnection> action)
		{
			OnCloseEvents.Add(action);
		}

		/// <summary>
		/// Trigger all on close events
		/// </summary>
		public void TriggerOnClose()
		{
			foreach (var action in OnCloseEvents)
			{
				new Thread(new ThreadStart(() => action(this))).Start();
			}
		}

		public void OnMessage(Action<string> action)
		{
			OnMessageEvents.Add(action);
		}

		/// <summary>
		/// Trigger all on message events
		/// </summary>
		/// <param name="message">Message to add to event</param>
		public void TriggerOnMessage(string message)
		{
			foreach (var action in OnMessageEvents)
			{
				new Thread(new ThreadStart(() => action(message))).Start();
			}
		}
	}
}

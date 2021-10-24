using Exolix.Json;
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
		public string Identifier = "";
        private IWebSocketConnection RealConnection;
        private List<Action<ApiConnection>> OnCloseEvents = new List<Action<ApiConnection>>();
		private List<Tuple<string, Action<string>>> OnMessageEvents = new List<Tuple<string, Action<string>>>();
		private List<Action<string, string>> OnMessageGlobalEvents = new List<Action<string, string>>();
		public bool Alive = false;

		public ApiConnection(IWebSocketConnection connection)
		{
			Alive = connection.IsAvailable;
			Identifier = connection.ConnectionInfo.Id.ToString();
			RemoteAddress = connection.ConnectionInfo.ClientIpAddress;
			RealConnection = connection;
		}

		public void Send<MessageType>(string channel, MessageType message)
        {
			CheckAliveState();

			try
            {
				var stringMessageData = JsonHandler.Stringify<MessageType>(message);
				var fullMessageString = JsonHandler.Stringify(new
				{
					Channel = channel,
					Data = stringMessageData
				});

				RealConnection.Send(fullMessageString);
			} catch (Exception) { }
        }

		public void OnClose(Action<ApiConnection> action)
		{
			OnCloseEvents.Add(action);
		}

		public void CheckAliveState()
        {
			Alive = RealConnection.IsAvailable;
			Console.WriteLine("ALV: " + Alive);
		}

		/// <summary>
		/// Trigger all on close events
		/// </summary>
		public void TriggerOnClose()
		{
			CheckAliveState();
			foreach (var action in OnCloseEvents)
			{
				new Thread(new ThreadStart(() => action(this))).Start();
			}
		}

		public void OnMessage(string channel, Action<string> action)
		{
			OnMessageEvents.Add(Tuple.Create(channel, action));
		}

		public void OnMessageGlobal(Action<string, string> action)
		{
			OnMessageGlobalEvents.Add(action);
		}

		/// <summary>
		/// Trigger all on message events
		/// </summary>
		/// <param name="message">Message to add to event</param>
		public void TriggerOnMessage(string channel, string message)
		{
			foreach (var action in OnMessageEvents)
			{
				if (action.Item1 == channel)
                {
					new Thread(new ThreadStart(() => action.Item2(message))).Start();
				}
			}
		}

		public void TriggerOnMessageGlobal(string channel, string message)
		{
			foreach (var action in OnMessageGlobalEvents)
			{
				action(channel, message);
			}
		}
	}
}

using Exolix.ApiHost;
using Exolix.Json;
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

	public class ApiConnector
	{
		public ApiBridgeSettings Settings;
		public string ServerAddress = "";
		private WebSocket? Socket;
		private List<Action> OnOpenEvents = new List<Action>();
		private List<Action> OnReadyEvents = new List<Action>();
		private List<Tuple<string, Action<string>>> OnMessageEvents = new List<Tuple<string,Action<string>>>();

		public ApiConnector(ApiBridgeSettings? settings = null)
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
			Terminal.Logger.KeepAlive(true);
			ServerAddress = BuildConnectAddress();
			Socket = new WebSocket(ServerAddress);

			OnMessage("#$server:ready", (raw) =>
			{
				TriggerOnReadyEvents();
			});

			Socket.OnOpen += (sender, e) => TriggerOnOpenEvents();
			Socket.OnMessage += (sender, e) =>
			{
				try
				{
					ApiMessageContainer parsedMessage = JsonHandler.Parse<ApiMessageContainer>(e.Data);
					if (parsedMessage.Data != null && parsedMessage.Channel != null && parsedMessage.Data is string)
					{
						TriggerOnMessageEvents(parsedMessage.Channel, parsedMessage.Data);
					}
				} catch (Exception xe) {
					Console.Error.WriteLine(xe.ToString());
				}
			};

			Socket.Connect();
		}
		 
		public void OnOpen(Action action)
		{
			OnOpenEvents.Add(action);
		}

		public void OnReady(Action action)
		{
			OnReadyEvents.Add(action);
		}

		public void TriggerOnReadyEvents()
		{
			foreach (var action in OnReadyEvents)
			{
				action();
			}
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

		public void Send<MessageType>(string channel, MessageType message)
        {
			if (Socket != null && Socket.IsAlive)
            {
				try
				{
					string parsed = JsonHandler.Stringify(new
					{
						Channel = channel,
						Data = JsonHandler.Stringify(message)
					});

					Socket.Send(parsed);
				} catch (Exception ex) {
					Console.Error.WriteLine(ex.ToString());
				}

				return;
			}

			throw new Exception("Socket is not connected");
        }
	}
}

using Exolix.Terminal;
using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.ApiHost
{
	public class ApiHostCertificate {
		public string Certificate = "";
		public string Key = "";
	}

	public class ApiClusterAuth
	{
		public string Key1 = "";
		public string Key2 = "";
	}

	public class ApiPeerNode
	{
		public string Key1 = "";
		public string Key2 = "";
		public string Host = "0.0.0.0";
		public int? Port = null;
	}

	public class ApiHostSettings
	{
		public string Host = "0.0.0.0";
		public int? Port = null;
		public ApiHostCertificate? Certificate = null;
		public ApiClusterAuth? ClusterAuth = null;
	}

	public class ApiHost
	{
		/// <summary>
		/// Settings for the API endpoint server
		/// </summary>
		private ApiHostSettings Settings;

		/// <summary>
		/// Address were server has been opened at
		/// </summary>
		public string ListeningAddress = "Server Not Connected";

		/// <summary>
		/// List of all on ready event actions
		/// </summary>
		private List<Action> OnReadyEvents = new List<Action>();

		private List<Action<ApiConnection>> OnOpenEvents = new List<Action<ApiConnection>>();

		/// <summary>
		/// Create a new API endpoint server
		/// </summary>
		/// <param name="settings">Settings for the server</param>
		public ApiHost(ApiHostSettings? settings)
		{
			if (settings == null)
			{
				Settings = new ApiHostSettings();
				return;
			}

			Settings = settings;
		}

		/// <summary>
		/// Build an address to connect to a cluster connection
		/// </summary>
		/// <param name="host">Peer node host</param>
		/// <param name="port">Peer node port</param>
		/// <param name="secure">Is the peer node using a secure protocol</param>
		/// <returns>Connect address</returns>
		private string BuildClusterConnectUrl(string host, int? port, bool secure = false)
		{
			string protocol = "ws://";
			if (secure)
			{
				protocol = "wss://";
			}

			string portSuffix = "";
			if (port != null)
			{
				portSuffix = ":" + port;
			}

			return protocol + host + portSuffix;
		}

		/// <summary>
		/// Build an address from settings for hosting the server
		/// </summary>
		/// <returns>Connection address</returns>
		private string BuildConnectUrl()
		{
			string protocol = "ws://";
			if (Settings.Certificate != null)
			{
				protocol = "wss://";
			}

			string portSuffix = "";
			if (Settings.Port != null)
			{
				portSuffix = ":" + Settings.Port;
			}
			
			return protocol + Settings.Host + portSuffix;
		}

		/// <summary>
		/// Start listening for API commands
		/// </summary>
		public void Run()
		{
			Logger.KeepAlive(true);
			ListeningAddress = BuildConnectUrl();

			var server = new WebSocketServer(ListeningAddress);
			FleckLog.LogAction = (level, message, ex) => {
				if (message == "Server started at " + ListeningAddress + " (actual port " + Settings.Port + ")")
				{
					TriggerOnReady();
				}
			};

			server.Start(socket =>
			{
				var apiConnection = new ApiConnection(socket);

				socket.OnOpen = () => TriggerOnOpen(apiConnection);

                socket.OnClose = () => apiConnection.TriggerOnClose();
				socket.OnMessage = (message) =>
				{
					apiConnection.TriggerOnMessage(message);
				};
			});
		}

		/// <summary>
		/// Trigger a callback when the server is ready for responding to client commands
		/// </summary>
		/// <param name="action">Callback</param>
		public void OnReady(Action action)
		{
			OnReadyEvents.Add(action);
		}

		/// <summary>
		/// Trigger all on ready events
		/// </summary>
		private void TriggerOnReady()
		{
			foreach (var action in OnReadyEvents)
			{
				new Thread(new ThreadStart(action)).Start();
			}
		}

		public void OnOpen(Action<ApiConnection> action)
		{
			OnOpenEvents.Add(action);
		}

		/// <summary>
		/// Trigger all on open events
		/// </summary>
		private void TriggerOnOpen(ApiConnection connection)
		{
			foreach (var action in OnOpenEvents)
			{
				new Thread(new ThreadStart(() => action(connection))).Start();
			}
		}
	}
}

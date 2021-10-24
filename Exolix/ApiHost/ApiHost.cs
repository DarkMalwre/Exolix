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
		public string Host = "localhost";
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

		private List<Action> OnReadyEvents = new List<Action>();

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
			
			return protocol + portSuffix;
		}

		/// <summary>
		/// Start listening for API commands
		/// </summary>
		public void Run()
		{
			var server = new WebSocketServer(BuildConnectUrl());
			server.Start(socket =>
			{
				socket.OnOpen = () => Console.WriteLine("Open!");
				socket.OnClose = () => Console.WriteLine("Close!");
				socket.OnMessage = message => socket.Send(message);
			});
		}
	}
}

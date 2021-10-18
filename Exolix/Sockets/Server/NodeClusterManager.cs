using Exolix.Json;
using Exolix.Sockets.Client;
using Exolix.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.Sockets.Server
{
	public class NodeClusterManager
	{
		public List<SocketClient> Nodes = new List<SocketClient>();

		public NodeClusterManager(SocketServer server, ServerNodeItem[] nodeList, SocketServerSettings serverSettings)
		{
			server.OnOpen((nodeConnection) =>
			{
				nodeConnection.OnMessage("_cluster:setup", (rawMessage) =>
				{
					NodeAuthData authData = JsonHandler.Parse<NodeAuthData>(rawMessage);

					if (serverSettings.NodeAuthData != null && serverSettings.NodeAuthData.User == authData.User && serverSettings.NodeAuthData.Key == authData.Key)
					{
						Logger.Success("Authenticated new connection");
						return;
					}

					nodeConnection.Close();
				});
			});

			foreach (var node in nodeList)
			{
				new Thread(new ThreadStart(() =>
				{
					SocketClient nodeInstance = new SocketClient(new SocketClientSettings
					{
						Host = node.Host,
						Port = node.Port,
					});

					Nodes.Add(nodeInstance);

					nodeInstance.OnOpen(() =>
					{
						nodeInstance.Send<NodeAuthData>("_cluster:setup", new NodeAuthData
						{
							User = node.User,
							Key = node.Key
						});
					});

					nodeInstance.OnClose(() =>
					{
						try
						{
							nodeInstance.Run();
						}
						catch (Exception) { }
					});

					nodeInstance.Run();
				})).Start();
			}
		}
	}
}

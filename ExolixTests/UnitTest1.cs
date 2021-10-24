using Exolix.ApiHost;
using Exolix.Terminal;
using Fleck;
using System;
<<<<<<< HEAD
=======
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WebSocketSharp.NetCore.Server;
>>>>>>> 376ae36af7f22668769dd0e8e76ad286edde105c

public class Tests
{
<<<<<<< HEAD
    public static void Main(string[] args)
    {
        Logger.Info("Starting server");

        var api = new ApiHost(new ApiHostSettings
        {

        });

        Logger.KeepAlive(true);
    }
=======
	public class MessageType
	{
		public string? Msg = "No Message Provided";
	}

	public class Tests
	{
		public class DemoMessage
		{
			public string? Message;
		}

		public static void Main(string[] args)
		{
			Logger.Info("Starting API server");
			SocketServer server = new SocketServer(new SocketServerSettings
			{
				Port = 80,
				NodeAuthData = new NodeAuthData
				{
					User = "DemoServer",
					Key = "DemoServer"
				},
				NodeList = new ServerNodeItem[]
				{
					new ServerNodeItem
					{
						Host = "localhost",
						Port = 80,
						User = "DemoServer",
						Key = "DemoServer"
					},
					new ServerNodeItem
					{
						Host = "localhost",
						Port = 8090,
						User = "DemoServer",
						Key = "DemoServer"
					}
				}
			});

			SocketServer server2 = new SocketServer(new SocketServerSettings
			{
				Port = 8090,
				NodeAuthData = new NodeAuthData
				{
					User = "DemoServer",
					Key = "DemoServer"
				},
				NodeList = new ServerNodeItem[]
				{
					new ServerNodeItem
					{
						Host = "localhost",
						Port = 80,
						User = "DemoServer",
						Key = "DemoServer"
					},
					new ServerNodeItem
					{
						Host = "localhost",
						Port = 8090,
						User = "DemoServer",
						Key = "DemoServer"
					}
				}
			});

			server2.Run();
			server.Run();

			Logger.Success("Server started at port 80");            
		}

		public static void DoException(SocketServer server)
		{
			DoExceptionSec(server);
		}

		public static void DoExceptionSec(SocketServer server)
		{
			try
			{
				server.Run();
			}
			catch (Exception ex)
			{
				Logger.ErrorException(ex);
			}
		}
	}
>>>>>>> 376ae36af7f22668769dd0e8e76ad286edde105c
}
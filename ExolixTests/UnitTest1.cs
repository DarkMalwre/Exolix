using Exolix.ApiHost;
using Exolix.Json;
using Exolix.Terminal;

public class MsgType
{
	public string Msg = "Hewwooo";
}

public class App
{
	public static void Main(string[] args)
	{
		var api = new ApiHost(new ApiHostSettings
		{
			Port = 8080
		});

		api.OnOpen((connection) =>
		{
			api.Emit("channel", new MsgType
			{
				Msg = "[System] New User Connected { Total = " + api.ConnectedClients + ", IP = " + connection.RemoteAddress + " }"
			});
			Logger.Info("[" + connection.RemoteAddress + "] Opened Total = " + api.ConnectedClients);

			connection.OnClose((connection) =>
			{
				Logger.Info("Closed Total = " + api.ConnectedClients);
				api.Emit("channel", new MsgType
				{
					Msg = "[System] User Disconnected { Total = " + api.ConnectedClients + ", IP = " + connection.RemoteAddress + " }"
				});
			});

			connection.OnMessageGlobal((channel, message) =>
			{
				var nmsg = JsonHandler.Parse<MsgType>(message);
				api.Emit("Main", new MsgType
				{
					Msg = nmsg.Msg,
				});
			});
		});

		api.Run();
	}
}
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
			Logger.Info("[" + connection.RemoteAddress + "] Opened");

			connection.OnClose((connection) =>
			{
				Logger.Info("Closed");
			});

			connection.OnMessageGlobal((channel, message) =>
			{
				var nmsg = JsonHandler.Parse<MsgType>(message);
				connection.Send("Main", new MsgType
				{
					Msg = nmsg.Msg,
				});
			});
		});

		api.Run();
	}
}
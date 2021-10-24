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
		Animation.Start("Booting");
		var api = new ApiHost(new ApiHostSettings
		{
			Port = 8080
		});

		api.OnReady(() =>
		{
			Animation.Stop("Boot container ready");
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
				Logger.Info("[" + channel + new string(' ', connection.RemoteAddress.Length - channel.Length) + "] " + nmsg.Msg);
			});
		});

		api.Run();
	}
}
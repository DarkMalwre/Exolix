using Exolix.ApiHost;
using Exolix.Terminal;

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
			Logger.Info("Opened");

			connection.OnClose((connection) =>
			{
				Logger.Info("Closed");
			});

			connection.OnMessageGlobal((channel, message) =>
			{
				Logger.Info("[" + channel + "] " + message);
			});
		});

		api.Run();
	}
}
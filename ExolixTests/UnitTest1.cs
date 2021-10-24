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
	}
}
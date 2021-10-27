using Exolix.ApiBridge;
using Exolix.ApiHost;
using Exolix.Json;
using System;

// Create a structure for messages from a connection
public class MessageType
{
	public string Msg = "No Message Was Provided";
}

// Application main class (Entry Point)
public class App
{
	public static void Main(string[] args)
	{
		// Create the new API server
		ApiHost api = new ApiHost(new ApiHostSettings
		{
			Port = 8080 // Set the listening port to 8080
		});

		// Listen for when the server is ready to listen for connections
		api.OnReady(() =>
		{
			Console.WriteLine("The server is ready"); // Log the success message
			ApiBridge brd =	new ApiBridge(new ApiBridgeSettings
            {
				Port = 8080
            });

			brd.Run();
		});

		// Listen for when the server recieves a new connection
		api.OnOpen((connection) =>
		{
			Console.WriteLine("[ Connection ] Opened, IP address is \"" + connection.RemoteAddress + "\""); // Log to the server about a new connection

			// Listen for messages from the connection
			connection.OnMessageGlobal((channel /* What channel the message was sent to */, raw /* Message serialized data */) =>
			{
				MessageType message = JsonHandler.Parse<MessageType>(raw); // Convert the raw message data to an object
				Console.WriteLine("[ Message ] New message on channel \"" + channel + "\", Contents = \"" + message.Msg + "\""); // Log information about the message
			});
		});

		api.Run(); // Start the server and try to listen on the listening address
	}
}

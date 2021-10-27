using Exolix.ApiBridge;
using Exolix.ApiHost;
using Exolix.Json;
using Exolix.Terminal;
using System;
using System.Collections.Generic;

// Create a structure for messages from a connection
public class MessageType
{
	public string Message = "No Message Was Provided";
	public string Poster = "Unknown";
}

// Application main class (Entry Point)
public class App
{
	public static void Main(string[] args)
	{
		List<Tuple<string, string>> posts = new List<Tuple<string, string>>();

		// Create the new API server
		ApiHost api = new ApiHost(new ApiHostSettings
		{
			Port = 8070, // Set the listening port to 8070
			PeerAuth = new ApiPeerAuth
            {
				Key1 = "peer"
            },
			PeerNodes = new List<ApiPeerNode>
            {
                new ApiPeerNode
                {
					Port = 8070,
					Key1 = "peer"
                }
            }
		});

		// Listen for when the server is ready to listen for connections
		api.OnReady(() =>
		{
			Logger.Info("The server is ready"); // Log the success message
		});

		var emitPosts = () =>
		{
			api.Emit("post:clear-ui", new { });

			foreach (var post in posts)
            {
				api.Emit("post:read", post);
            }
		};

		// Listen for when the server recieves a new connection
		api.OnOpen((connection) =>
		{
			emitPosts();
			Logger.Info("[ Connection ] Opened, IP address is \"" + connection.RemoteAddress + "\""); // Log to the server about a new connection

			// Listen for messages from the connection
			connection.OnMessage("post:write", (raw) =>
			{
				MessageType message = JsonHandler.Parse<MessageType>(raw);
				Logger.Info("Creating new post by " + message.Poster);

				posts.Add(Tuple.Create(message.Poster, message.Message));
				emitPosts();
			});
		});

        api.Run(); // Start the server and try to listen on the listening address
    }
}

using Exolix.ApiBridge;
using Exolix.ApiHost;
using Exolix.DataBase;
using Exolix.Json;
using Exolix.Terminal;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

// Create a structure for messages from a connection
public class MessageType
{
	public string Message = "No Message Was Provided";
	public string Poster = "Unknown";
}

public class Obj
{
	public MongoDB.Bson.ObjectId _id = new MongoDB.Bson.ObjectId();
	public string userName = "";
	public string displayName = "";
	public int tagNumber = 0;
	public string[] otherEmails = { };
	public string token = "";
	public string email = "";
}

// Application main class (Entry Point)
public class App
{
	public static void Main(string[] args)
	{
		List<Tuple<string, string>> posts = new List<Tuple<string, string>>();

		DataBaseApi db = new DataBaseApi();
        db.Run();

        db.InsertRecord("Axeri", "Accounts", new BsonDocument
        {
            { "displayName", "Uwu Kitty Cattt" }
        });

        db.FetchRecords<Obj>("Axeri", "Accounts", new string[,] {
            { "displayName", "XFaon" }
        })?.ForEach((doc) =>
        {
            Console.WriteLine(doc.displayName);
        });

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

			api.OnOpen((connection) =>
			{
				Logger.Info("New Connection");
				api.Send("ws://localhost:8070", connection.Identifier, "printer", new
				{
					Msg = "Hewwoo"
				});
			});
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

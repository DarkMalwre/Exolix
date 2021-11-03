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
	public string Name = "";
	public ObjectId _id = new();
}

// Application main class (Entry Point)
public class App
{
	public static void Main(string[] args)
	{
		DataBaseApi api = new DataBaseApi(new DataBaseApiSettings
		{

		});

		api.OnReady(() =>
		{
			Logger.Info("Connected to database localy");

			api.FetchRecords<Obj>("Axeri", "Tes", new string[,]
			{
				{ "Name", "OldName" }
			})?.ForEach((e) =>
			{
				Console.WriteLine(e.Name);
			});

			api.UpdateRecords<Obj>("Axeri", "Tes", new string[,]
			{
				{ "Name", "OldName" }
			}, new string[,]
			{
				{ "Name", "XFaonUpdated" }
			});
		});

		api.Run();
	}
}

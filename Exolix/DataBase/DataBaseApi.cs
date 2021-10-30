using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.DataBase
{
	public class UwuGamer
	{
		public ObjectId _id = new ObjectId();
		public string displayName = "XFaon";
		public string userName = "";
		public int tagNumber = 1;
		public string email = "";
		public string[] otherEmails = { "" };
		public string token = "";
	}

	public class DataBaseApiSettings
    {
		public string ConnectAddress = "mongodb://localhost:27017";
	}

	public class QueryFetchOptions
	{
		public int? Limit = null;
	}

	public class DataBaseApi
	{
		public MongoClient? Client;
		private List<Action> OnReadyEvents = new List<Action>();
		private DataBaseApiSettings Settings;

		public DataBaseApi(DataBaseApiSettings settings)
		{
			if (settings == null)
            {
				Settings = new DataBaseApiSettings();
				return;
            }

			Settings = settings;
		}

		public void InsertRecord(string database, string collection, BsonDocument document)
		{
			Client?.GetDatabase(database).GetCollection<BsonDocument>(collection).InsertOne(document);
		}

		public void Run()
		{
			new Thread(new ThreadStart(() =>
			{
				Client = new MongoClient(Settings.ConnectAddress);
				TriggerOnReadyEvents();
			})).Start();
		}

		public List<DocType>? FetchRecords<DocType>(string database, string collection, string[,] stringFilters, QueryFetchOptions? settings = null)
		{
            List<FilterDefinition<DocType>> filters = new List<FilterDefinition<DocType>>();

			for (int index = 0; index < stringFilters.GetLength(0); index++)
            {
				filters.Add(Builders<DocType>.Filter.Eq(stringFilters[index, 0], stringFilters[index, 1]));
            }

            IFindFluent<DocType, DocType>? query = Client?.GetDatabase(database).GetCollection<DocType>(collection).Find(Builders<DocType>.Filter.And(filters));

            if (settings == null)
            {
                settings = new QueryFetchOptions();
            }

            if (settings.Limit != null)
            {
                query?.Limit(settings.Limit);
            }

			try
            {
				return query.ToList();
			} catch (Exception ex)
            {
				Console.Error.WriteLine(ex);
				return null;
            }
        }

		public void OnReady(Action action)
        {
			OnReadyEvents.Add(action);
        }

		public void TriggerOnReadyEvents()
        {
			foreach (var action in OnReadyEvents)
            {
				action();
            }
        }
	}
}

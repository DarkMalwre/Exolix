using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.DataBase
{
	//public class QueryUpdateOptions
	//{

	//}

	//public class DataBaseApiSettings
 //   {
	//	public string ConnectAddress = "mongodb://localhost:27017";
	//}

	//public class QueryFetchOptions
	//{
	//	public int? Limit = null;
	//}

	//public class DataBaseApi
	//{
	//	public MongoClient? Client;
	//	private List<Action> OnReadyEvents = new List<Action>();
	//	private DataBaseApiSettings Settings;

	//	public DataBaseApi(DataBaseApiSettings settings)
	//	{
	//		if (settings == null)
 //           {
	//			Settings = new DataBaseApiSettings();
	//			return;
 //           }

	//		Settings = settings;
	//	}

	//	public void InsertRecord(string database, string collection, BsonDocument document)
	//	{
	//		Client?.GetDatabase(database).GetCollection<BsonDocument>(collection).InsertOne(document);
	//	}

	//	public void Run()
	//	{
	//		new Thread(new ThreadStart(() =>
	//		{
	//			Client = new MongoClient(Settings.ConnectAddress);
	//			TriggerOnReadyEvents();
	//		})).Start();
	//	}

	//	public List<DocType>? FetchRecords<DocType>(string database, string collection, string[,] stringFilters, QueryFetchOptions? settings = null)
	//	{
 //           List<FilterDefinition<DocType>> filters = new List<FilterDefinition<DocType>>();

	//		for (int index = 0; index < stringFilters.GetLength(0); index++)
 //           {
	//			filters.Add(Builders<DocType>.Filter.Eq(stringFilters[index, 0], stringFilters[index, 1]));
 //           }

 //           IFindFluent<DocType, DocType>? query = Client?.GetDatabase(database).GetCollection<DocType>(collection).Find(Builders<DocType>.Filter.And(filters));

 //           if (settings == null)
 //           {
 //               settings = new QueryFetchOptions();
 //           }

 //           if (settings.Limit != null)
 //           {
 //               query?.Limit(settings.Limit);
 //           }

	//		try
 //           {
	//			return query.ToList();
	//		} catch (Exception ex)
 //           {
	//			Console.Error.WriteLine(ex);
	//			return null;
 //           }
 //       }

	//	public void UpdateRecords<DocType>(string database, string collection, string[,] stringFilters, string[,] updateProps, QueryUpdateOptions? settings = null)
	//	{
	//		List<UpdateDefinition<DocType>> updatesList = new();

	//		var builder = Builders<DocType>.Filter;
	//		var filter = builder.Empty;

	//		for (int index = 0; index < stringFilters.GetLength(0); index++)
	//		{
	//			filter &= builder.Eq(stringFilters[index, 0], stringFilters[index, 1]);
	//		}

	//		for (int index = 0; index < updateProps.GetLength(0); index++)
	//		{
	//			//if (index == 0)
	//			//{
	//			//	Console.WriteLine("Create");
	//			//	update = Builders<BsonDocument>.Update.Set(stringFilters[index, 0], stringFilters[index, 1]);
	//			//	update.Dump();
	//			//}

	//			//if (update != null)
	//			//{
	//			//	update.Set(stringFilters[index, 0], stringFilters[index, 1]);
	//			//}
	//			updatesList.Add(Builders<DocType>.Update.Set(stringFilters[index, 0], stringFilters[index, 1]));
	//		}

	//		var update = Builders<DocType>.Update.Combine(updatesList);

	//		if (settings == null)
	//		{
	//			settings = new QueryUpdateOptions();
	//		}

	//		//if (settings.Limit != null)
	//		//{
	//		//	query?.Limit(settings.Limit);
	//		//}

	//		IMongoCollection<DocType>? queryCollection = Client?.GetDatabase(database).GetCollection<DocType>(collection);
	//		var updateResult = queryCollection?.UpdateMany(filter, /* update */ Builders<DocType>.Update.Set(t => t.GetType()!.GetProperty("Name")!.GetValue(t, null), "New"));
	//	}

	//	public void OnReady(Action action)
 //       {
	//		OnReadyEvents.Add(action);
 //       }

	//	public void TriggerOnReadyEvents()
 //       {
	//		foreach (var action in OnReadyEvents)
 //           {
	//			action();
 //           }
 //       }
	//}
}

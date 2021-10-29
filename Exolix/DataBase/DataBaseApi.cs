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

	public class QueryFetchOptions
	{
		public int? Limit = null;
	}

	public class DataBaseApi
	{
		public MongoClient? Client;

		public DataBaseApi()
		{

		}

		public void InsertDocument<DocType>(string database, string collection, DocType document)
		{

		}

		public void Run()
		{
			Client = new MongoClient("mongodb://localhost:27017");
		}

		public DocType[]? FetchRecords<DocType>(string database, string collection, string[,] stringFilters, QueryFetchOptions? settings = null)
		{
            var filters = new List<FilterDefinition<DocType>>();

			for (int index = 0; index < stringFilters.GetLength(0); index++)
            {
				Console.WriteLine(">: " + index);
				Console.WriteLine(stringFilters[index, 0]);
            }

			return null;

            foreach (var item in stringFilters)
			{
				if (item.Length > 0)
				{
					Console.WriteLine(item);
					//filters.Add(Builders<DocType>.Filter.Eq(item[0], item[1]));
				} else
				{
					// TODO: THROW ERR
				}
			}

			return null;

			//    var query = Client?.GetDatabase(database).GetCollection<DocType>(collection).Find(new BsonDocument(new object {}));

			//    if (settings == null)
			//    {
			//        settings = new QueryFetchOptions();
			//    }

			//    if (settings.Limit != null)
			//    {
			//        query?.Limit(settings.Limit);
			//    }

			//    return null;
			//}
		}
	}
}

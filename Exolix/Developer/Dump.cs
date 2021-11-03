using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ObjectHelper
{
	/// <summary>
	/// Dump the object
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="x"></param>
	public static void Dump<T>(this T x)
	{
		string json = JsonConvert.SerializeObject(x, Formatting.Indented);
		Console.WriteLine(json);
	}
}

namespace Exolix.Developer
{

	public class Dump
	{
		public void FromObject(object obj)
		{
			JsonConvert.SerializeObject(obj, Formatting.Indented);
		}
	}
}

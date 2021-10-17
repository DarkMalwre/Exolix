using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.Json
{
    public class JsonHandler
    {
        public static DocumentType Parse<DocumentType>(string jsonString)
        {
            return JsonConvert.DeserializeObject<DocumentType>(jsonString, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            })!;
        }

        public static string Stringify<DocumentType>(DocumentType json)
        {
            return JsonConvert.SerializeObject(json);
        }
    }
}

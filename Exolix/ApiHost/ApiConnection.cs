using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.ApiHost
{
    public class ApiConnection
    {
        public string RemoteAddress = "";
        private IWebSocketConnection RealConnection;

        public ApiConnection(IWebSocketConnection connection)
        {
            RealConnection = connection;
        }


    }
}

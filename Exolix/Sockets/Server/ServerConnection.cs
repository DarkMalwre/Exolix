using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Exolix.Sockets.Server
{
    public class ServerConnection
    {
        public string ID = "";

        public ServerConnection(CoreServerConnection coreConnection)
        {
            ID = coreConnection.ID;
        }
    }

    public class CoreServerConnection: WebSocketBehavior
    {
        protected override void OnOpen()
        {
            ServerConnection connection = new ServerConnection(this);
        }
    }
}

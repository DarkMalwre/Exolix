using Exolix.Json;
using Exolix.Sockets.Client;
using Exolix.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.Sockets.Server
{
    public class NodeClusterManager
    {
        public NodeClusterManager(SocketServer server, ServerNodeItem[] nodeList)
        {
            server.OnOpen((nodeConnection) =>
            {
                nodeConnection.OnMessage("_cluster:setup", (rawMessage) =>
                {
                    NodeAuthData authData = JsonHandler.Parse<NodeAuthData>(rawMessage);
                    Logger.Info($"[ Query Authenticate ] User={authData.User ?? "NoUserProvided"} key={authData.Key ?? "NoKeyProvided"}");
                });
            });

            foreach (var node in nodeList)
            {
                Logger.Info("[ Queue Connect ] " + node.Host + ":" + node.Port);
                SocketClient nodeInstance = new SocketClient(new SocketClientSettings
                {
                    Host = node.Host,
                    Port = node.Port,
                });

                nodeInstance.OnOpen(() =>
                {
                    nodeInstance.Send<NodeAuthData>("_cluster:setup", new NodeAuthData
                    {
                        User = node.User,
                        Key = node.Key
                    });
                });

                nodeInstance.Run();
            }
        }
    }
}

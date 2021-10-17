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
        public NodeClusterManager(SocketServer server, ServerNodeItem[] nodeList, SocketServerSettings serverSettings)
        {
            server.OnOpen((nodeConnection) =>
            {
                Logger.Info("New Connection: " + nodeConnection.RemoteAddress);

                nodeConnection.OnMessage("_cluster:setup", (rawMessage) =>
                {
                    NodeAuthData authData = JsonHandler.Parse<NodeAuthData>(rawMessage);
                    Logger.Info($"[ Query Authenticate ] User={authData.User ?? "NoUserProvided"} key={authData.Key ?? "NoKeyProvided"}");

                    if (serverSettings.NodeAuthData != null && serverSettings.NodeAuthData.User == authData.User && serverSettings.NodeAuthData.Key == authData.Key)
                    {
                        Logger.Success("Authenticated new connection");
                        return;
                    }

                    Logger.Warning("A possible cluster node failed to authenticate");
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

                //nodeInstance.OnOpenFail(() =>
                //{
                //    Logger.Warning("Failed to connect to server for authentication, The server may still be starting. Trying again");
                //    nodeInstance.Run();
                //});

                nodeInstance.Run();
            }
        }
    }
}

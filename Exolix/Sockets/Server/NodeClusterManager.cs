using Exolix.Json;
using Exolix.Sockets.Client;
using Exolix.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
                new Thread(new ThreadStart(() =>
                {
                    Logger.Info("[ Queue Connect ] " + node.Host + ":" + node.Port);
                    SocketClient nodeInstance = new SocketClient(new SocketClientSettings
                    {
                        Host = node.Host,
                        Port = node.Port,
                    });

                    //bool tryPingTesting = false;
                    //bool pingable = false;
                    //Ping? pinger = null;

                    //new Thread(new ThreadStart(() =>
                    //{
                    //    do
                    //    {
                    //        try
                    //        {
                    //            pinger = new Ping();
                    //            PingReply reply = pinger.Send(node.Host + (node.Port != null ? ":" + node.Port : ""));
                    //            pingable = reply.Status == IPStatus.Success;
                    //        }
                    //        catch (Exception ) { }
                    //        finally
                    //        {
                    //            if (pinger != null)
                    //            {
                    //                pinger.Dispose();
                    //            }
                    //        }

                    //        if (pingable)
                    //        {
                    //            Logger.Info("This host is good " + node.Host + ":" + node.Port);
                    //            tryPingTesting = false;
                    //        }
                    //        else
                    //        {
                    //            Logger.Warning("Failed to ping " + node.Host + ":" + node.Port);
                    //        }

                    //        Thread.Sleep(500);
                    //    } while (tryPingTesting);
                    //})).Start();

                    bool connected = false;

                    nodeInstance.OnOpen(() =>
                    {
                        connected = true;
                        nodeInstance.Send<NodeAuthData>("_cluster:setup", new NodeAuthData
                        {
                            User = node.User,
                            Key = node.Key
                        });
                    });

                    nodeInstance.OnClose(() =>
                    {
                        if (connected)
                        {
                            Logger.Error("An initialized node has broken away");
                            return;
                        }

                        try
                        {
                            nodeInstance.Run();
                        } 
                        catch (Exception) { }
                    });

                    nodeInstance.Run();
                })).Start();
            }
        }
    }
}

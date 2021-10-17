﻿using Exolix.Json;
using Exolix.Sockets.Client;
using Exolix.Sockets.Server;
using Exolix.Terminal;
using Pastel;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WebSocketSharp.Server;

namespace ExolixTests
{
    public class MessageType
    {
        public string? Msg = "No Message Provided";
    }

    public class Tests
    {
        public class DemoMessage
        {
            public string? Message;
        }

        public static void Main(string[] args)
        {
            Logger.Info("Starting API server");
            SocketServer server = new SocketServer(new SocketServerSettings
            {
                Port = 80,
                NodeAuthData = new NodeAuthData
                {
                    User = "DemoServer",
                    Key = "DemoServer"
                },
                NodeList = new ServerNodeItem[]
                {
                    new ServerNodeItem
                    {
                        Host = "localhost",
                        Port = 80,
                        User = "DemoServer",
                        Key = "DemoServer"
                    },
                    new ServerNodeItem
                    {
                        Host = "localhost",
                        Port = 8090,
                        User = "DemoServer",
                        Key = "DemoServer"
                    }
                }
            });

            SocketServer server2 = new SocketServer(new SocketServerSettings
            {
                Port = 8090,
                NodeAuthData = new NodeAuthData
                {
                    User = "DemoServer",
                    Key = "DemoServer"
                },
                NodeList = new ServerNodeItem[]
                {
                    new ServerNodeItem
                    {
                        Host = "localhost",
                        Port = 80,
                        User = "DemoServer",
                        Key = "DemoServer"
                    },
                    new ServerNodeItem
                    {
                        Host = "localhost",
                        Port = 8090,
                        User = "DemoServer",
                        Key = "DemoServer"
                    }
                }
            });

            server2.Run();
            server.Run();

            Thread.Sleep(1000);
            server2.Stop();

            Logger.Success("Server started at port 80");            
        }

        public static void DoException(SocketServer server)
        {
            DoExceptionSec(server);
        }

        public static void DoExceptionSec(SocketServer server)
        {
            try
            {
                server.Run();
            }
            catch (Exception ex)
            {
                Logger.ErrorException(ex);
            }
        }
    }

    public class HandleService: WebSocketBehavior {
        protected override void OnMessage(WebSocketSharp.MessageEventArgs e)
        {
            Logger.PrintDynamic("MSG: " + e.Data);
        }

        protected override void OnOpen()
        {
            Logger.PrintDynamic("New Conn: " + Context.QueryString["name"]);
        }
    }
}
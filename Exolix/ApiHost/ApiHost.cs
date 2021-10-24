using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.ApiHost
{
    public class ApiHostCertificate {
        public string Certificate = "";
        public string Key = "";
    }

    public class ApiHostSettings
    {
        public string Host = "0.0.0.0";
        public int? Port = null;
        public ApiHostCertificate? Certificate = null;
    }

    public class ApiHost
    {
        /// <summary>
        /// Settings for the API endpoint server
        /// </summary>
        private ApiHostSettings Settings;

        /// <summary>
        /// Create a new API endpoint server
        /// </summary>
        /// <param name="settings">Settings for the server</param>
        public ApiHost(ApiHostSettings? settings)
        {
            if (settings == null)
            {
                Settings = new ApiHostSettings();
                return;
            }

            Settings = settings;
        }

        /// <summary>
        /// Start listening for API commands
        /// </summary>
        public void Run()
        {
            var server = new WebSocketServer("ws://0.0.0.0:8181");
            server.Start(socket =>
            {
                socket.OnOpen = () => Console.WriteLine("Open!");
                socket.OnClose = () => Console.WriteLine("Close!");
                socket.OnMessage = message => socket.Send(message);
            });
        }
    }
}

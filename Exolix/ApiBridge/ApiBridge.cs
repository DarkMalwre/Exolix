using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.ApiBridge
{
    public class ApiBridgeSettings
    {
        public string Host = "localhost";
        public int? Port = null;
    }

    public class ApiBridge
    {
        public ApiBridgeSettings Settings;

        public ApiBridge(ApiBridgeSettings? settings)
        {
            if (settings == null)
            {
                Settings = new ApiBridgeSettings();
                return;
            }

            Settings = settings;
        }

        public void Run()
        {

        }
    }
}

using Exolix.ApiHost;
using Exolix.Terminal;
using Fleck;
using System;

public class Tests
{
    public static void Main(string[] args)
    {
        Logger.Info("Starting server");

        var api = new ApiHost(new ApiHostSettings
        {

        });

        Logger.KeepAlive(true);
    }
}
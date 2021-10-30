![Exolix Banner](https://github.com/AxeriDev/Exolix/blob/master/ExolixBanner.png?raw=true)
C# Real-Time Framework

# Why
Why Exolix? Exolix is a C# framework that allows you to develop API applications without needing too many external dependencies. Exolix keeps its consistancy to eliminate breaking changes! 

# Links
 - [Project Website](https://axeri.net/projects/exolix)
 - [Documentation](https://axeri.net/docs/exolix)

# Simple Usage Example
This example will show how to create a server with exolix!

```cs
using Exolix.Terminal;

class MainFile 
{
	public static void Main(string[] args)
	{
		logger.info("Hello, Exolix!");
		// configure Exolix API host server settings
        	ApiHost server = new ApiHost(new ApiHostSettings { 
            		Port = 4040
        	});
	server.OnReady(() =>
        {
            Logger.Success("Server is ready");
            // Check for incomming connection to the server
            server.OnOpen((con) =>
            {
	    // code for when a connection is made (;
            });
        });
	//run the server
        server.Run();
	}
}
```

# Links
 - [Maintainers](https://axeri.net/staff/developers)
 - [Maintainers GitHub](https://github.com/axeridev)
 - [Docs (Coming Soon)](https://github.com/AxeriDev/Exolix/wiki)

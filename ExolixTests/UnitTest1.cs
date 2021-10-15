using Exolix.Terminal;

namespace ExolixTests
{
    public class Tests
    {
        public static void Main(string[] args)
        {
            Logger.PrintDynamic("hey");

            System.Threading.Thread.Sleep(1000);
            Logger.PrintDynamic("1s Past");
        }
    }
}
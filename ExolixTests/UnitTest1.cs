using Microsoft.VisualStudio.TestTools.UnitTesting;
using Exolix.Terminal;
using System;
using Exolix.Developer;

namespace ExolixTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            States.SetDebugMode(true);

            Logger.PrintDynamic("----------     ----- Exolic -----     ----------\n");
            Logger.PrintDynamic("Hey Whats Up\n");

            States.End();
        }
    }
}
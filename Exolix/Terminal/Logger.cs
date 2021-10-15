using Exolix.Developer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.Terminal
{
    public class Logger
    {
        public static void Info(string text)
        {
            Console.WriteLine(text);
        }

        public static void PrintDynamic(string stdOutRaw)
        {
            

            if (States.GetDebugMode())
            {
                Debug.Write(stdOutRaw);
                return;
            }

            Console.Write("Raw: " + stdOutRaw);
        }
    }
}

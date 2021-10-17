using Exolix.Developer;
using Pastel;
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

            Console.Write(stdOutRaw);
        }

        public static void PrintLineDynamic(string stdOutText)
        {
            PrintDynamic(stdOutText + "\n");
        }

        public static void AppDone(string toAction = "exit")
        {
            Logger.PrintLineDynamic($" {"Press".Pastel("#999999")} {"ENTER".Pastel("#ffffff")} {$"to {toAction}".Pastel("#999999")}");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }

        public static void HideCursor()
        {
            Console.CursorVisible = false;
        }

        public static void ShowCursor()
        {
            Console.CursorVisible = true;
        }
    }
}

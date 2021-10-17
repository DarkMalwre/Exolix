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
        private static bool KeepAliveState = false;
        private static Thread? KeepAliveThreadInstance;

        public static void Info(string message)
        {
            PrintLineDynamic(" ·".Pastel("#60cdff") + " " + message);
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

        public static void Success(string message)
        {
            PrintLineDynamic(" ·".Pastel("#50ffab") + " " + message);
        }

        public static void Error(string message)
        {
            PrintLineDynamic(" ·".Pastel("#ff5555") + " " + message);
        }

        public static void Warning(string message)
        {
            PrintLineDynamic(" ·".Pastel("#ffff55") + " " + message);
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

        private static void KeepAliveThread()
        {
            do
            {
                Thread.Sleep(100);
            } while (KeepAliveState);
        }

        public static void KeepAlive(bool enabled)
        {
            KeepAliveState = enabled;

            if (enabled)
            {
                if (KeepAliveThreadInstance == null || !KeepAliveThreadInstance.IsAlive)
                {
                    KeepAliveThreadInstance = new Thread(new ThreadStart(KeepAliveThread));
                    KeepAliveThreadInstance.Start();
                }
            }
        }
    }
}

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
	public static class StandAloneLogger
    {
		public static void LogObj(object obj)
        {
			Console.WriteLine(obj.ToString());
        }
    }

	public class Logger
	{
		private static bool KeepAliveState = false;
		private static Thread? KeepAliveThreadInstance;
		private static int KeepAliveRequests = 0;

		public static void Info(string message)
		{
			PrintLineDynamic(" · [ Info ]".Pastel("#60cdff") + " " + message);
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
			PrintLineDynamic(" · [ Success ]".Pastel("#50ffab") + " " + message);
		}

		public static void Error(string message)
		{
			PrintLineDynamic(" · [ Error ]".Pastel("#ff5555") + " " + message);
		}

		public static void ErrorException(Exception error)
		{
			Error(error.Message);
			StackFrame[] stFrames = new StackTrace(1, true).GetFrames();

			foreach (var stack in stFrames)
			{
				Error("    " + stack.ToString().TrimEnd('\n'));
			}
		}

		public static void Warning(string message)
		{
			PrintLineDynamic(" · [ Warning ]".Pastel("#ffaa55") + " " + message);
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
			if (enabled)
			{
				KeepAliveRequests++;

				if (KeepAliveThreadInstance == null || !KeepAliveThreadInstance.IsAlive)
				{
					KeepAliveThreadInstance = new Thread(new ThreadStart(KeepAliveThread));
					KeepAliveThreadInstance.Start();
				}
			}
			else
			{
				KeepAliveRequests--;
			}

			if (KeepAliveRequests > 0)
			{
				KeepAliveState = true;
				return;
			}

			KeepAliveState = false;
		}
	}
}
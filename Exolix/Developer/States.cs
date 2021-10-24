using Exolix.Terminal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exolix.Developer
{
	public class States
	{
		private static bool isDebugMode = false;

		public static void SetDebugMode(bool enabled)
		{
			isDebugMode = enabled;
		}

		public static bool GetDebugMode()
		{
			return isDebugMode;
		}

		public static void End()
		{
			if (isDebugMode)
			{
				Logger.PrintDynamic("\n Exolix End | Debug Mode \n --- \n");
			}
		}
	}
}

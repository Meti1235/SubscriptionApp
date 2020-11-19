using System;
using System.Collections.Generic;
using System.Text;

namespace MassEmailSender.Services.Helpers
{
	public static class MessageHelper
	{
		public static void PrintMessage(string errorMsg, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(errorMsg);
			Console.ResetColor();
			Console.ReadLine();
		}
	}
}

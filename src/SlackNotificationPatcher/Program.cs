using SlackNotificationPatcher.Infrastructure;
using SlackNotificationPatcher.Infrastructure.Implementation;
using System;

namespace SlackNotificationPatcher
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Patching slack notifications\n");

			INotificationFixer notificationFixer = new NotificationFixer();
			notificationFixer.FixAll();

			Console.WriteLine("\nDone");
			Console.ReadKey(true);
		}
	}
}

using Microsoft.Extensions.DependencyInjection;
using SlackNotificationPatcher.Infrastructure;
using SlackNotificationPatcher.Infrastructure.Implementation;
using System;

namespace SlackNotificationPatcher
{
	public class Program
	{
		public static IServiceProvider Container { get; private set; }

		static void Main(string[] args)
		{
			Console.WriteLine("Patching slack notifications\n");

			RegisterServices();

			var notificationFixer = Container.GetService<INotificationFixer>();
			notificationFixer.FixAll();

			Console.WriteLine("\nDone");
			Console.ReadKey(true);
		}

		private static void RegisterServices()
		{
			var services = new ServiceCollection();
			services.AddSingleton<INotificationFixer, NotificationFixer>();
			services.AddTransient<ISlackFinder, SlackFinder>();
			services.AddTransient<IPatcher, NotificationPatcher>();
			Container = services.BuildServiceProvider();
		}
	}
}

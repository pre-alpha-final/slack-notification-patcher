using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SlackNotificationPatcher.Infrastructure.Implementation
{
	class NotificationFixer : INotificationFixer
	{
		public void FixAll()
		{
			var slackFinder = Program.Container.GetService<ISlackFinder>();
			var fileInfoList = slackFinder.FindAll().ToList();

			var tasks = new List<Task>();
			foreach (var fileInfo in fileInfoList)
			{
				tasks.Add(Task.Factory.StartNew(() =>
				{
					var patcher = Program.Container.GetService<IPatcher>();
					patcher.Patch(fileInfo);
				}));
			}
			Task.WaitAll(tasks.ToArray());
		}
	}
}

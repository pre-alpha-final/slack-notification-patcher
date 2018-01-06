using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SlackNotificationPatcher.Infrastructure.Implementation
{
	class NotificationFixer : INotificationFixer
	{
		public void FixAll()
		{
			ISlackFinder slackFinder = new SlackFinder();
			var fileInfoList = slackFinder.FindAll().ToList();

			var tasks = new List<Task>();
			foreach (var fileInfo in fileInfoList)
			{
				tasks.Add(Task.Factory.StartNew(() =>
				{
					IPatcher patcher = new NotificationPatcher();
					patcher.Patch(fileInfo);
				}));
			}
			Task.WaitAll(tasks.ToArray());
		}
	}
}

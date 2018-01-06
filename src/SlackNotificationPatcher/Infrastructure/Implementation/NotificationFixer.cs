using System.Linq;

namespace SlackNotificationPatcher.Infrastructure.Implementation
{
	class NotificationFixer : INotificationFixer
	{
		public void FixAll()
		{
			ISlackFinder slackFinder = new SlackFinder();
			var fileInfoList = slackFinder.FindAll().ToList();
			foreach (var fileInfo in fileInfoList)
			{
				IPatcher patcher = new NotificationPatcher();
				patcher.Patch(fileInfo);
			}
		}
	}
}

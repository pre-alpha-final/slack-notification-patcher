using System.Linq;

namespace SlackNotificationPatcher.Infrastructure.Implementation
{
	class NotificationFixer : INotificationFixer
	{
		public void FixAll()
		{
			ISlackFinder slackFinder = new SlackFinder();
			var slackList = slackFinder.FindAll().ToList();
		}
	}
}

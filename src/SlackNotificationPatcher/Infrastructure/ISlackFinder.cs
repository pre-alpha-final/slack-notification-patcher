using System.Collections.Generic;

namespace SlackNotificationPatcher.Infrastructure
{
	interface ISlackFinder
	{
		IEnumerable<FileInfo> FindAll();
	}
}

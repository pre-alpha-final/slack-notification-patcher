namespace SlackNotificationPatcher.Infrastructure
{
	interface IPatcher
	{
		void Patch(FileInfo fileInfo);
	}
}

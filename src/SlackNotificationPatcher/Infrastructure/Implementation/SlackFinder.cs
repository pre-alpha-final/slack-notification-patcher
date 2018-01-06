using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SlackNotificationPatcher.Infrastructure.Implementation
{
	class SlackFinder : ISlackFinder
	{
		public IEnumerable<FileInfo> FindAll()
		{
			var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			var executables = Directory.EnumerateFiles($"{appDataFolder}\\slack", "slack.exe", SearchOption.AllDirectories)
				.Where(e => e.Contains("app-"))
				.ToList();
			return executables.Select(e => new FileInfo
			{
				Path = e,
			});
		}
	}
}

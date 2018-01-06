using System.Text.RegularExpressions;

namespace SlackNotificationPatcher.Infrastructure
{
	class FileInfo
	{
		public string Path { get; set; }
		public string Version { get; set; }

		public FileInfo(string path)
		{
			Path = path;

			var versionRegex = new Regex(@"app-([^\\]*)");
			Version = versionRegex.Match(path)?.Groups[1]?.Value;
		}
	}
}

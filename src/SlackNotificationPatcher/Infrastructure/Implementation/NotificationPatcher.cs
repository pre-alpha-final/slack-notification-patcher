using System;
using System.Collections.Generic;
using System.IO;

namespace SlackNotificationPatcher.Infrastructure.Implementation
{
	class NotificationPatcher : IPatcher
	{
		static readonly byte[] OriginalOpCode = { 0xC7, 0x44, 0x24, 0x34, 0x04, 0x00, 0x00, 0x00, 0x1B, 0xC9, 0x83, 0xC1, 0x03 };
		static readonly byte[] PatchedOpCode = { 0xC7, 0x44, 0x24, 0x34, 0xFF, 0xFF, 0xFF, 0x7F, 0x1B, 0xC9, 0x83, 0xC1, 0x0F };
		const string PatchSuffix = "_patched";
		const string BackupSuffix = ".bak";

		public void Patch(FileInfo fileInfo)
		{
			if (FileInUse(fileInfo.Path))
			{
				Console.WriteLine($"Version: {fileInfo.Version} - IOException (Slack running?)");
				return;
			}

			if (AlreadyPatched(fileInfo.Path))
			{
				Console.WriteLine($"Version: {fileInfo.Version} - Already patched");
				return;
			}

			using (var reader = new BinaryReader(new FileStream(fileInfo.Path, FileMode.Open, FileAccess.Read)))
			{
				using (var writer = new BinaryWriter(new FileStream($"{fileInfo.Path}{PatchSuffix}", FileMode.Create)))
				{
					BinaryUtility.Replace(reader, writer, new List<Tuple<byte[], byte[]>>()
					{
						Tuple.Create(OriginalOpCode, PatchedOpCode),
					});
				}
			}
			File.Replace($"{fileInfo.Path}{PatchSuffix}", fileInfo.Path, $"{fileInfo.Path}{BackupSuffix}");
			Console.WriteLine($"Version: {fileInfo.Version} - Patched successfully");
		}

		private bool FileInUse(string path)
		{
			try
			{
				using (var reader = new BinaryReader(new FileStream(path, FileMode.Open)))
				{
					return false;
				}
			}
			catch (IOException)
			{
				return true;
			}
		}

		private bool AlreadyPatched(string path)
		{
			using (var reader = new BinaryReader(new FileStream(path, FileMode.Open)))
			{
				return BinaryUtility.Contains(reader, PatchedOpCode);
			}
		}
	}
}

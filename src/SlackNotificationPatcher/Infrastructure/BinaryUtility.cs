using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SlackNotificationPatcher.Extensions;

namespace SlackNotificationPatcher.Infrastructure
{
	/*
	 * Modified https://stackoverflow.com/a/28607981
	 * as the original would break if data was on the border
	 * between two 1024 blocks 
	 * 
	 * also adding Contains(...)
	 */
	public static class BinaryUtility
	{
		public static IEnumerable<byte> GetByteStream(BinaryReader reader)
		{
			//const int bufferSize = 1024;
			//byte[] buffer;
			//do
			//{
			//	buffer = reader.ReadBytes(bufferSize);
			//	foreach (var d in buffer) { yield return d; }
			//} while (bufferSize == buffer.Length);
			return reader.ReadAllBytes();
		}

		public static void Replace(BinaryReader reader, BinaryWriter writer, IEnumerable<Tuple<byte[], byte[]>> searchAndReplace)
		{
			foreach (byte d in Replace(GetByteStream(reader), searchAndReplace)) { writer.Write(d); }
		}

		public static IEnumerable<byte> Replace(IEnumerable<byte> source, IEnumerable<Tuple<byte[], byte[]>> searchAndReplace)
		{
			foreach (var s in searchAndReplace)
			{
				source = Replace(source, s.Item1, s.Item2);
			}
			return source;
		}

		public static IEnumerable<byte> Replace(IEnumerable<byte> input, IEnumerable<byte> from, IEnumerable<byte> to)
		{
			var fromEnumerator = from.GetEnumerator();
			fromEnumerator.MoveNext();
			int match = 0;
			foreach (var data in input)
			{
				if (data == fromEnumerator.Current)
				{
					match++;
					if (fromEnumerator.MoveNext()) { continue; }
					foreach (byte d in to) { yield return d; }
					match = 0;
					fromEnumerator.Reset();
					fromEnumerator.MoveNext();
					continue;
				}
				if (0 != match)
				{
					foreach (byte d in from.Take(match)) { yield return d; }
					match = 0;
					fromEnumerator.Reset();
					fromEnumerator.MoveNext();
				}
				yield return data;
			}
			if (0 != match)
			{
				foreach (byte d in from.Take(match)) { yield return d; }
			}
		}

		public static bool Contains(BinaryReader reader, IEnumerable<byte> sequence)
		{
			int match = 0;
			var input = GetByteStream(reader);
			var sequenceEnumerator = sequence.GetEnumerator();
			sequenceEnumerator.MoveNext();

			foreach (var data in input)
			{
				if (data == sequenceEnumerator.Current)
				{
					match++;
					if (sequenceEnumerator.MoveNext()) { continue; }
					return true;
				}
				if (0 != match)
				{
					match = 0;
					sequenceEnumerator.Reset();
					sequenceEnumerator.MoveNext();
				}
			}

			return false;
		}
	}
}

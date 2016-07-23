using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace bedrock.util
{
	public static class LoggingHelper
	{
		public static int? MainThreadManagedId;

		public static void AppendWithLimit(StringBuilder builder, string targetSite, string line)
		{
			if (builder == null)
				return;

			lock (builder)
			{
				const int limit = 10000;
				if (builder.Length >= limit)
				{
					builder.Remove(0, (int)(limit * 0.8)); // remove first 80% of buffer
				}

				string timestamp = DateTime.UtcNow.ToString("dd.MM.yyyy hh:mm:ss.ffffff");
				int threadId = Thread.CurrentThread.ManagedThreadId;
				string thread = threadId.ToString() + (MainThreadManagedId.HasValue && threadId == MainThreadManagedId.Value ? "*" : " ");

				builder.AppendLine($"{timestamp} {thread} {targetSite} {line}");
			}
		}
	}
}

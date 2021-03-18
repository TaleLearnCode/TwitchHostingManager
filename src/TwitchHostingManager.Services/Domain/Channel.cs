using Azure;
using Azure.Data.Tables;
using System;

namespace TaleLearnCode.TwitchHostingManager.Domain
{

	public class Channel : ITableEntity
	{

		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }

		public string UserId { get; set; } // Partition Key
		public string ChannelName { get; set; } // Row Key
		public string ChannelId { get; set; }
		public int SortOrder { get; set; }
		public bool IsDeleted { get; set; } = false;

	}

}
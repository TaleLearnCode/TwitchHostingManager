using Azure.Data.Tables;
using System;
using System.Linq;
using TaleLearnCode.TwitchHostingManager.Domain;

namespace TaleLearnCode.TwitchHostingManager.Services
{

	public class ChannelServices : IChannelServices
	{

		private TableClient _tableClient;

		public ChannelServices(TableClient tableClient)
		{
			_tableClient = tableClient;
		}

		public void AddChannel(string userId, string channelName, int sortOrder)
		{

			if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"The {nameof(userId)} argument requires a value.", nameof(userId));
			if (string.IsNullOrWhiteSpace(channelName)) throw new ArgumentException($"The {nameof(channelName)} argument requires a value.", nameof(channelName));
			if (sortOrder <= 0) throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, $"The {nameof(sortOrder)} argument must be greater than 0.");

			Channel channel = new Channel()
			{
				PartitionKey = userId,
				RowKey = channelName,
				UserId = userId,
				ChannelName = channelName,
				SortOrder = sortOrder
			};

			_tableClient.UpsertEntity(channel);

		}

		public void RemoveChannel(string userId, string channelName)
		{

			if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"The {nameof(userId)} argument requires a value.", nameof(userId));
			if (string.IsNullOrWhiteSpace(channelName)) throw new ArgumentException($"The {nameof(channelName)} argument requires a value.", nameof(channelName));

			Channel channel = GetChannel(userId, channelName);
			if (channel == default)
				throw new Exception($"The '{channelName}' is not currently in the list of channel to host.");



			channel.IsDeleted = true;
			_tableClient.UpsertEntity(channel);

		}

		public Channel GetChannel(string userId, string channelName, bool includeDeleted = false)
		{

			if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"The {nameof(userId)} argument requires a value.", nameof(userId));
			if (string.IsNullOrWhiteSpace(channelName)) throw new ArgumentException($"The {nameof(channelName)} argument requires a value.", nameof(channelName));

			// TODO: Better handling for 404 errors

			return _tableClient
				.Query<Channel>(t => t.PartitionKey == userId && t.RowKey == channelName && (includeDeleted || t.IsDeleted == false))
				.SingleOrDefault();

		}

	}

}
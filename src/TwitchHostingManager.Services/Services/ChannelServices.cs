using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaleLearnCode.TwitchHostingManager.Helpers;
using TwitchLib.Api;
using TwitchLib.Api.V5.Models.Channels;
using TwitchLib.Api.V5.Models.Search;

namespace TaleLearnCode.TwitchHostingManager.Services
{

	public class ChannelServices : IChannelServices
	{

		private TableClient _tableClient;
		private TwitchAPI _twitchAPI;

		public ChannelServices(TableClient tableClient, TwitchAPI twitchAPI)
		{
			_tableClient = tableClient;
			_twitchAPI = twitchAPI;
		}

		public async Task AddChannelAsync(string userId, string channelName, int sortOrder)
		{

			if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"The {nameof(userId)} argument requires a value.", nameof(userId));
			if (string.IsNullOrWhiteSpace(channelName)) throw new ArgumentException($"The {nameof(channelName)} argument requires a value.", nameof(channelName));
			if (sortOrder <= 0) throw new ArgumentOutOfRangeException(nameof(sortOrder), sortOrder, $"The {nameof(sortOrder)} argument must be greater than 0.");

			string channelId = await GetChannelId(channelName);
			if (string.IsNullOrWhiteSpace(channelId)) throw new Exception($"Unable to get Twitch channel id for '{channelName}'");

			Domain.Channel channel = new()
			{
				PartitionKey = userId,
				RowKey = channelName,
				UserId = userId,
				ChannelName = channelName,
				SortOrder = sortOrder,
				ChannelId = channelId
			};

			_tableClient.UpsertEntity(channel);

		}

		public void RemoveChannel(string userId, string channelName)
		{

			if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"The {nameof(userId)} argument requires a value.", nameof(userId));
			if (string.IsNullOrWhiteSpace(channelName)) throw new ArgumentException($"The {nameof(channelName)} argument requires a value.", nameof(channelName));

			Domain.Channel channel = GetChannel(userId, channelName);
			if (channel == default)
				throw new Exception($"The '{channelName}' is not currently in the list of channel to host.");



			channel.IsDeleted = true;
			_tableClient.UpsertEntity(channel);

		}

		public Domain.Channel GetChannel(string userId, string channelName, bool includeDeleted = false)
		{

			if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"The {nameof(userId)} argument requires a value.", nameof(userId));
			if (string.IsNullOrWhiteSpace(channelName)) throw new ArgumentException($"The {nameof(channelName)} argument requires a value.", nameof(channelName));

			// TODO: Better handling for 404 errors

			return _tableClient
				.Query<Domain.Channel>(t => t.PartitionKey == userId && t.RowKey == channelName && (includeDeleted || t.IsDeleted == false))
				.SingleOrDefault();

		}

		public void HostChannel(string channel, string channelToHost)
		{
			TwitchBot twitchBot = new TwitchBot(channel, GetAccessToken(channel));
			twitchBot.SendMessage($".host {channelToHost}");
		}

		private string GetAccessToken(string channel)
		{
			// TODO: Get the access token from Key Vault
			return Environment.GetEnvironmentVariable($"TwitchAccessToken_{channel}");
		}

		public async Task<string> GetChannelStatusAsync(string channelName)
		{
			var channel = await _twitchAPI.V5.Channels.GetChannelByIDAsync(await GetChannelId(channelName));
			return channel.Status;
		}


		public Task<bool> IsChannelBroadcasting(string channelId)
		{
			return _twitchAPI.V5.Streams.BroadcasterOnlineAsync(channelId);
		}

		public async Task<string> GetChannelId(string channelName)
		{
			SearchChannels results = await _twitchAPI.V5.Search.SearchChannelsAsync(channelName);
			if (results.Channels.Any())
			{
				foreach (var channel in results.Channels)
				{
					if (channel.Name == channelName)
						return channel.Id;
				}
			}
			return default;
		}

		public async Task<TwitchLib.Api.V5.Models.Channels.Channel> GetTwitchChannelDetails(string channelId)
		{
			return await _twitchAPI.V5.Channels.GetChannelByIDAsync(channelId);
		}

		public async Task<TwitchLib.Api.V5.Models.Teams.Team[]> GetTwitchChannelTeams(string channelId)
		{
			ChannelTeams teams = await _twitchAPI.V5.Channels.GetChannelTeamsAsync(channelId);
			if (teams != default && teams.Teams.Any())
				return teams.Teams;
			else
				return default;
		}

		public List<Domain.Channel> GetChannels(string userId, bool includeDeleted = false)
		{
			if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException($"The {nameof(userId)} argument requires a value.", nameof(userId));

			// TODO: Better handling for 404 errors

			var results = _tableClient
				.Query<Domain.Channel>(t => t.PartitionKey == userId && (includeDeleted || t.IsDeleted == false))
				.ToList();

			results.Sort(delegate (Domain.Channel x, Domain.Channel y)
			{
				if (x.SortOrder == null && y.SortOrder == null) return 0;
				else if (x.SortOrder == null) return -1;
				else if (y.SortOrder == null) return 1;
				else if (x.SortOrder > y.SortOrder) return 1;
				else return -1;
			});

			return results;

			//parts.Sort(delegate (Part x, Part y)
			//{
			//	if (x.PartName == null && y.PartName == null) return 0;
			//	else if (x.PartName == null) return -1;
			//	else if (y.PartName == null) return 1;
			//	else return x.PartName.CompareTo(y.PartName);
			//});

		}
	}

}
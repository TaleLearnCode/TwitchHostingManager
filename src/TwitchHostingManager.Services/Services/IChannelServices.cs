using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.TwitchHostingManager.Domain;

namespace TaleLearnCode.TwitchHostingManager.Services
{
	public interface IChannelServices
	{

		Task AddChannelAsync(string userId, string channelName, int sortOrder);
		Channel GetChannel(string userId, string channelName, bool includeDeleted);
		void RemoveChannel(string userId, string channelName);
		List<Channel> GetChannels(string userId, bool includeDeleted);

		void HostChannel(string userId, string channelName);

		Task<bool> IsChannelBroadcasting(string channelId);

		Task<string> GetChannelId(string channelName);

		Task<TwitchLib.Api.V5.Models.Teams.Team[]> GetTwitchChannelTeams(string channelId);

	}

}
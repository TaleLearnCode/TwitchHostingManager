using TaleLearnCode.TwitchHostingManager.Domain;

namespace TaleLearnCode.TwitchHostingManager.Services
{
	public interface IChannelServices
	{
		void AddChannel(string userId, string channelName, int sortOrder);
		Channel GetChannel(string userId, string channelName, bool includeDeleted);
		void RemoveChannel(string userId, string channelName);

		void HostChannel(string userId, string channelName);
	}
}
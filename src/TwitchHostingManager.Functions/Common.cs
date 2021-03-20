using System;
using TaleLearnCode.TwitchHostingManager.Helpers;
using TaleLearnCode.TwitchHostingManager.Services;
using TwitchLib.Api;

namespace TwitchHostingManager.Functions
{

	public static class Common
	{

		public static IChannelServices GetChannelServices()
		{

			AzureStorageSettings azureStorageSettings = new()
			{
				AccountKey = Environment.GetEnvironmentVariable("Storage_AccountKey"),
				AccountName = Environment.GetEnvironmentVariable("Storage_AccountName"),
				Url = Environment.GetEnvironmentVariable("Storage_Url")
			};

			TwitchAPI twitchAPI = new();
			twitchAPI.Settings.ClientId = Environment.GetEnvironmentVariable("TwitchHostingManager_ClientId");
			twitchAPI.Settings.AccessToken = Environment.GetEnvironmentVariable("TwitchHostingManager_AccessToken");

			return new ChannelServices(AzureStorageHelper.GetTableClient(azureStorageSettings, "channels"), twitchAPI);


		}


	}

}
using System;
using TaleLearnCode.TwitchHostingManager.Helpers;
using TaleLearnCode.TwitchHostingManager.Services;

namespace TestClient
{
	class Program
	{

		static void Main()
		{
			AzureStorageSettings azureStorageSettings = new()
			{
				AccountKey = Settings.AccountKey,
				AccountName = Settings.AccountName,
				Url = Settings.Url
			};
			IChannelServices channelServices = new ChannelServices(AzureStorageHelper.GetTableClient(azureStorageSettings, "channels"));


			RemoveChannel(channelServices);
		}


		static void AddChannel(IChannelServices channelServices)
		{
			channelServices.AddChannel("chad.green@talelearncode.com", "TaleLearnCode", 10);
			Console.WriteLine("Channel added");
		}

		static void RemoveChannel(IChannelServices channelServices)
		{
			channelServices.RemoveChannel("chad.green@talelearncode.com", "TaleLearnCode");
			Console.WriteLine("Channel removed");
		}



	}

}
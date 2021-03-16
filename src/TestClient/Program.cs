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


			HostChannel(channelServices);
			//Console.WriteLine("Hosted channel");
			//Console.ReadLine();

			//TwitchBot twitchBot = new("thelegomaestro");
			////Console.WriteLine("Hit enter to send message");
			////Console.ReadLine();
			//twitchBot.SendMessage("Does this work now?");
			////Console.ReadLine();
			////Console.WriteLine("Done");


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

		static void HostChannel(IChannelServices channelServices)
		{
			channelServices.HostChannel("thelegomaestro", "talelearncode");
		}



	}

}
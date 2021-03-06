using System;
using System.Threading.Tasks;
using TaleLearnCode.TwitchHostingManager.Helpers;
using TaleLearnCode.TwitchHostingManager.Services;
using TwitchLib.Api;

namespace TestClient
{
	class Program
	{

		static async Task Main()
		{
			AzureStorageSettings azureStorageSettings = new()
			{
				AccountKey = Settings.AccountKey,
				AccountName = Settings.AccountName,
				Url = Settings.Url
			};

			TwitchAPI twitchAPI = new();
			twitchAPI.Settings.ClientId = Environment.GetEnvironmentVariable("TwitchHostingManager_ClientId");
			twitchAPI.Settings.AccessToken = Environment.GetEnvironmentVariable("TwitchHostingManager_AccessToken");


			IChannelServices channelServices = new ChannelServices(AzureStorageHelper.GetTableClient(azureStorageSettings, "channels"), twitchAPI);

			//await GetChannelTeams(channelServices);
			//await IsChannelBroadcasting(channelServices);
			//await GetChannelStatus("tlcoperator");

			//HostChannel(channelServices);
			//Console.WriteLine("Hosted channel");
			//Console.ReadLine();

			//TwitchBot twitchBot = new("thelegomaestro");
			////Console.WriteLine("Hit enter to send message");
			////Console.ReadLine();
			//twitchBot.SendMessage("Does this work now?");
			////Console.ReadLine();
			////Console.WriteLine("Done");

			//await AddChannelAsync(channelServices);

			await GetChannelStatus(channelServices);

			//await AddChannelAsync(channelServices);

			//GetChannels(channelServices);



		}


		static async Task AddChannelAsync(IChannelServices channelServices)
		{
			await channelServices.AddChannelAsync("thelegomaestro", "codewithsean", 10);
			await channelServices.AddChannelAsync("thelegomaestro", "talelearncode", 20);
			await channelServices.AddChannelAsync("thelegomaestro", "callowcreation", 30);
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

		static async Task IsChannelBroadcasting(IChannelServices channelServices)
		{
			Console.WriteLine($"TaleLearnCode: {await channelServices.IsChannelBroadcasting(await channelServices.GetChannelId("talelearncode"))}");
			Console.WriteLine($"BricksWithChad: {await channelServices.IsChannelBroadcasting(await channelServices.GetChannelId("brickswithchad"))}");
		}

		static async Task GetChannelTeams(IChannelServices channelServices)
		{
			var teams = await channelServices.GetTwitchChannelTeams(await channelServices.GetChannelId("talelearncode"));
			foreach (var team in teams)
			{
				Console.WriteLine(team.DisplayName);
			}
		}

		static async Task GetChannelStatus(IChannelServices channelServices)
		{
			Console.WriteLine(await channelServices.GetChannelStatusAsync("thelegomaestro"));
		}

		static void GetChannels(IChannelServices channelServices)
		{
			var list = channelServices.GetChannels("thelegomaestro", false);
			foreach (var item in list)
			{
				Console.WriteLine(item.ChannelName);
			}
		}

	}

}
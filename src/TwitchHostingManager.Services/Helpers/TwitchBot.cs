using System.Diagnostics;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TaleLearnCode.TwitchHostingManager.Helpers
{
	public class TwitchBot
	{

		private TwitchClient _twitchClient = new TwitchClient();
		private string _destinationChannel;

		private bool _joinedChannel;

		public TwitchBot(string destinationChannel, string accessToken)
		{
			_destinationChannel = destinationChannel;
			ConnectionCredentials credentials = new ConnectionCredentials(destinationChannel, accessToken);
			_twitchClient.OnMessageSent += TwitchClient_OnMessageSent;
			_twitchClient.OnJoinedChannel += TwitchClient_OnJoinedChannel;
			_twitchClient.Initialize(credentials, _destinationChannel);
			_twitchClient.Connect();
		}

		public void SendMessage(string message)
		{
			bool sendMessage = true;
			Stopwatch stopwatch = Stopwatch.StartNew();
			// Need to wait for the connection to be made; waiting for up to 5 seconds for that to happen
			while (!_joinedChannel && sendMessage)
				if (stopwatch.ElapsedMilliseconds >= 5000) sendMessage = false;

			if (sendMessage) _twitchClient.SendMessage(_destinationChannel, message);
		}

		private void TwitchClient_OnMessageSent(object sender, OnMessageSentArgs e)
		{
			if (_twitchClient.IsConnected) _twitchClient.Disconnect();
		}

		private void TwitchClient_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
		{
			_joinedChannel = true;
		}

	}

}
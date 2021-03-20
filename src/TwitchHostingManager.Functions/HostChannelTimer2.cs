using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using TaleLearnCode.TwitchHostingManager.Domain;
using TaleLearnCode.TwitchHostingManager.Services;

namespace TwitchHostingManager.Functions
{
	public static class HostChannelTimer2
	{
		[Function("HostChannelTimer2")]
		public static async System.Threading.Tasks.Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
				FunctionContext executionContext)
		{
			//var logger = executionContext.GetLogger("HostChannelTimer2");
			//logger.LogInformation("C# HTTP trigger function processed a request.");

			//var response = req.CreateResponse(HttpStatusCode.OK);
			//response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

			//response.WriteString("Welcome to Azure Functions!");

			//return response;

			var logger = executionContext.GetLogger("HostChannelTimer2");
			IChannelServices channelServices = Common.GetChannelServices();

			if (!await channelServices.IsChannelBroadcasting(await channelServices.GetChannelId("thelegomaestro")))
			{
				List<Channel> potentialHostingChannels = channelServices.GetChannels("thelegomaestro", false);
				//logger.LogInformation($"Potential Hosting Channels: {potentialHostingChannels.Count}");
				foreach(Channel channel in potentialHostingChannels)
				{
					if (await channelServices.IsChannelBroadcasting(channel.ChannelId))
					{
						channelServices.HostChannel("thelegomaestro", channel.ChannelId);
						logger.LogInformation($"TheLegoMaestro is now hosting {channel.ChannelName}");
						break;
					}
				}
			}


			return req.CreateResponse(HttpStatusCode.OK);

		}
	}
}

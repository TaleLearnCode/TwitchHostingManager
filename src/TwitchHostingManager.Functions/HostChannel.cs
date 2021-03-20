using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using TaleLearnCode.TwitchHostingManager.Services;

namespace TwitchHostingManager.Functions
{
	public class HostChannel
	{


		[Function("HostChannel")]
		public async System.Threading.Tasks.Task<HttpResponseData> RunAsync(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
			FunctionContext executionContext)
		{

			IChannelServices _channelServices = Common.GetChannelServices();

			var logger = executionContext.GetLogger("HostChannel");

			TaleLearnCode.TwitchHostingManager.RequestBody.HostChannel hostChannel = await JsonSerializer.DeserializeAsync<TaleLearnCode.TwitchHostingManager.RequestBody.HostChannel>(req.Body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
			if (hostChannel == default && string.IsNullOrWhiteSpace(hostChannel.Channel) && string.IsNullOrWhiteSpace(hostChannel.ChannelToHost))
				return req.CreateResponse(HttpStatusCode.BadRequest);

			_channelServices.HostChannel(hostChannel.Channel, hostChannel.ChannelToHost);
			logger.LogInformation($"{hostChannel.Channel} has hosted {hostChannel.ChannelToHost}");
			return req.CreateResponse(HttpStatusCode.OK);

		}
	}
}

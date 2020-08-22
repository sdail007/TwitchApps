using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.Endpoints;
using TwitchAPI.Query;
using TwitchAPI.Query.GetChannelInfo;

namespace TwitchAPI.V5.Queries.GetChannelInfo
{
    public class GetChannelInfoQueryHandler : KrakenQueryHandler<GetChannelInfoQuery, GetChannelInfoResponse>
    {
        public GetChannelInfoQueryHandler(KrakenEndpoint endpoint) : base(endpoint)
        {
        }

        public override async Task<GetChannelInfoResponse> HandleAsync(GetChannelInfoQuery query)
        {
            JsonValue document = await Endpoint.GetAsync("channel");

            string idString = document["_id"];
            string displayName = document["display_name"];
            string game = document["game"];
            string status = document["status"];
            string logo = document["logo"];

            int id = Int32.Parse(idString);

            var channelInfo = new ChannelInfo(id, displayName, game, status, new Uri(logo));

            return new GetChannelInfoResponse(channelInfo);
        }
    }
}

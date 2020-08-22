using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.Command;
using TwitchAPI.Endpoints;

namespace TwitchAPI.V5.Commands.UpdateChannelInfo
{
    public class UpdateChannelInfoCommandHandler : KrakenCommandHandler<UpdateChannelInfoCommand>
    {
        public UpdateChannelInfoCommandHandler(KrakenEndpoint endpoint) : base(endpoint)
        {
        }

        public override async Task HandleAsync(UpdateChannelInfoCommand command)
        {
            string contentString = @"{""channel"": {""status"": """ + command.Status + @""", ""game"": """ + command.Game + @"""}}";
            JsonValue content = JsonValue.Parse(contentString);

            JsonValue response = await Endpoint.PutAsync($"channels/{command.ChannelID}", content);
        }
    }
}

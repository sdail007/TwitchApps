using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.Endpoints;
using TwitchAPI.Query;
using TwitchAPI.Query.GetGames;

namespace TwitchAPI.V5.Queries.GetGames
{
    public class GetGamesByNameQueryHandler : KrakenQueryHandler<GetGamesByNameQuery, GetGamesByNameResponse>
    {
        public GetGamesByNameQueryHandler(KrakenEndpoint endpoint) : base (endpoint)
        {
        }

        public override async Task<GetGamesByNameResponse> HandleAsync(GetGamesByNameQuery query)
        {
            string searchString = WebUtility.UrlEncode(query.SearchString);

            string queryString = $"search/games?query={searchString}";

            JsonValue jsonDocument = await Endpoint.GetAsync(queryString);

            List<Game> matches = new List<Game>();

            JsonValue games = jsonDocument["games"];

            if (games != null)
            {
                foreach (JsonValue game in games)
                {
                    Game match = new Game(game["name"], new Uri(game["box"]["large"]));
                    matches.Add(match);
                }
            }

            return new GetGamesByNameResponse(matches);
        }
    }
}

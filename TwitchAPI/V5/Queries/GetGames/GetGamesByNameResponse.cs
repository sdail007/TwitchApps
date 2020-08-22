using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Query.GetGames
{
    public class GetGamesByNameResponse : ResponseBase
    {
        public List<Game> Games { get; private set; }

        public GetGamesByNameResponse(params Game[] games)
        {
            Games = new List<Game>(games);
        }

        public GetGamesByNameResponse(IEnumerable<Game> games)
        {
            Games = new List<Game>(games);
        }
    }
}

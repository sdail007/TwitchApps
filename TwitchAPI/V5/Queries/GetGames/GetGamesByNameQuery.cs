using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.Endpoints;

namespace TwitchAPI.Query.GetGames
{
    public class GetGamesByNameQuery : QueryBase<GetGamesByNameResponse>
    {
        public string SearchString { get; private set; }

        public GetGamesByNameQuery(string searchString)
        {
            SearchString = searchString;
        }       
    }
}

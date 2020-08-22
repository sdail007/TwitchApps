using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Endpoints
{
    public class AuthEndpoint : Endpoint
    {
        protected override string Uri => "https://id.twitch.tv/oauth2";

        public AuthEndpoint(TwitchClient application) : base(application)
        {
        }
    }
}

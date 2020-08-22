using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI;
using TwitchAPI.Endpoints;
using TwitchAPI.Query;
using TwitchAPI.V5;

namespace TwitchAPI
{
    //TODO: put in app config
    public class TwitchClient
    {
        public string ID { get; }

        public string Secret { get; }

        public string RegisteredRedirectUri { get; } 

        public string[] RequiredScopes { get; }

        internal KrakenEndpoint Kraken { get; }

        internal AuthEndpoint Auth { get; }

        public Authenticator Authenticator { get; }

        public Kraken V5 { get; }

        static TwitchClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }
        public TwitchClient(string clientID, string clientSecret, string registeredRedirectUri, string[] requiredScopes)
        {
            ID = clientID;
            Secret = clientSecret;
            RegisteredRedirectUri = registeredRedirectUri;
            RequiredScopes = requiredScopes;

            Auth = new AuthEndpoint(this);
            Authenticator = new Authenticator(this);
            Kraken = new KrakenEndpoint(this, Authenticator);

            V5 = new Kraken(Kraken);
        }
    }
}

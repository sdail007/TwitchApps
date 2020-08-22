using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Linq;
using System.Web;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.Globalization;

namespace TwitchAPI
{
    public static class Twitch
    {
        static Twitch()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;           
        }

        public static TwitchClient Start(string applicationName)
        {
            //look in app config for name, and return result
            return null; //new TwitchClient();
        }

        public static TwitchClient Start(string clientID, string clientSecret, string registeredRedirectUri, string[] requiredScopes)
        {
            return new TwitchClient(clientID, clientSecret, registeredRedirectUri, requiredScopes);
        }
    }
}

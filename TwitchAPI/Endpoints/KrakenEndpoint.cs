using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Endpoints
{
    public class KrakenEndpoint : Endpoint
    {
        private readonly Authenticator authenticator;

        protected override string Uri => $"https://api.twitch.tv/kraken";

        public KrakenEndpoint(TwitchClient application, Authenticator authenticator) : base(application)
        {
            this.authenticator = authenticator;

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.twitchtv.v5+json"));
            
            authenticator.TokenUpdated += Authenticator_TokenUpdated;
            
            if(authenticator.Token != null)
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authenticator.Token);            
        }

        private void Authenticator_TokenUpdated(object sender, EventArgs e)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authenticator.Token);
        }

        public override async Task<JsonValue> GetAsync(string queryString)
        {
            try
            {
                JsonValue value = await base.GetAsync(queryString);
                return value;
            }
            catch(UnauthorizedAccessException ex)
            {
                authenticator.Handle(ex);
                throw;
            }
        }

        public override async Task<JsonValue> PostAsync(string queryString, string endpoint = "")
        {
            try
            {
                JsonValue value = await base.PostAsync(queryString, endpoint);
                return value;
            }
            catch (UnauthorizedAccessException ex)
            {
                authenticator.Handle(ex);
                throw;
            }
        }
    }
}

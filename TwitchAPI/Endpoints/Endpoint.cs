using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TwitchAPI.Endpoints
{
    public abstract class Endpoint
    {
        protected readonly HttpClient httpClient;

        protected abstract string Uri { get; }

        public TwitchClient Application { get; }

        public Endpoint(TwitchClient authenticator)
        {
            Application = authenticator;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Client-ID", Application.ID);
        }

        public virtual async Task<JsonValue> GetAsync(string queryString)
        {
            //queryString = WebUtility.UrlEncode(queryString);

            var response = await httpClient.GetAsync($"{Uri}/{queryString}");
            
            ThrowOnUnauthorized(response);

            return GetJson(response);
        }

        public virtual async Task<JsonValue> PostAsync(string queryString, string endpoint = "")
        {
            //queryString = WebUtility.UrlEncode(queryString);

            string url = $"{Uri}/{endpoint}{queryString}";
            var response = await httpClient.PostAsync(url, new StringContent(""));

            ThrowOnUnauthorized(response);

            return GetJson(response);
        }

        public virtual async Task<JsonValue> PutAsync(string queryString, JsonValue content)
        {
            //queryString = WebUtility.UrlEncode(queryString);

            string contentString = content.ToString();
            StringContent stringContent = new StringContent(contentString, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"{Uri}/{queryString}", stringContent);

            ThrowOnUnauthorized(response);

            return GetJson(response);
        }

        private static void ThrowOnUnauthorized(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                JsonValue body = GetJson(response);
                string message = body["message"];
                //"""{\"error\":\"Unauthorized\",\"status\":401,\"message\":\"invalid oauth token\"}"""
                //""{\"error\":\"Unauthorized\",\"status\":401,\"message\":\"authentication failed\"}""
                throw new UnauthorizedAccessException(message);
            }
        }

        private static JsonValue GetJson(HttpResponseMessage response)
        {
            string content = response.Content.ReadAsStringAsync().Result;

            JsonValue jsonDocument = JsonValue.Parse(content);
            return jsonDocument;
        }
    }
}



using System;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TwitchAPI.Utilities;

namespace TwitchAPI
{
    public class Authenticator
    {
        private string FileName => $"{Client.ID}-Token.json";

        private Token _Token;

        public Token Token 
        {
            get
            {
                if (File.Exists(FileName))
                {
                    using (var fs = new FileStream(FileName, FileMode.Open))
                    {
                        try
                        {
                            _Token = (Token)serializer.ReadObject(fs);
                        }
                        catch (SerializationException)
                        {
                        }
                    }
                }

                return _Token;
            }
            private set
            {
                _Token = value;
                SaveToken(_Token);
            }
        }

        private void SaveToken(Token token)
        {
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, token);

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    File.WriteAllText(FileName, json);
                }
            };
        }

        public bool Authenticated { get; private set; }

        public bool Authenticating { get; private set; }

        public TwitchClient Client { get; }

        private readonly HttpListener redirectServer = new HttpListener();

        private readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Token));

        public event EventHandler TokenUpdated;

        public Authenticator(TwitchClient client)
        {
            Client = client;

            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }

            redirectServer.Prefixes.Add(Client.RegisteredRedirectUri + "/");
        }

        public void BeginAuthentication()
        {
            if (Authenticated || Authenticating)
                return;

            Authenticating = true;

            redirectServer.Start();

            string state = "555555";

            RequestAuthentication(state);

            EndAuthentication(state)
                .ContinueWith(t =>
                {
                    Token = t.Result;

                    TokenUpdated?.Invoke(this, EventArgs.Empty);
                });
        }

        private void RequestAuthentication(string state)
        {
            string scopes = string.Join("+", Client.RequiredScopes);

            string url = @"https://id.twitch.tv/oauth2/authorize?"
                    + $"client_id={Client.ID}"
                    + $"&redirect_uri={Client.RegisteredRedirectUri}"
                    + @"&response_type=code"
                    + $"&scope={scopes}"
                    + $"&state={state}";

            Process.Start(url);
        }

        private async Task<Token> EndAuthentication(string state)
        {
            HttpListenerContext context = await Task.Run(() =>
            {
                return redirectServer.GetContext();
            });

            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string code = request.QueryString.Get("code");
            string returnState = request.QueryString.Get("state");

            if(state != returnState)
            {
                throw new UnauthorizedAccessException("State did not match return State");
            }

            Task<Token> tokenTask = FromCode(code);

            SendResponse(response);
            
            redirectServer.Stop();

            Token token = await tokenTask;
            
            Authenticating = false;
            Authenticated = true;

            return token;
        }

        private void SendResponse(HttpListenerResponse response)
        {
            string responseString = "<HTML><BODY>Welcome to Roguelette!  You can close this tab.</BODY></HTML>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);

            output.Close();
        }

        public void Handle(UnauthorizedAccessException ex)
        {
            if (ex.Message == "authentication failed")
            {
                BeginAuthentication();
            }
            else if(ex.Message == "invalid oauth token")
            {
                RefreshToken().ContinueWith(_ => { });
            }
        }

        public async Task RefreshToken()
        {
            string commandUrl = "token?grant_type=refresh_token"
                              +  $"&refresh_token={Token.RefreshToken}"
                              +  $"&client_id={Client.ID}"
                              +  $"&client_secret={Client.Secret}";

            JsonValue response = await Client.Auth.PostAsync(commandUrl);

            string accessToken = response["access_token"];
            string refreshToken = response["refresh_token"];
            int expiresIn = response["expires_in"];
            string[] scopes = response["scope"].ToArray();

            Token.Refresh(accessToken, refreshToken, expiresIn, scopes);
            SaveToken(_Token);

            TokenUpdated?.Invoke(this, EventArgs.Empty);
        }

        private async Task<Token> FromCode(string code)
        {
            string url = $"token?"
                       + $"client_id={Client.ID}"
                       + $"&client_secret={Client.Secret}"
                       + $"&code={code}"
                       + $"&grant_type=authorization_code"
                       + $"&redirect_uri={Client.RegisteredRedirectUri}";

            JsonValue response = await Client.Auth.PostAsync(url);

            string accessToken = response["access_token"];
            string refreshToken = response["refresh_token"];
            int expiresIn = response["expires_in"];
            string[] scopes = response["scope"].ToArray();

            Token token = new Token(accessToken, refreshToken, expiresIn, scopes);

            return token;
        }
    }
}

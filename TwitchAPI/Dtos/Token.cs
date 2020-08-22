using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TwitchAPI
{
    [DataContract]
    public class Token
    {
        [DataMember]
        public string Value { get; private set; }

        [DataMember]
        public string User { get; private set; }

        [DataMember]
        public int UserID { get; private set; }

        [DataMember]
        public int ExpiresIn { get; private set; }

        [DataMember]
        public string RefreshToken { get; private set; }

        [DataMember]
        public string[] Scopes { get; private set; }

        [DataMember]
        public DateTimeOffset ExpirationDate { get; private set; }

        public Token(string value, int expiresIn)
        {
            Value = value;
            ExpiresIn = expiresIn;

            ExpirationDate = DateTime.Now.AddSeconds(ExpiresIn);
        }

        public Token(string value, int expiresIn, int userID, string user) : this(value, expiresIn)
        {
            User = user;
            UserID = userID;
        }

        public Token(string value, string refreshToken, int expiresIn, string[] scopes)
        {
            Value = value;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
            Scopes = scopes;
        }

        public void Refresh(string value, string refreshToken, int expiresIn, string[] scopes)
        {
            Value = value;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
            Scopes = scopes;
        }

        public static implicit operator string(Token token)
        {
            return token.Value;
        }
    }
}

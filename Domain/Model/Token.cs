
using System;
using System.Configuration;

namespace Portal.Domain.Model
{
    [Serializable]
    public class Token 
    {
        public static readonly int ConfigExpired = int.Parse(ConfigurationManager.AppSettings["TokenExpriedTime"] ?? "5");
        private static readonly string _tokenCachedPrefix = "#Token";
        public Token(string identity, string tokenVal, DateTime start)
        {
            this.UserIdentity = identity;
            this.Value = tokenVal;
            this.StartTime = start;
        }

        public string UserIdentity { get; private set; }
        public string Value { get; private set; }
        public DateTime StartTime { get; private set; }

        public bool IsExpired()
        {
            //var span = DateTime.Now - this.StartTime;
            //return span.TotalMinutes > Token.ConfigExpired;
            return false;
        }

        public static string GetCacheTokenKey(string token)
        {
            return Token._tokenCachedPrefix + token;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Admin.Core
{
    public class TempTokenBiz
    {
        private static readonly object LockToken = new object();
        private const int ExpireMinutes = 5;
        public static List<TempToken> TempResponeToken = new List<TempToken>();

        private static void RemoveExpire()
        {
            lock (LockToken)
            {
                TempResponeToken.RemoveAll(p => p.CreateTime.AddMinutes(ExpireMinutes) < DateTime.Now);
            }
        }

        public static void Add(TempToken tempToken)
        {
            lock (LockToken)
            {
                TempResponeToken.Add(tempToken);
            }
        }

        public static bool IsExists(string authenticator)
        {
            RemoveExpire();
            bool result = TempResponeToken.Exists(p => p.Token.Equals(authenticator));
            return result;
        }

        public static TempToken GetTempToken(string authenticator)
        {
            RemoveExpire();
            return TempResponeToken.Find(p => p.Token.Equals(authenticator));
        }

        public static void RemoveByToken(string authenticator)
        {
            lock (LockToken)
            {
                TempResponeToken.RemoveAll(p => p.Token.Equals(authenticator));
            }
        }
    }

    public class TempToken
    {
        public string Token { get; set; }
        public string LoginName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

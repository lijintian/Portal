
using System.Web.Security;

namespace Portal.Domain.Aggregates.UserAgg.Strategies
{
    public class PasswordEncryptStrategy : IPasswordEncryptStrategy
    {
        public string Encrypt(string password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5").Substring(3, 0x17);
        }
    }
}

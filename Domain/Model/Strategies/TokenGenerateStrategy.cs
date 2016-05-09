using System;
using System.Text;
using System.Security.Cryptography;


namespace Portal.Domain.Model.Strategies
{
    public class TokenGenerateStrategy : ITokenGenerateStrategy
    {
        private const string TimeFormatStr = "yyyy-MM-dd HH:mm:ss";
        public Token Generate(string loginName)
        {
            var now = DateTime.Now;
            var token = new Token(loginName, this.MD5Encrypt(loginName + now.ToString(TokenGenerateStrategy.TimeFormatStr)), now);

            return token;
        }

        private string MD5Encrypt(string strSource)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(strSource);
            byte[] hs = md5.ComputeHash(bs);
            var sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}

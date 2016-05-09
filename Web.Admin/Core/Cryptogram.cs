using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Web.Admin.Core
{
    [Serializable()]
    public class Cryptogram : MarshalByRefObject
    {
        public static string MD5Encrypt(string strSource)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(strSource);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        public static string MD5Encrypt(string key, string strSource)
        {
            return MD5Encrypt(strSource + key);
        }

    }
}

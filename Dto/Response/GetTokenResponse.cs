using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto.Response
{
    /// <summary>
    /// 表示通过授权码或刷新Token获取亲的Token响应
    /// </summary>
    public class GetTokenResponse
    {
        public string AccessToken
        {
            get;
            set;
        }

        /// <summary>
        ///访问Token过期时间数，分钟
        /// </summary>
        public long AccessTokenExpiresIn
        {
            get;
            set;
        }

        /// <summary>
        /// 刷新Token过期时间数，分钟
        /// </summary>
        public long RefreshTokenExpiresIn
        {
            get;
            set;
        }

        public string RefreshToken
        {
            get;
            set;
        }

        public string CustomerId
        {
            get;
            set;
        }
    }
}

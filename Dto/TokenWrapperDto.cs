using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto
{
    /// <summary>
    /// 表示Token数据传输对象
    /// </summary>
    public class TokenWrapperDto : AggregateDto
    {
        /// <summary>
        /// 表示Access Token
        /// </summary>
        public string AccessToken
        {
            get;
            set;
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken
        {
            get;
            set;
        }

        /// <summary>
        /// Token类型，外部或内部
        /// </summary>
        public bool IsExternal
        {
            get;
            set;
        }

        /// <summary>
        /// 客户标识
        /// </summary>
        public string CustomerIdentity
        {
            get;
            set;
        }

        /// <summary>
        /// 表示开发者标识
        /// </summary>
        public string ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string DeveloperAppName { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime AccessTokenExpiredTime
        {
            get;
            set;
        }
        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired { get; set; } 

        /// <summary>
        /// 刷新Token过期时间
        /// </summary>
        public DateTime RefreshTokenExpiredTime
        {
            get;
            set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn
        {
            get;
            set;
        }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled
        {
            get;
            set;
        }

        public string[] Codes
        {
            get;
            set;
        }
    }
}

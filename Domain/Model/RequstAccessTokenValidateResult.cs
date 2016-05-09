using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.DeveloperAppAgg;
using Portal.Domain.Aggregates.TokenWrapperAgg;

namespace Portal.Domain.Model
{
    /// <summary>
    /// 表示请求Access Token验证结果
    /// </summary>
    public class RequstAccessTokenValidateResult
    {
        /// <summary>
        /// 要创建的Token权限
        /// </summary>
        public ReferenceApiPermssionInfo[] ApiPermssionInfos
        {
            get;
            set;
        }

        /// <summary>
        /// 第三方应用
        /// </summary>
        public DeveloperApp App
        {
            get;
            set;
        }

        /// <summary>
        /// 客户标识
        /// </summary>
        public string CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 授权类型
        /// </summary>
        public GrantType GrantType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 授权类型
    /// </summary>
    public enum GrantType
    {
        AuthorizationCode,
        RefreshToken
    }
}

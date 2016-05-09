using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.DeveloperAppAgg;

namespace Portal.Domain.Model
{
    /// <summary>
    /// 表示Oauth请求客户授权验证结果
    /// </summary>
    public class CustomerAuthValidateResult
    {
        /// <summary>
        /// 请求开发者应用
        /// </summary>
        public DeveloperApp App
        {
            get;
            set;
        }

        /// <summary>
        /// 授权范围
        /// </summary>
        public ReferenceApiPermssionInfo[] RefApiPermssionInfos
        {
            get;
            set;
        }

        /// <summary>
        /// 表示授权响应类型
        /// </summary>
        public ResponseType ResponseType
        {
            get;
            set;
        }
    }

    public enum ResponseType
    {
        Code,
        Token
    }
}

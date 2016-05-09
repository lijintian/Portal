using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.DeveloperAppAgg;

namespace Portal.Domain.Model
{
    /// <summary>
    /// 表示直接请求Token验证结果
    /// </summary>
    public class DirectGetAccessTokenValidateResult
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto
{
    /// <summary>
    /// 表示分页查询授权码请求
    /// </summary>
    public class FindAuthorizationCodesRequest : PagerFindRequest
    {
        #region 属性
        /// <summary>
        /// 表示授权码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 授权客户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 表示开发者标识
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 生成起始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 生成结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        #endregion
    }
}

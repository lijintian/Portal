using System;

namespace Portal.Dto
{
    /// <summary>
    /// 表示分页查询Token请求
    /// </summary>
    public class FindTokensRequest : PagerFindRequest
    {
        #region 属性
        /// <summary>
        /// 表示Access Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 所属客户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 表示开发者标识
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Token类型，外部或内部
        /// </summary>
        public bool? IsExternal { get; set; }

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

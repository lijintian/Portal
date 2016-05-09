using System;

namespace Portal.Dto
{
    public class FindDeveloperAppRequest : PagerFindRequest
    {
        #region 属性
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 应用类型,1网站应用,2桌面应用,3移动应用，参考枚举：DeveloperAppType
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 状态，1开发中，2审核中，3审核通过，参考枚举：DeveloperAppState
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 属性账号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 所属客户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 空表示所有，true表示外部，false表示内部
        /// </summary>
        public bool? IsExternal { get; set; }

        /// <summary>
        ///false表示所有，true表示未删除
        /// </summary>
        public bool IsUnDeleted { get; set; }

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

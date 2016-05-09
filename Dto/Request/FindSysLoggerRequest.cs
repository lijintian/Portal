/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志表查询参数类
*/
using System;

namespace Portal.Dto
{
    [Serializable]
    public class FindSysLoggerRequest : PagerFindRequest
    {
        #region 属性
        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 应用系统Id
        /// </summary>
        public string ApplicationName { get; set; }
        /// <summary>
        /// 操作人员
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 重要等级: 1Debug异常, 2Info提示, 3Warning警告, 4Error错误, 5Critical严重的
        /// </summary>
        public SysLoggerLevel? Level { get; set; }
        /// <summary>
        /// 1增, 2改, 3删, 4查, 5登录, 21异常, 22错误, 23性能日记, 24其它
        /// </summary>
        public SysLoggerType? Type { get; set; }
        /// <summary>
        /// 查看权限: 0所有人可见(包括客户), 1仅内部管理员可见
        /// </summary>
        public SysLoggerRight? Right { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        #endregion

        #region 方法

        #endregion
    }
}

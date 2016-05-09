/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志-日志类型枚举
*/

namespace Portal.Domain.Aggregates
{
    /// <summary>
    /// 日志类型：1增, 2改, 3删, 4查, 5登录, 6其它
    /// </summary>
    public enum SysLoggerType
    {
        /// <summary>
        /// 新增
        /// </summary>
        Create,

        /// <summary>
        /// 修改
        /// </summary>
        Update,

        /// <summary>
        /// 删除
        /// </summary>
        Delete,

        /// <summary>
        /// 查询
        /// </summary>
        Search,

        /// <summary>
        /// 登陆
        /// </summary>
        Login,

        /// <summary>
        /// 其他
        /// </summary>
        Other,
    }
}

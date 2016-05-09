/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志-查看权限枚举
*/

namespace Portal.Domain.Aggregates
{
    /// <summary>
    /// 查看权限: 所有人可见(包括客户), 仅内部帐号可见，仅内部管理员可见
    /// </summary>
    public enum SysLoggerRight
    {
        /// <summary>
        /// 所有人可见(包括客户)
        /// </summary>
        All,

        /// <summary>
        /// 仅内部帐号可见
        /// </summary>
        Employee,

        /// <summary>
        /// 仅内部管理员可见
        /// </summary>
        Admin,
    }
}

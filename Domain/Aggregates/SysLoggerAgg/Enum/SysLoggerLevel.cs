/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志-重要等级枚举
*/
using System;

namespace Portal.Domain.Aggregates
{
    /// <summary>
    /// 重要等级：1Debug异常, 2Info提示, 3Warning警告, 4Error错误, 5Critical严重的
    /// </summary>
    public enum SysLoggerLevel
    {
        /// <summary>
        /// 异常
        /// </summary>
        Debug,
        /// <summary>
        /// 提示
        /// </summary>
        Info,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 严重
        /// </summary>
        Critical,
    }
}

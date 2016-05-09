/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  模块层
创建日期：  2015-11-16

内容摘要：  外部开发者应用信息表信息类
*/
using System;

namespace Portal.Dto
{
    [Serializable]
    public class ClientApplicationDto : DomainDto
    {
        #region 01.属性
        /// <summary>
        /// 应用名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 应用地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 回调地址
        /// </summary>
        public string PostUrl { get; set; }
        /// <summary>
        /// 应用类型,1网站应用,2桌面应用,3移动应用，参考枚举：ClientApplicationType
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 状态，1开发中，2审核中，3审核通过，参考枚举：ClientApplicationState
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 应用简介
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 申请权限
        /// </summary>
        public string Permissions { get; set; }
        /// <summary>
        /// 已审权限
        /// </summary>
        public string PassPermissions { get; set; }
        /// <summary>
        /// 审核记录
        /// </summary>
        public string AuditRecord { get; set; }

        public string ClientId { get; set; }

        public string Secret { get; set; }
        #endregion

        #region 02.初始化
        #endregion

        #region 03.方法
        #endregion
    }

}
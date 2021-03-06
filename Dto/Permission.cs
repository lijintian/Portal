﻿namespace Portal.Dto
{
    /// <summary>
    /// 表示权限传输对象
    /// </summary>
    public class Permission : DomainDto
    {
        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 权限附属标志
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 序列编号
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否兼容老的业务系统
        /// </summary>
        public bool IsCompatible { get; set; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Desc { get; set; }
    }
}

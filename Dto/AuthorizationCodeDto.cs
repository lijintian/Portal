using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto
{
    /// <summary>
    /// 表示授权码数据传输对象
    /// </summary>
    public class AuthorizationCodeDto : AggregateDto
    {
        /// <summary>
        /// 授权码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 授码码列表
        /// </summary>
        public string[] Permssions { get; set; }

        /// <summary>
        /// 授权客户
        /// </summary>
        public string CustomerIdentity { get; set; }

        /// <summary>
        /// 开发者应用标识
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string DeveloperAppName { get; set; }

        /// <summary>
        /// 授权时间
        /// </summary>
        public DateTime AuthorizationTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        public bool IsExpired { get; set; } 

        /// <summary>
        /// 授权码是否使用
        /// </summary>
        public bool IsUsed { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime UsedTime { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}

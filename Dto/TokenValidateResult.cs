using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portal.Dto
{
    /// <summary>
    /// 表示访问Token验证结果
    /// </summary>
    public class TokenValidateResult
    {
        public TokenValidateResult() : this(false)
        {
            
        }
        public TokenValidateResult(bool isPassed)
        {
            this.CustomerNo = string.Empty;
            this.AppClientId = string.Empty;
            this.CustomerName = string.Empty;
        }

        /// <summary>
        /// 客户编码
        /// </summary>
        public string CustomerNo
        {
            get;
            set;
        }

        /// <summary>
        /// 表示客户名称
        /// </summary>
        public string CustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 应用标识Id
        /// </summary>
        public string AppClientId
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Model
{
    /// <summary>
    /// 表示引用API权限信息
    /// </summary>
    public class ReferenceApiPermssionInfo
    {
        /// <summary>
        /// 权限码
        /// </summary>
        public string Code
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否开发授权
        /// </summary>
        public bool IsOpened
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否客户授权
        /// </summary>
        public bool IsCustomerGranted
        {
            get;
            private set;
        }

        public ReferenceApiPermssionInfo(string code, bool isOpened, bool isCustomerGranted)
        {
            this.Code = code;
            this.IsOpened = isOpened;
            this.IsCustomerGranted = isCustomerGranted;
        }
    }
}

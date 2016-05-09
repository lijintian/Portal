using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Model
{
    /// <summary>
    /// 表示业务系统客户信息
    /// </summary>
    public class CustomerInfo
    {
        public static readonly CustomerInfo NullCustomer = new CustomerInfo();
        public CustomerInfo()
        {
            this.CustomerId = string.Empty;
            this.CustomerName = string.Empty;
            this.CustomerNo = string.Empty;
        }
        /// <summary>
        /// 客户Id
        /// </summary>
        public string CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 客户代码
        /// </summary>
        public string CustomerNo
        {
            get;
            set;
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName
        {
            get;
            set;
        }
    }
}

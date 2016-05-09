using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Model;

namespace Portal.Domain.Services
{
    /// <summary>
    /// 表示客户信息提供服务
    /// </summary>
    public interface ICustomerProviderService
    {
        /// <summary>
        /// 根据登录获取客户信息
        /// </summary>
        CustomerInfo GetCustomerInfoByLoginName(string loginName);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Infrastructure.Exceptions
{
    /// <summary>
    /// 表示Portal验证异常
    /// </summary>
    public class PortalValidateException : PortalException
    {
        public PortalValidateException(string errorCode, string message)
            : base(errorCode, message)
        {

        }
    }
}

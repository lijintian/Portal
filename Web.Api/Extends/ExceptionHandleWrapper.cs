using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Dto.Common;
using Portal.Infrastructure.Exceptions;

namespace Portal.Web.Api.Extends
{
    /// <summary>
    /// Api异常Wrapper处理
    /// </summary>
    static class ExceptionHandleWrapper
    {
        public static T GetResult<T>(Func<T> call) where T : ResponseBase, new()
        {
            var response = new T();
            try
            {
                response = call();
            }
            catch (PortalException ex)
            {
                response.ErrorData = new ErrorData(ex.ErrorCode, ex.Message);
            }

            return response;
        }
    }
}

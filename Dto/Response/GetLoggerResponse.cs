using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Portal.Dto.Common;

namespace Portal.Dto.Response
{
    /// <summary>
    /// 表示系统日志信息查询响应
    /// </summary>
    public class GetLoggerResponse : GetListResponse<SysLoggerDto>
    {
        #region 初始化
        public GetLoggerResponse()
        {
        }

        public GetLoggerResponse(PagerFindResponse<SysLoggerDto> data)
            : base(data)
        {
        }
        #endregion
    }
}

/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  Domain层
创建日期：  2016-03-02

内容摘要：  系统日志表查询结果类
*/
using System;
using System.Collections.Generic;
using Portal.Dto.Response;

namespace Portal.Dto
{
    [Serializable]
    public class FindSysLoggerResponse : PagerFindResponse<SysLoggerDto>
    {
        #region 初始化
        public FindSysLoggerResponse()
        { }

        public FindSysLoggerResponse(int total, IEnumerable<SysLoggerDto> data)
            : base(total, data)
        {
        }
        public FindSysLoggerResponse(GetLoggerResponse data)
            : base(data)
        {
        }
        #endregion
    }
}

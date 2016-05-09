/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制管理层
创建日期：  2016-03-02

内容摘要：  系统日志表接口服务
*/
using System;
using Portal.Dto;
using Portal.Dto.Common;
using Portal.Dto.Response;
using Portal.SDK.Common;
using Portal.SDK.Core;
using Portal.SDK.Security;
using RestSharp;

namespace Portal.SDK.ServiceClient
{
    public static class LoggerServiceClient
    {
        #region 方法
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static GetLoggerResponse Search(FindSysLoggerRequest request)
        {
            return Helpers.GetResultFromRequest<GetLoggerResponse>("api/v1/logger", request, Method.POST);
        }
        //public static FindSysLoggerResponse Search(FindSysLoggerRequest request)
        //{
        //    var result= Helpers.GetResultFromRequest<GetLoggerResponse>("api/v1/logger/create", request, Method.POST);
        //    return new FindSysLoggerResponse(result);
        //}
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="createdBy"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static ResponseBase Create(string title, string content, string createdBy)
        {
            return Create(title, content, createdBy, SysLoggerLevel.Info, SysLoggerType.Other, SysLoggerRight.All);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="createdBy"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ResponseBase Create(string title, string content, string createdBy, SysLoggerType type)
        {
            return Create(title, content, createdBy, SysLoggerLevel.Info, type, SysLoggerRight.All);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="createdBy"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="level"></param>
        /// <param name="type"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static ResponseBase Create(string title, string content, string createdBy, SysLoggerLevel level, SysLoggerType type, SysLoggerRight right)
        {
            var item = new SysLoggerDto()
            {
                ApplicationName = WebHelper.GetAppName(),
                Title = title,
                Content = content,
                Level = level,
                Type = type,
                Right = right,
                CreatedBy = createdBy
            };
            return Helpers.GetResultFromRequest<ResponseBase>("api/v1/logger/create", item, Method.POST);
        }
        #endregion
    }
}

/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制管理层
创建日期：  2016-03-02

内容摘要：  系统日志表接口管理
*/
using System.Web.Http;
using System.Web.Http.Description;
using Portal.Applications.Services;
using Portal.Dto;
using Portal.Dto.Common;
using Portal.Dto.Response;
using Portal.Web.Api.Core;

namespace Portal.Web.Api.Controllers
{
    public class LoggerController : BaseController
    {
        #region 初始化
        private ISysLoggerManagerService LoggerMangerService { get; set; }
        public LoggerController(ISysLoggerManagerService service)
        {
            this.LoggerMangerService = service;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取用户聚合信息,包括角色与权限信息
        /// </summary>
        [Route("api/v1/logger")]
        [HttpPost]
        [ResponseType(typeof(GetLoggerResponse))]
        public IHttpActionResult Get()
        {
            return ReturnAction(() =>
            {
                var result = LoggerMangerService.Search(RequestHelper.GetObject<FindSysLoggerRequest>());
                return new GetLoggerResponse(result);
            });
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        [Route("api/v1/logger/create")]
        [HttpPost]
        [ResponseType(typeof(ResponseBase))]
        public IHttpActionResult Create()
        {
            var logger = RequestHelper.GetObject<SysLoggerDto>();
            this.Request.InitInfo(logger);
            return ReturnAction(() =>
            {
                LoggerMangerService.Save(logger);
                return new ResponseBase();
            });
        }
        #endregion
    }
}

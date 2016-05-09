using System;
using System.Web.Http;


using Portal.Dto;
using Portal.Dto.Common;
using Portal.Infrastructure.Exceptions;
using Portal.Web.Api.Core;
using ServiceStack.Text;
using EasyDDD.Infrastructure.Crosscutting.Json;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;

namespace Portal.Web.Api.Controllers
{
    public class BaseController : ApiController
    {
        #region 属性
        protected IJson Json
        {
            get { return IoC.Resolve<IJson>(); }
        }
        #endregion

        #region 方法
        protected IHttpActionResult ReturnAction<T>(Func<T> call) where T : ResponseBase, new()
        {
            var response = new T();
            try
            {
                response = call();
                return Ok(response);
            }
            catch (PortalException ex)
            {
                response.ErrorData = new ErrorData(ex.ErrorCode, ex.Message);
                return this.BadRequest(response.ToJson());
            }
        }
        protected SysLoggerDto GetLogger()
        {
            return this.Request.GetLogger();
        }
        protected SysLoggerDto GetLogger(string identity)
        {
            return this.Request.GetLogger(identity);
        }
        #endregion
    }
}

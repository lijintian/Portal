using System.Web;
using Portal.Infrastructure.Exceptions;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace Portal.Web.Core.Extends
{
    /// <summary>
    /// 全局的异常处理
    /// </summary>
    public class ContentNegotiatedExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            //自定义异常
            var exception = context.Exception as PortalException;
            string ticketId = Guid.NewGuid().ToString();
            var requestInfo = RequestToString(context.Request, ticketId);
            if (exception != null)
            {
                var metadata = new ErrorData
                {
                    TicketId = ticketId,
                    Message = exception.CustomMessage,
                    ErrorCode = exception.ErrorCode,
                    DateTime = DateTime.UtcNow,
                    RequestUri = context.Request.RequestUri,
                    RequestIp = this.GetClientIp(context.Request)
                };

                //Task.Run(() => Log<ContentNegotiatedExceptionHandler>.LogWarning(requestInfo, exception));
                var response = context.Request.CreateResponse(HttpStatusCode.BadRequest, metadata);
                context.Result = new ResponseMessageResult(response);
            }
            //序列化异常
            else if (context.Exception is System.Runtime.Serialization.SerializationException)
            {
                var metadata = new ErrorData
                {
                    TicketId = ticketId,
                    Message = string.Format("序列化发生异常，可能提交的数据格式有误：{0}", context.Exception.Message),
                    DateTime = DateTime.UtcNow,
                    RequestUri = context.Request.RequestUri,
                    RequestIp = this.GetClientIp(context.Request)
                };

                var ex = context.Exception as System.Runtime.Serialization.SerializationException;
                //Task.Run(() => Log<ContentNegotiatedExceptionHandler>.LogError(requestInfo, ex));

                var response = context.Request.CreateResponse(HttpStatusCode.BadRequest, metadata);
                context.Result = new ResponseMessageResult(response);
            }
            else //未处理异常
            {
                var metadata = new ErrorData
                {
                    TicketId = ticketId,
                    Message = string.Format("抱歉！发现系统异常，请您用此TicketId:{0}联系系统管理员，我们将以最快的速度为您解决此问题，谢谢！", ticketId),
                    DateTime = DateTime.UtcNow,
                    RequestUri = context.Request.RequestUri,
                    RequestIp = this.GetClientIp(context.Request)
                };

                var ex = context.Exception;
                //Task.Run(() => Log<ContentNegotiatedExceptionHandler>.LogError(requestInfo, ex));

                var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, metadata);
                context.Result = new ResponseMessageResult(response);
            }
        }

        private static string RequestToString(HttpRequestMessage request, string ticketId = "")
        {
            var message = new StringBuilder();

            if (!string.IsNullOrEmpty(ticketId))
            {
                message.AppendFormat(" TicketId: {0} ", ticketId);
            }

            if (request.Method != null)
            {
                message.AppendFormat(" Method: {0} ", request.Method);
            }

            if (request.RequestUri != null)
            {
                message.AppendFormat(" RequestUri:{0} ", request.RequestUri);
            }


            return message.ToString();
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

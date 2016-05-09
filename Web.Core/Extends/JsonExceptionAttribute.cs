using System;
using System.Web;
using System.Web.Mvc;
using Portal.Web.Core.Model;

namespace Portal.Web.Core.Extends
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class JsonExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                if (filterContext.Exception is HttpRequestValidationException)
                {
                    SetResult(filterContext, "请您输入合法字符串！");
                }
                else
                {
                    //返回异常JSON
                    SetResult(filterContext, filterContext.Exception.Message);
                }
            }
        }

        /// <summary>
        /// 设置返回值
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="message"></param>
        private void SetResult(ExceptionContext filterContext, string message)
        {
            filterContext.Result = new JsonResult
            {
                Data = new ReturnModel<int>(0, message, false)
            };
            filterContext.ExceptionHandled = true;
        }
    }
}

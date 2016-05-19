/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  控制层
创建日期：  2015-07-29
作者：      吴勺良
内容摘要：  底层控制类
*/
using System.Text;
using System.Web.Mvc;
using Portal.Client.Core;
using Portal.Dto;
using Portal.SDK.Security;
using Portal.Web.Core;
using Portal.Web.Core.Model;

namespace Portal.Client.Controllers
{
    public class BaseController : Controller
    {
        #region 01.当前用户信息
        /// <summary>
        /// 当前登录用户信息(保存Session)
        /// </summary>
        public PortalPrincipal CurrentUser
        {
            get
            {
                //return new CK1Principal(new UserPackageInfo("tss", "tss", "token", UserType.ExternalApi, null, null));
                return (PortalPrincipal)this.ControllerContext.HttpContext.User;
            }
        }
        #endregion

        #region 02.公用方法
        /// <summary>
        /// 返回html或json
        /// </summary>
        /// <param name="str"></param>
        /// <param name="isJson"></param>
        /// <returns></returns>
        protected ActionResult ReturnContent(string str, bool isJson = false)
        {
            return isJson ? Content(str, "application/json", Encoding.UTF8) : Content(str, "text/plain");
        }

        /// <summary>
        /// 返回JSON格式的字符串
        /// </summary>
        /// <returns></returns>
        public string ReturnJson()
        {
            return new ReturnModel<int>("保存成功！", true).ToJson();
        }

        /// <summary>
        /// 返回JSON格式的字符串
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public string ReturnJson(string errorMessage, bool status = false)
        {
            return new ReturnModel<int>(errorMessage, status).ToJson();
        }

        /// <summary>
        /// 返回页面或用户控件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="isPostBack">是否非首次加载</param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult ReturnView(object source, bool isPostBack, string viewName)
        {
            if (!isPostBack)
            {
                return View(source);
            }
            return PartialView(viewName, source);
        }

        /// <summary>
        /// 返回页面
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parameter"></param>
        /// <param name="viewName"></param>
        /// <param name="jsName"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <returns></returns>
        public ActionResult ReturnView<T, TParameter>(PagerFindResponse<T> list, TParameter parameter, string viewName, string jsName)
            where T : new()
            where TParameter : PagerFindRequest
        {
            return ReturnView(GetPageList(list, parameter, jsName), parameter.IsPostBack, viewName);
        }

        /// <summary>
        /// 获取分页结果
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parameter"></param>
        /// <param name="jsName"></param>
        /// <returns></returns>
        public PageListSource<T> GetPageList<T>(PagerFindResponse<T> list, PagerFindRequest parameter, string jsName) where T : new()
        {
            return PageingCreator.GetList(list, parameter, PageUtility.PageInfo, jsName);
        }
        #endregion
    }
}

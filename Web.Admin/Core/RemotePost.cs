using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Portal.Web.Admin.Core
{
    public class RemotePost
    {
        private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();
        public string Url = "";
        public string Method = "post";
        public string FormName = "form1";

        /// <summary>
        /// 添加需要提交的名和值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }

        /// <summary>
        /// 以输出Html方式POST
        /// </summary>
        public void Post()
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write("<html><head>");
            System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
            try
            {
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");
                System.Web.HttpContext.Current.Response.End();
            }
            catch (Exception ee)
            {

            }
        }

        public void Redirect()
        {
            string url = Url;
            Uri uri = new Uri(Url);
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                url += i == 0 && string.IsNullOrEmpty(uri.Query) ? "?" : "&";
                url += string.Format("{0}={1}", Inputs.Keys[i], HttpUtility.UrlEncode(Inputs[Inputs.Keys[i]], System.Text.Encoding.UTF8));
            }
            System.Web.HttpContext.Current.Response.Redirect(url, true);
        }
    }
}

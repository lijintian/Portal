/*
 EasyDDD
系统名称：  用户中心管理平台
模块名称：  控制层
创建日期：  2015-07-29

内容摘要：  分页HTML
*/


using System;
using System.Text;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public static class PagingUtility
    {
        #region 01.获取翻页代码
        /// <summary>
        /// 获取翻页代码
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="pageInfo"></param>
        /// <param name="jsname"></param>
        /// <returns></returns>
        public static string Paging(BaseParameter parameter, CreatePagingInfo pageInfo, string jsname)
        {
            pageInfo.InitInfo(jsname);
            return pageInfo.IsClient ? ClientPaging(parameter, pageInfo) : Paging(parameter, pageInfo);
        }
        /// <summary>
        /// 获取翻页代码
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        private static string Paging(BaseParameter parameter, CreatePagingInfo pageInfo)
        {
            const string emptyFormat = "<a href=\"javascript:void(0)\">{1}</a>";
            var sbstart = new StringBuilder { Capacity = 200 };
            sbstart.AppendFormat("<div class=\"col-xs-4\"><div class=\"dataTables_info\" id=\"table_report_info\">共{0}条信息</div></div>", parameter.TotalRows);
            sbstart.Append("<div class=\"col-xs-8\"><div class=\"dataTables_paginate paging_simple_numbers\"><ul class='pagination'>");
            var sbresult = new StringBuilder();
            var ps = ScrollRangeCalc(parameter, pageInfo.ShowPageCount);
            bool isDisabled = parameter.PageIndex <= 1;
            bool isDisabled2 = parameter.PageIndex >= parameter.TotalPages;
            sbstart.AppendFormat("<li {0}>{1}</li>", isDisabled ? "class=\"disabled\"" : "", string.Format(isDisabled ? emptyFormat : pageInfo.HrefFormat, 1, "首页"));
            sbstart.AppendFormat("<li class=\"paginate_button previous {0}\">{1}</li>", isDisabled ? "disabled" : "", string.Format(isDisabled ? emptyFormat : pageInfo.HrefFormat, parameter.PageIndex - 1, "<i class=\"ace-icon fa fa-angle-double-left\"></i>"));
            sbstart.Append("{0}");
            for (int i = ps[0]; i <= ps[1]; i++)
            {
                isDisabled = parameter.PageIndex == i;
                sbstart.AppendFormat("<li {0}>{1}</li>", isDisabled ? "class=\"active\"" : "", string.Format(isDisabled ? emptyFormat : pageInfo.HrefFormat, i, i));
            }
            if (pageInfo.ShowStart)
            {
                sbresult.AppendFormat(sbstart.ToString(), ps[0] > 1 ? string.Format("<li>" + pageInfo.HrefFormat + "</li>", 1, "...") : "");
            }
            else
            {
                sbresult.AppendFormat(sbstart.ToString(), "");
            }
            if (pageInfo.ShowEnd && ps[1] < parameter.TotalPages)
            {
                sbresult.AppendFormat("<li>" + pageInfo.HrefFormat + "</li>", parameter.TotalPages, "...");
            }

            sbresult.AppendFormat("<li class=\"paginate_button next {0}\">{1}</li>", isDisabled2 ? "disabled" : "", string.Format(isDisabled2 ? emptyFormat : pageInfo.HrefFormat, parameter.PageIndex + 1, "<i class=\"ace-icon fa fa-angle-double-right\"></i>"));
            sbstart.AppendFormat("<li {0}>{1}</li>", isDisabled2 ? "class=\"disabled\"" : "", string.Format(isDisabled2 ? emptyFormat : pageInfo.HrefFormat, parameter.TotalPages, "尾页"));
            sbresult.AppendFormat("</ul></div></div>");
            if (pageInfo.ShowSetPageSize)
            {
                GetSetPageSizeHtml(parameter, pageInfo.OnChangeName, sbresult);
            }
            return sbresult.ToString();
        }

        /// <summary>
        /// 获取翻页代码
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        private static string ClientPaging(BaseParameter parameter, CreatePagingInfo pageInfo)
        {
            const string emptyFormat = "<a href=\"javascript:void(0)\">{1}</a>";
            var sbstart = new StringBuilder { Capacity = 200 };
            sbstart.AppendFormat("<div class=\"span3\"><div class=\"pagination\" style=\"line-height:30px; vertical-align:middle;\">共{0}条信息</div></div>", parameter.TotalRows);
            sbstart.Append("<div class=\"span9\"><div class=\"pagination\"><ul>");
            var sbresult = new StringBuilder();
            var ps = ScrollRangeCalc(parameter, pageInfo.ShowPageCount);
            bool isDisabled = parameter.PageIndex <= 1;
            bool isDisabled2 = parameter.PageIndex >= parameter.TotalPages;
            sbstart.AppendFormat("<li {0}>{1}</li>", isDisabled ? "class=\"disabled\"" : "", string.Format(isDisabled ? emptyFormat : pageInfo.HrefFormat, 1, "首页"));
            sbstart.AppendFormat("<li class=\"prev {0}\">{1}</li>", isDisabled ? "disabled" : "", string.Format(isDisabled ? emptyFormat : pageInfo.HrefFormat, parameter.PageIndex - 1, "&laquo;"));
            sbstart.Append("{0}");
            for (int i = ps[0]; i <= ps[1]; i++)
            {
                isDisabled = parameter.PageIndex == i;
                sbstart.AppendFormat("<li {0}>{1}</li>", isDisabled ? "class=\"active\"" : "", string.Format(isDisabled ? emptyFormat : pageInfo.HrefFormat, i, i));
            }
            if (pageInfo.ShowStart)
            {
                sbresult.AppendFormat(sbstart.ToString(), ps[0] > 1 ? string.Format("<li>" + pageInfo.HrefFormat + "</li>", 1, "...") : "");
            }
            else
            {
                sbresult.AppendFormat(sbstart.ToString(), "");
            }
            if (pageInfo.ShowEnd && ps[1] < parameter.TotalPages)
            {
                sbresult.AppendFormat("<li>" + pageInfo.HrefFormat + "</li>", parameter.TotalPages, "...");
            }

            sbresult.AppendFormat("<li class=\"next {0}\">{1}</li>", isDisabled2 ? "disabled" : "", string.Format(isDisabled2 ? emptyFormat : pageInfo.HrefFormat, parameter.PageIndex + 1, "&raquo;"));
            sbstart.AppendFormat("<li {0}>{1}</li>", isDisabled2 ? "class=\"disabled\"" : "", string.Format(isDisabled2 ? emptyFormat : pageInfo.HrefFormat, parameter.TotalPages, "尾页"));
            sbresult.AppendFormat("</ul></div></div>");
            if (pageInfo.ShowSetPageSize)
            {
                GetSetPageSizeHtml(parameter, pageInfo.OnChangeName, sbresult);
            }
            return sbresult.ToString();
        }
        #endregion

        #region 02.计算滚动翻页区
        /// <summary>
        /// 计算滚动翻页区
        /// </summary>
        /// <returns></returns>
        private static int[] ScrollRangeCalc(BaseParameter parameter, int count)
        {
            var startPage = Math.Max(parameter.PageIndex - (count / 2), 1);
            var endPage = Math.Min(parameter.TotalPages, startPage + count - 1);
            return new[] { startPage, endPage };
        }
        #endregion

        #region 03.带页码设置的翻页代码
        private static void GetSetPageSizeHtml(BaseParameter parameter, string jsname, StringBuilder sbresult)
        {
            if (string.IsNullOrEmpty(jsname))
            {
                jsname = "SetPageSize";
            }
            sbresult.AppendFormat(@"&nbsp;<select id='selPageSize' onchange='javascript:{0}()'  style='margin-bottom:-3px; _margin-top:6px;'>", jsname);
            sbresult.Append(GetOptionHtml(12, parameter.PageSize));
            sbresult.Append(GetOptionHtml(20, parameter.PageSize));
            sbresult.Append(GetOptionHtml(30, parameter.PageSize));
            sbresult.Append(GetOptionHtml(40, parameter.PageSize));
            sbresult.Append(GetOptionHtml(80, parameter.PageSize));
            sbresult.Append("</select>");
        }
        /// <summary>
        /// 获取下拉选项的值
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="currentPageSize"></param>
        /// <returns></returns>
        private static string GetOptionHtml(int pagesize, int currentPageSize)
        {
            return string.Format("<option {1} value='{0}'>{0}条/页</option>", pagesize, pagesize == currentPageSize ? "selected=\"selected\"" : "");
        }
        #endregion
    }
}

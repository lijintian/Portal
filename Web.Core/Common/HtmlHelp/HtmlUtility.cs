using System.Collections.Generic;
using System.Text;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public static class HtmlUtility
    {
        #region 01.将需要显示的字符串转换成 HTML 格式的
        /// <summary>
        /// 将需要显示的字符串转换成 HTML 格式的
        /// 例如：保存前台HTML标签时，需要对HTML标签进行转换再提交
        /// </summary>
        /// <param name="sour">被转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ToHtml(this string sour)
        {
            if (string.IsNullOrEmpty(sour)) return "";

            // 以下逐一转换
            sour = sour.Replace("&", "&amp;"); // & 符號,最先转换
            sour = sour.Replace(" ", "&nbsp;");
            sour = sour.Replace("%", "&#37;");
            sour = sour.Replace("<", "&lt;");
            sour = sour.Replace(">", "&gt;");
            sour = sour.Replace("\n", "\n<br/>");
            sour = sour.Replace("\"", "&quot;");
            sour = sour.Replace("'", "&#39;");
            sour = sour.Replace("+", "&#43;");
            return sour;
        }
        /// <summary>
        /// 符号转换，特殊符号都在前面加上斜杠(\)
        /// </summary>
        /// <param name="sour">被转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ChangeMarks(this string sour)
        {
            if (string.IsNullOrEmpty(sour)) return "";

            // 斜杠(\)取代成(\\)
            sour = sour.Replace(@"\", @"\\");
            // 各符號，都在前面加上斜杠(\)
            sour = sour.Replace("\"", "\\\""); // 双引号变成 \"
            sour = sour.Replace("'", @"\'");
            sour = sour.Replace("/", @"\/");
            sour = sour.Replace("<", @"\<");
            sour = sour.Replace(">", @"\>");
            sour = sour.Replace(":", @"\:");
            sour = sour.Replace("#", @"\#");
            sour = sour.Replace("%", @"\%");
            sour = sour.Replace("&", @"\&");
            return sour;
        }
        #endregion

        #region 02.拼接下拉框，单选框，复选框数据源
        /// <summary>
        /// 拼接下拉框，单选框，复选框数据源
        /// </summary>
        /// <param name="list">各种菜单的实体类列表</param>
        /// <param name="foramtstr">0表示值，1表示是否选中，2表示名称，3表示描述</param>
        /// <param name="type">类型</param>
        /// <param name="initValue">添加项的值</param>
        /// <param name="initText">添加项的文本</param>
        /// <param name="selectedValue">被选中的选项值</param>
        /// <param name="hasDescription">是否添加描述</param>
        /// <returns>实体类列表的option标签字符串</returns>
        public static string OptionList(List<EnumModel> list, string foramtstr, string type, string initValue, string initText, string selectedValue = null, bool hasDescription = false)
        {
            var menu = new StringBuilder();
            // 默认选项
            if (!string.IsNullOrEmpty(initValue) || !string.IsNullOrEmpty(initText))
                menu.AppendFormat(foramtstr, initValue, initText, null, null, null);
            if (LengthUtility.IsNullOrEmpty(list)) return menu.ToString();
            bool isMultiple = type.Contains("Checkbox") || type.Contains("MoreHref");//是否是多选
            List<string> selList = isMultiple ? ArrayUtility.ToList(selectedValue) : null;
            string selectStr = type.Contains("Checkbox") || type.Contains("Radio") ? "checked=\"checked\"" : (type.Contains("Href") ? "class=\"active\"" : "selected");
            // 拼接成 option 字符串
            int groupIndex = 0;
            foreach (var item in list)
            {
                string value = item.Value.ChangeMarks();
                string selected = (isMultiple ? !selList.IsNullOrEmpty() && selList.Contains(value) : value.Equals(selectedValue)) ? selectStr : ""; // 是否选中
                if (item.IsGroup)
                {
                    if (groupIndex > 0)
                    {
                        menu.Append("</optgroup>");
                    }
                    menu.AppendFormat("<optgroup label='{0}'>", item.Text.ToHtml());
                    groupIndex++;
                }
                else
                {
                    menu.AppendFormat(foramtstr, value, selected, item.Text.ToHtml(), !hasDescription ? "" : string.Format("Other='{0}'", item.Description.ToHtml()));
                }
            }
            if (groupIndex > 0)
            {
                menu.Append("</optgroup>");
            }
            return menu.ToString();
        }
        #endregion
    }
}

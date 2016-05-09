/*
系统名称：  通用类
模块名称：  根据枚举型获取下拉框的数据源
创建日期：  2015-07-30

内容摘要：  
*/

using System.Collections.Generic;
using System.Web.Mvc;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class EnumSource<T>
    {
        #region 方法
        /// <summary>
        /// 获取DropDownList的数据源
        /// </summary>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static SelectList Options(bool hasDefalut, string defaultvalue = null)
        {
            EnumModel model = hasDefalut ? ListUtility.GetDefaultSelect : null;
            return EnumListUtility<T>.Options(model, defaultvalue);
        }
        /// <summary>
        /// 获取DropDownList的数据源
        /// </summary>
        /// <param name="defaultvalue"></param>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static SelectList Options2(string value = null, string text = null, string defaultvalue = null)
        {
            var model = !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(text) ? new EnumModel(value, text) : null;
            return EnumListUtility<T>.Options(model, defaultvalue);
        }
        /// <summary>
        /// 有默认值无添加项
        /// </summary>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static SelectList Options(string defaultvalue)
        {
            return Options2(null, null, defaultvalue);
        }
        #endregion
    }
}

/*
 EasyDDD
系统名称：  通用类
模块名称：  根据List型获取下拉框的数据源
创建日期：  2015-07-30

内容摘要：  
*/
using System.Collections.Generic;
using System.Web.Mvc;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class ListSource
    {
        #region 方法
        /// <summary>
        /// 有默认值无添加项
        /// </summary>
        /// <param name="list"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static SelectList Options(List<EnumModel> list, string defaultvalue = null)
        {
            return ListUtility.Options(list, null, defaultvalue);
        }

        /// <summary>
        /// 有默认值无添加项
        /// </summary>
        /// <param name="list"></param>
        /// <param name="model"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static SelectList Options(List<EnumModel> list, EnumModel model, string defaultvalue = null)
        {
            return ListUtility.Options(list, model, defaultvalue);
        }

        /// <summary>
        /// 获取DropDownList的数据源
        /// </summary>
        /// <param name="list"></param>
        /// <param name="hasDefalut">是否有默认项</param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static SelectList Options(List<EnumModel> list, bool hasDefalut, string defaultvalue = null)
        {
            EnumModel model = hasDefalut ? ListUtility.GetDefaultSelect : null;
            return ListUtility.Options(list, model, defaultvalue);
        }

        /// <summary>
        /// 获取CheckBox的数据源
        /// </summary>
        /// <returns></returns>
        public static string GetSource(List<EnumModel> list, EnumModel model, string defaultvalue = null)
        {
            return ListUtility.GetSource(list, model, defaultvalue);
        }

        /// <summary>
        /// 获取CheckBox的数据源
        /// </summary>
        /// <returns></returns>
        public static string GetSource(List<EnumModel> list, string defaultvalue = null)
        {
            return ListUtility.GetSource(list, null, defaultvalue);
        }

        /// <summary>
        /// 获取DropDownList的数据源
        /// </summary>
        /// <param name="list"></param>
        /// <param name="hasDefalut">是否有默认项</param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static string GetSource(List<EnumModel> list, bool hasDefalut, string defaultvalue = null)
        {
            EnumModel model = hasDefalut ? ListUtility.GetDefaultSelect : null;
            return ListUtility.GetSource(list, model, defaultvalue);
        }
        #endregion
    }
}

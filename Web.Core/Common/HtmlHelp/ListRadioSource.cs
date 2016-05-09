/*
系统名称：  通用类
模块名称：  根据List获取单选框的数据源
创建日期：  2015-07-30

内容摘要：  
*/
using System.Collections.Generic;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class ListRadioSource
    {
        #region 方法
        /// <summary>
        /// 获取RadioButtonList的数据源
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <param name="info"></param>
        /// <param name="defaultvalue"></param>
        /// <param name="foramtstr">格式化字符串</param>
        /// <returns></returns>
        public static string GetSource(List<EnumModel> list, string name, EnumModel info = null, string defaultvalue = null, string foramtstr = null)
        {
            //var model = !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(text) ? new EnumModel(value, text) : null;
            return ListUtility.GetSource(list, SelectListType.Radio, name, info, defaultvalue,foramtstr);
        }

        /// <summary>
        /// 有默认值无添加项
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static string GetCurrentSource(List<EnumModel> list, string name, string defaultvalue, string foramtstr = null)
        {
            return GetSource(list, name, null, defaultvalue,foramtstr);
        }
        #endregion
    }
}

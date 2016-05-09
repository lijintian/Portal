/*
系统名称：  通用类
模块名称：  根据枚举型获取复选框的数据源
创建日期：  2015-07-30

内容摘要：  
*/

using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class EnumCbxSource<T>
    {
        #region 方法
        /// <summary>
        /// 获取CheckBox的数据源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="text"></param>
        /// <param name="defaultvalue"></param>
        /// <param name="foramtstr"></param>
        /// <returns></returns>
        public static string GetSource(string name, string value = null, string text = null, string defaultvalue = null, string foramtstr = null)
        {
            var model = !string.IsNullOrEmpty(value) || !string.IsNullOrEmpty(text) ? new EnumModel(value, text) : null;
            return EnumListUtility<T>.GetSource(SelectListType.Checkbox, name, model, defaultvalue, foramtstr);
        }

        /// <summary>
        /// 有默认值无添加项
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static string GetCurrentSource(string name, string defaultvalue)
        {
            return GetSource(name, null, null, defaultvalue);
        }
        #endregion
    }
}

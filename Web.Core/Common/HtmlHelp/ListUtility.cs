using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class ListUtility
    {
        #region 属性
        public static readonly EnumModel GetDefaultSelect = new EnumModel("", "==请选择==");
        #endregion

        #region 01.将枚举转换为List数据源
        /// <summary>
        /// 将枚举转换为List数据源
        /// </summary>
        /// <returns></returns>
        public static List<EnumModel> GetList<T>()
        {
            return (from T items in Enum.GetValues(typeof(T)) select new EnumModel(System.Convert.ToInt32(items), items.GetDescription()) { TextEg = items.ToString(), Description = items.ToString() }).ToList();
        }
        #endregion

        #region 02.获取下拉框数据源
        /// <summary>
        /// 获取下拉框数据源
        /// </summary>
        /// <param name="list"></param>
        /// <param name="model"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static SelectList Options(List<EnumModel> list, EnumModel model = null, string defaultvalue = null)
        {
            if (list == null)
            {
                list = new List<EnumModel>();
            }
            Add(list, model);
            defaultvalue = string.IsNullOrEmpty(defaultvalue) ? "" : defaultvalue;
            var slist = new SelectList(list, "Value", "Text", defaultvalue);
            return slist;
        }
        #endregion

        #region 03.获取下拉框数据源HTML
        /// <summary>
        /// 获取下拉框数据源HTML
        /// </summary>
        /// <param name="list"></param>
        /// <param name="model"></param>
        /// <param name="defaultvalue"></param>
        /// <param name="hasDescription"></param>
        /// <returns></returns>
        public static string GetSource(List<EnumModel> list, EnumModel model = null, string defaultvalue = null, bool hasDescription = false)
        {
            if (list == null)
            {
                list = new List<EnumModel>();
            }
            Add(list, model);
            return HtmlUtility.OptionList(list, SelectListType.Select.GetDescription(), "Select", null, null, defaultvalue, hasDescription);
        }
        #endregion

        #region 04.获取单选、复选框数据源HTML
        /// <summary>
        /// 获取单选、复选框数据源HTML
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <param name="defaultvalue"></param>
        /// <param name="foramtstr">格式化字符串</param>
        /// <param name="hasDescription"></param>
        /// <returns></returns>
        public static string GetSource(List<EnumModel> list, SelectListType type, string name, EnumModel model = null, string defaultvalue = null, string foramtstr = null, bool hasDescription = false)
        {
            if (list == null)
            {
                list = new List<EnumModel>();
            }
            Add(list, model);
            foramtstr = string.IsNullOrEmpty(foramtstr) ? type.GetDescription() : foramtstr;
            if (type.ToString().Contains("Radio") || type.ToString().Contains("Checkbox"))
            {
                string typeName = type.ToString().Contains("Radio") ? "radio" : "checkbox";
                foramtstr = string.Format(foramtstr, typeName, name);
            }
            return HtmlUtility.OptionList(list, foramtstr, type.ToString(), null, null, defaultvalue, hasDescription);
        }
        #endregion

        #region 05.将List1转换为List2
        /// <summary>
        /// 将List1(id,text,orderid)转换为List2(orderid,text)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<EnumModel> Convert(List<EnumModel> list)
        {
            List<EnumModel> result = new List<EnumModel>();
            if (!LengthUtility.IsNullOrEmpty(list))
            {
                result.AddRange(list.Select(item => new EnumModel(item.OrderId, item.Text)));
            }
            return result;
        }
        #endregion

        #region 06.添加一行
        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="list"></param>
        /// <param name="model"></param>
        protected static void Add(List<EnumModel> list, EnumModel model)
        {
            if (model != null)
            {
                list.Insert(0, model);
            }
        }
        #endregion
    }
}

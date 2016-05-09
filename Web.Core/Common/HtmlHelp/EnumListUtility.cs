using System.Web.Mvc;
using System.Collections.Generic;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public static class EnumListUtility<T>
    {
        #region 01.字段
        private static readonly List<EnumModel> _list = ListUtility.GetList<T>();
        #endregion

        #region 02.属性
        public static List<EnumModel> GetList
        {
            get { return _list; }
        }
        public static List<EnumModel> GetList2
        {
            get { return ArrayUtility.Clone(_list); }
        }
        #endregion

        #region 03.获取下拉框数据源
        /// <summary>
        /// 获取下拉框数据源
        /// </summary>
        /// <param name="model"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static SelectList Options(EnumModel model = null, string defaultvalue = null)
        {
            return ListUtility.Options(GetList2, model, defaultvalue);
        }
        #endregion

        #region 04.获取下拉框数据源HTML
        /// <summary>
        /// 获取下拉框数据源HTML
        /// </summary>
        /// <param name="model"></param>
        /// <param name="defaultvalue"></param>
        /// <param name="hasDescription"></param>
        /// <returns></returns>
        public static string GetSource(EnumModel model = null, string defaultvalue = null, bool hasDescription = false)
        {
            return ListUtility.GetSource(GetList2, model, defaultvalue, hasDescription);
        }
        #endregion

        #region 05.获取单选、复选框数据源HTML
        /// <summary>
        /// 获取单选、复选框数据源HTML
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="model"></param>
        /// <param name="defaultvalue"></param>
        /// <param name="foramtstr">格式化字符串</param>
        /// <param name="hasDescription"></param>
        /// <returns></returns>
        public static string GetSource(SelectListType type, string name, EnumModel model = null, string defaultvalue = null, string foramtstr = null, bool hasDescription = false)
        {
            return ListUtility.GetSource(GetList2, type, name, model, defaultvalue, foramtstr, hasDescription);
        }
        #endregion
    }
}

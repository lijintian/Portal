/*
 EasyDDD
系统名称：  通用类
模块名称：  根据List转换为树的数据源
创建日期：  2013-5-9

内容摘要：  
*/
using System.Collections.Generic;
using System.Text;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public class TreeSource
    {
        /// <summary>
        /// 获取树的数据源
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static ZTreeInfo GetSouce(List<TreeModel> list, TreeParameter parameter)
        {
            var model = new ZTreeInfo(parameter.DataType, parameter.Type, parameter.JsName)
            {
                TreeName = parameter.TreeName
            };
            switch (parameter.Type)
            {
                case (int)TreeType.RadioButton:
                case (int)TreeType.CheckBox:
                    {
                        model.Source = Convert(list);
                        break;
                    }
                default:
                    {
                        model.Source = ConvertHasClick(list, parameter.JsName);
                        break;
                    }
            }
            return model;
        }

        #region 01.数据集转换为字符串结果
        /// <summary>
        /// 多选树数据源转换为STRING
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string Convert(List<TreeModel> result)
        {
            var sb = new StringBuilder();
            foreach (TreeModel item in result)
            {
                GetHtml(item, sb);
            }
            return sb.ToString().TrimEnd(',');
        }
        #endregion

        #region 02.有单击事件的结果字符串
        /// <summary>
        /// 有单击事件的结果字符串
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string ConvertHasClick(List<TreeModel> result)
        {
            return ConvertHasClick(result, "ShowMsg");
        }

        /// <summary>
        /// 有单击事件的结果字符串
        /// </summary>
        /// <param name="result"></param>
        /// <param name="jsName"></param>
        /// <returns></returns>
        public static string ConvertHasClick(List<TreeModel> result, string jsName)
        {
            var sb = new StringBuilder();
            foreach (TreeModel item in result)
            {
                GetHtml(item, sb, jsName);
            }
            return sb.ToString().TrimEnd(',');
        }
        #endregion

        #region 03.私有方法
        /// <summary>
        /// 获取一行的HTML
        /// </summary>
        /// <param name="item"></param>
        /// <param name="sb"></param>
        /// <param name="jsname"></param>
        private static void GetHtml(TreeModel item, StringBuilder sb, string jsname = null)
        {
            sb.AppendFormat("{{id: '{0}', pId:'{1}', name: '{2}',checked:{3},isParent: {4},IsGroup:{5}", item.Id, item.ParentId,
                            Remove(item.Name), (item.Checked ? "true" : "false"),
                            item.IsParent.ToString().ToLower(), (item.IsGroup ? "true" : "false"));
            if (!string.IsNullOrEmpty(item.Url))
            {
                sb.AppendFormat(",url:'{0}'", item.Url);
            }
            if (!string.IsNullOrEmpty(item.Rel))
            {
                sb.AppendFormat(",rel:'{0}'", item.Rel);
            }
            if (!string.IsNullOrEmpty(item.Icon))
            {
                sb.AppendFormat(",icon:'{0}'", item.Icon);
            }
            if (item.Open)
            {
                sb.AppendFormat(",open:true");
            }
            if (!string.IsNullOrEmpty(jsname))
            {
                sb.AppendFormat(",click:\"{1}('{0}');\"", item.Id, jsname);
            }
            sb.Append("},");
        }
        public static string Remove(string value)
        {
            return value.Replace("'", "");
        }
        #endregion

    }
}

/*
 EasyDDD
系统名称：  Model
模块名称：  树的具体数据类
创建日期：  2015-07-29

内容摘要：  树参数信息
*/
namespace Portal.Web.Core.Model
{
    public class TreeParameter
    {
        #region 字段
        /// <summary>
        /// 主键（用于异步加载用户组）
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 树的名称
        /// </summary>
        public string TreeName { get; set; }

        /// <summary>
        /// 树的类型，1表示一般树，2表示Checkbox的树，3表示RadioButton的树，参考枚举：TreeType
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 默认选中项（多个逗号分隔）
        /// </summary>
        public string Checkeds { get; set; }

        /// <summary>
        /// Js方法名称，默认是ShowMsg
        /// </summary>
        public string JsName { get; set; }
        #endregion

        #region 初始化

        public TreeParameter()
        {
            JsName = "ShowMsg";
        }
        public TreeParameter(int datatype, int type, string jsname)
        {
            DataType = datatype;
            Type = type;
            JsName = jsname;
        }
        #endregion
    }
}

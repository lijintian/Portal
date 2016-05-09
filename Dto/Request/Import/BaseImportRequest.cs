using System.Data;

namespace Portal.Dto.Request
{
    public class BaseImportRequest
    {
        #region 属性
        /// <summary>
        /// 原数据
        /// </summary>
        public DataRow RowItem { get; set; }
        #endregion

        #region 方法
        public static string GetItemName()
        {
            return "RowItem";
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.Data;

namespace Portal.Web.Core.Model.File
{
    public class ImportFileResult<T> : ReturnModel<string>
    {
        #region 属性
        public List<T> List;

        public DataTable ErrorTable;
        #endregion

        #region 初始化
        public ImportFileResult()
        { }
        public ImportFileResult(string errorMessage, bool status)
        {
            ErrorMessage = errorMessage;
            Status = status;
        }
        #endregion

        #region 方法

        public void InitInfo(List<T> list, DataTable errorTable)
        {
            this.List=new List<T>();
            this.ErrorTable=new DataTable();
            this.List = list;
            this.ErrorTable = errorTable;
            this.Status = true;
        }
        #endregion
    }
}

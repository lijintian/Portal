using System;
using System.Data;

namespace Portal.Applications.Events.Callbacks
{
    [Serializable]
    public class ImportCheckEventResult : CommonCheckEventResult<DataTable>
    {
        #region 初始化
        public ImportCheckEventResult()
        { }
        public ImportCheckEventResult(bool status)
            : base(status)
        {
        }
        public ImportCheckEventResult(string errorMessage, bool status = false)
            : base(errorMessage, status)
        {
        }
        public ImportCheckEventResult(DataTable data, string errorMessage, bool status = false)
            : base(data, errorMessage, status)
        {
        }
        #endregion
    }
}

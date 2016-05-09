using System.Collections.Generic;
using System.Data;

using Portal.Dto;

using EasyDDD.Core.Event;

namespace Portal.Applications.Events
{
    public class BaseImportCheckEvent<T> : DomainEvent
    {
        #region 属性
        public List<T> List { get; private set; }
        public DataTable ErrorTable { get; private set; }
        public SysLoggerDto Logger { get; private set; }
        #endregion

        #region 初始化
        public BaseImportCheckEvent(List<T> list, DataTable errorTable, SysLoggerDto logger)
        {
            this.List = list;
            this.ErrorTable = errorTable;
            this.Logger = logger;
        }
        #endregion
    }
}

using EasyDDD.Core.Aggregate;
using System;


namespace Portal.Domain.Aggregates
{
    public class DomainRoot : AggregateRoot
    {
        #region 属性
        public DateTime CreatedOn { get; private set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; private set; }
        public string UpdatedBy { get; set; }
        #endregion

        #region 初始化
        public DomainRoot()
        {

        }
        public DomainRoot(string updateBy)
        {
            this.CreatedOn = DateTime.Now;
            this.CreatedBy = updateBy;
            this.UpdatedOn = this.CreatedOn;
            this.UpdatedBy = updateBy;
        }
        #endregion

        #region 方法

        public void InitUpdateInfo(string updateBy)
        {
            this.UpdatedOn = DateTime.Now;
            this.UpdatedBy = updateBy;
        }
        #endregion
    }
}

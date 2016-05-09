namespace Portal.Web.Core.Model
{
    /// <summary>
    /// 附件删除类型
    /// </summary>
    public enum DeleteType
    {
        /// <summary>
        /// 不显示删除
        /// </summary>
        UnDelete = 1,
        /// <summary>
        /// 所有人都显示删除
        /// </summary>
        AllDelete = 2,
        /// <summary>
        /// 当前人才显示删除
        /// </summary>
        MySelf = 3,
        /// <summary>
        /// 当前部门才显示删除
        /// </summary>
        MyDepartment = 4,
    }
}

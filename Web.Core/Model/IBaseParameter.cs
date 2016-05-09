namespace Portal.Web.Core.Model
{
    public interface IBaseParameter
    {
        /// <summary>
        /// 是否去掉分页，为true则不分页，为false则分页。默认分页
        /// </summary>
        bool NoPage { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        int PageIndex { get; set; }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        long TotalRows { get; set; }
    }
}

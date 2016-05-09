using Portal.Dto;

namespace Portal.Web.Core.Model
{
    public class BaseParameter : IBaseParameter
    {
        #region 属性
        /// <summary>
        /// 是否不是第一次加载
        /// </summary>
        public bool IsPostBack { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 是否去掉分页，为true则不分页，为false则分页。默认分页
        /// </summary>
        public bool NoPage { get; set; }
        /// <summary>
        /// 排序ID
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 总行数
        /// </summary>
        public long TotalRows { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 新的访问权限令牌
        /// </summary>
        public string VisitToken { get; set; }
        #endregion

        #region 字段
        /// <summary>
        /// 跳转JS方法名
        /// </summary>
        public string GoPagerName;
        /// <summary>
        /// 当前页的起始下标
        /// </summary>
        public int Start;

        /// <summary>
        /// 当前页的结束下标
        /// </summary>
        public int End;
        #endregion

        #region 初始化
        public BaseParameter()
        {
            PageIndex = 1;
            GoPagerName = "GoPager";
        }
        public BaseParameter(int pageindex, int pagesize, int sort)
            : this()
        {
            PageIndex = pageindex;
            PageSize = pagesize;
            Sort = sort;//0表示降序1表示升序
        }
        public BaseParameter(BaseParameter parameter)
        {
            PageIndex = parameter.PageIndex > 0 ? parameter.PageIndex : 1;
            PageSize = parameter.PageSize;
            Sort = parameter.Sort;
        }
        public BaseParameter(PagerFindRequest parameter)
            : this()
        {
            PageIndex = parameter.PageIndex;
            PageSize = parameter.PageSize;
        }
        #endregion
    }
}

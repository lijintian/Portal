
namespace Portal.Web.Core.Model
{
    public class CreatePagingInfo
    {
        #region 字段
        public bool IsClient { get; set; }
        /// <summary>
        /// 如果起始位置大于1生成前面的省略号
        /// </summary>
        public bool ShowStart { get; set; }

        /// <summary>
        /// 如果结束位置小于最大页数生成后面的省略号
        /// </summary>
        public bool ShowEnd { get; set; }

        /// <summary>
        /// 最多显示多少页的数据
        /// </summary>
        public int ShowPageCount { get; set; }

        /// <summary>
        /// 是否显示设置每次显示条数下拉框
        /// </summary>
        public bool ShowSetPageSize { get; set; }

        /// <summary>
        /// 设置每页显示多少条的下拉框事件，当ShowSetPageSize=true时有效
        /// </summary>
        public string OnChangeName { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string OldHrefFormat { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string HrefFormat { get; set; }

        /// <summary>
        /// 是否显示一共多少条数据,true=显示
        /// </summary>
        public bool ShowAllCount { get; set; }

        /// <summary>
        /// 是否显示页面跳转，true=显示
        /// </summary>
        public bool ShowJump { get; set; }
        #endregion

        #region 初始化
        public CreatePagingInfo()
        {
            OldHrefFormat = "<a href=\"javascript:{0}({{0}})\">{{1}}</a>";
            ShowPageCount = 8;
            ShowStart = true;
            ShowEnd = true;
            ShowSetPageSize = false;
            ShowAllCount = true;
            ShowJump = true;
        }
        public CreatePagingInfo(int count)
            : this()
        {
            ShowPageCount = count;
        }
        public CreatePagingInfo(string hrefFormat)
            : this()
        {
            OldHrefFormat = hrefFormat;
        }
        #endregion

        #region 03.方法
        public void InitInfo(string jsname)
        {
            HrefFormat = string.Format(OldHrefFormat, jsname);
        }
        #endregion
    }
}

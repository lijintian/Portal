using System.ComponentModel;

namespace Web.Host.Test.Model
{
    public enum ExecuteType
    {
        [Description("获取页面")]
        Index = 1,

        [Description("获取一行")]
        GetModel = 2,

        [Description("添加")]
        Create = 3,

        [Description("更新")]
        Update = 4,

        [Description("保存")]
        Save = 5,

        [Description("删除")]
        Delete = 6,

        [Description("删除更多")]
        DeleteMore = 7,

        [Description("排序")]
        OrderBy = 8
    }
}

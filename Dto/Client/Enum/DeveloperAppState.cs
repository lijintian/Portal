using System.ComponentModel;

namespace Portal.Dto
{
    /// <summary>
    /// 开发者应用的状态
    /// </summary>
    public enum DeveloperAppState
    {
        [Description("开发中")]
        Developing = 1,
        [Description("审核中")]
        Verifying = 2,
        [Description("审核通过")]
        Approved = 3,
        [Description("禁用")]
        Disable=4
    }

    /// <summary>
    /// 开发者应用的状态(用于查询)
    /// </summary>
    public enum SearchDeveloperAppState
    {
        [Description("开发中")]
        Developing = 1,
        [Description("审核中")]
        Verifying = 2,
        [Description("审核通过")]
        Approved = 3
    }
}

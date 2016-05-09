using System.ComponentModel;

namespace Portal.Dto
{
    /// <summary>
    /// 应用程序类型
    /// </summary>
    public enum DeveloperAppType
    {
        [Description("网站应用")]
        Web = 1,
        [Description("桌面应用")]
        Desktop = 2,
        [Description("移动应用")]
        MobileApp = 3
    }
}

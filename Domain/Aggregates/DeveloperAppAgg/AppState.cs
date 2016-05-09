namespace Portal.Domain.Aggregates.DeveloperAppAgg
{
    /// <summary>
    /// 表示应用的状态
    /// </summary>
    public enum AppState
    {
        Developing = 1,
        Verifying = 2,
        Approved = 3,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable=4
    }
}

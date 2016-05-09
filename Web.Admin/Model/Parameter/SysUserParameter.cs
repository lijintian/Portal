namespace Portal.Web.Admin.Model
{
    public class SysUserParameter
    {
        #region 属性
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 类型：1表示内部帐号，2表示客户帐号，3表示API帐号，参考枚举：UserTypeEm
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 状态,0禁用,1启用,参考枚举：UserState
        /// </summary>
        public int State { get; set; }
        #endregion
    }
}

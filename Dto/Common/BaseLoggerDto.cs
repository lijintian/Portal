
namespace Portal.Dto.Common
{
    public class BaseLoggerDto
    {
        #region 属性
        /// <summary>
        /// 应用系统ID
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// 访问用户IP
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 访问的地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 客户端使用的浏览器名称和版本号
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// 客户端使用的系统
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// 创建人员账号
        /// </summary>
        public string CreatedBy { get; set; }
        #endregion

        #region 方法
        #endregion
    }
}

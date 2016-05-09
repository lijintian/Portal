
namespace Portal.Infrastructure.Exceptions
{
    /// <summary>
    /// 错误级别
    /// </summary>
    public enum ErrorLevel
    {
        /// <summary>
        /// 一切正常
        /// </summary>
        Ok = 200,

        /// <summary>
        /// 请求无效，需要附加细节解释如 "JSON无效"
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// 没有发现该资源
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// API开发者应该避免这种错误
        /// </summary>
        InternalServerError = 500,
    }
}

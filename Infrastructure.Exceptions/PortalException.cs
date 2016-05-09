

using EasyDDD.Infrastructure.Crosscutting.Exceptions;

namespace Portal.Infrastructure.Exceptions
{
    public class PortalException : ExceptionBase
    {
        #region 属性
        public override string Message
        {
            get { return "Portal:" + ErrorCode + "  Message: " + CustomMessage; }
        }
        #endregion

        #region 初始化
        public PortalException(string errorCode, string message)
            : base(errorCode, message)
        {
           
        }
      
        #endregion
    }
}

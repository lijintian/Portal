namespace Portal.Dto.Common
{
    public class ResponseBase
    {
        #region 属性
        public ErrorData ErrorData { get; set; }

        public bool HasError
        {
            get
            {
                bool hasError = ErrorData != null && !string.IsNullOrEmpty(ErrorData.Message);
                return hasError;
            }
        }
        #endregion

        #region 初始化
        public ResponseBase()
        { }
        public ResponseBase(string errorCode, string errorMessage)
        {
            this.ErrorData = new ErrorData(errorCode, errorMessage);
        }
        #endregion
    }
}

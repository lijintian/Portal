using System.ComponentModel;

namespace Portal.Dto
{
    public class FindPermissionRequest : PagerFindRequest
    {
        #region 属性
        public string ApplicationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsOpened { get; set; }
        public bool? IsCustomerGranted { get; set; }
        #endregion

        #region 初始化
        public FindPermissionRequest()
            : base()
        {
        }
        #endregion

        #region 方法
        public bool IsSetValueForIsOpened()
        {
            return this.IsOpened.HasValue;
        }
        public bool IsSetValueForIsCustomerGranted()
        {
            return this.IsCustomerGranted.HasValue;
        }

        public bool IsSetValueForApplication()
        {
            return !string.IsNullOrEmpty(this.ApplicationId);
        }

        public bool IsSetValueForName()
        {
            return !string.IsNullOrEmpty(this.Name);
        }

        public bool IsSetValueForCode()
        {
            return !string.IsNullOrEmpty(this.Code);
        }
        #endregion
    }

    public enum PermissionType
    {
        Page,
        Function,
        Api
    }
}

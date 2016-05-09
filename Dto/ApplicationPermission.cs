using System.Collections.Generic;
using System.Linq;

namespace Portal.Dto
{
    public abstract class ApplicationPermission : Permission
    {
        #region 属性
        /// <summary>
        /// 所属应用程序
        /// </summary>
        public string ApplicationId { get; set; }
        /// <summary>
        /// 所属应用程序名称
        /// </summary>
        public string ApplictionName { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 获取应用程序名称
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public void SetApplictionName(IEnumerable<Application> list)
        {
            var appInfo = list.FirstOrDefault(u => u.Id == ApplicationId);
            ApplictionName = appInfo == null ? string.Empty : appInfo.CnName;
        }
        #endregion
    }
}

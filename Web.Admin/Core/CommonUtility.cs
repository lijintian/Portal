using System;
using Portal.Dto;

namespace Portal.Web.Admin.Core
{
    public static class CommonUtility
    {
        /// <summary>
        /// 初始化用户操作信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        public static void InitOperateInfo<T>(this T info) where T : DomainDto
        {
            if (string.IsNullOrEmpty(info.Id))
            {
                info.CreatedBy = PageUtility.CurrentUser.Identity.Name;
                info.CreatedOn = DateTime.Now;
            }
            info.UpdatedBy = PageUtility.CurrentUser.Identity.Name;
            info.UpdatedOn = DateTime.Now;
        }
    }
}

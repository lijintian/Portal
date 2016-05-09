using System;
using System.Collections.Generic;
using Portal.Applications.Helper;
using Portal.Applications.Services;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Application;
using Portal.Domain.Specification.Permission;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Web.Core.Common;
using PagePermissionDomain = Portal.Domain.Aggregates.PermissionAgg.PagePermission;
using MenuDto = Portal.Dto.Menu;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Events.Handler
{
    public class ImportMenuCheckEventHandler : BaseImportCheckEventHandler<ImportMenuCheckEvent, ImportMenuRequest, MenuDto>
    {
        #region 字段
        private readonly IMenuManagerService _menuService;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IApplicationRepository applicationRepository;
        #endregion

        #region 初始化
        public ImportMenuCheckEventHandler(IMenuManagerService MenuService, IApplicationRepository ApplicationRepository, IPermissionRepository PersssionRepository)
        {
            Check.Argument.IsNotNull(MenuService, "MenuService");
            Check.Argument.IsNotNull(ApplicationRepository, "ApplicationRepository");
            Check.Argument.IsNotNull(PersssionRepository, "PersssionRepository");
            this._menuService = MenuService;
            this.applicationRepository = ApplicationRepository;
            this._permissionRepository = PersssionRepository;
        }
        #endregion

        #region 方法
        public override MenuDto GetModel(ImportMenuRequest item, ImportMenuCheckEvent domainEvent, List<string> errorList)
        {
            MenuDto dto = new MenuDto()
            {
                Name = item.Name,
                ParentId = item.ParentId,
                Target = (int)MenuTarget.Self,
                IsShare = !CheckUtility.IsEmpty(item.IsShare) && item.IsShare == "是",
                Url = item.Url,
                Order = 0,
                Desc = item.Desc,
                PermissionCode = item.PermissionCode,
                CreatedBy = domainEvent.Logger.CreatedBy,
                CreatedOn = DateTime.Now
            };
            if (CheckHelper.CheckNotEmpty(item.ApplicationCode, "应用系统英文名称", errorList))
            {
                var domain = applicationRepository.Get(new ApplicationEnNameSpecification(item.ApplicationCode));
                if (domain == null)
                {
                    errorList.Add(string.Format("应用系统英文名称【{0}】不存在", item.ApplicationCode));
                }
                else
                {
                    dto.ApplicationId = domain.Id;
                }
            }
            if (!CheckUtility.IsEmpty(item.ParentId))
            {
                var domain = _menuService.GetById(item.ParentId);
                if (domain == null)
                {
                    errorList.Add(string.Format("上级菜单ID【{0}】不存在", item.ParentId));
                }
            }
            CheckHelper.CheckNotEmpty(item.Name, "菜单名称", errorList);
            if (!CheckUtility.IsEmpty(item.PermissionCode))
            {
                var exists = _permissionRepository.Exists(false, new PermissionCodeSpecification(item.PermissionCode));
                if (!exists)
                {
                    errorList.Add("系统不存在系统页面权限码:{0}".FormatWith(item.PermissionCode));
                }
            }
            if (!CheckUtility.IsEmpty(item.Target))
            {
                var targetResult = CheckHelper.CheckEnum<MenuTarget>(item.Target, "打开方式", errorList);
                if (targetResult.Status)
                {
                    dto.Target = (int)targetResult.Data;
                }
            }
            var orderResult = CheckHelper.CheckInt(item.Order, "排序", errorList, true);
            if (orderResult.Status)
            {
                dto.Order = orderResult.Data;
                CheckHelper.CheckRange(orderResult.Data, "排序", errorList);
            }
            CheckHelper.CheckByteLen(item.Desc, 200, "备注", errorList);
            return dto;
        }

        public override void Save(MenuDto dto, SysLoggerDto logger)
        {
            _menuService.Save(dto, logger);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using Portal.Applications.Helper;
using Portal.Applications.Services;
using Portal.Domain.Aggregates.PermissionAgg.Events;
using Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Application;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Web.Core;
using Portal.Web.Core.Common;
using RoleDto = Portal.Dto.Role;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Events.Handler
{
    public class ImportRoleCheckEventHandler : BaseImportCheckEventHandler<ImportRoleCheckEvent, ImportRoleRequest, RoleDto>
    {
        #region 字段
        private readonly IRoleManagerService roleService;
        private readonly IApplicationRepository applicationRepository;
        private readonly IAuthorizationManagerService _authorizationManagerService;
        #endregion

        #region 初始化
        public ImportRoleCheckEventHandler(IRoleManagerService RoleService, IApplicationRepository ApplicationRepository, IAuthorizationManagerService AuthorizationManagerService)
        {
            Check.Argument.IsNotNull(RoleService, "RoleService");
            Check.Argument.IsNotNull(ApplicationRepository, "ApplicationRepository");
            Check.Argument.IsNotNull(AuthorizationManagerService, "AuthorizationManagerService");
            this.roleService = RoleService;
            this.applicationRepository = ApplicationRepository;
            this._authorizationManagerService = AuthorizationManagerService;
        }
        #endregion

        #region 方法
        public override RoleDto GetModel(ImportRoleRequest item, ImportRoleCheckEvent domainEvent, List<string> errorList)
        {
            RoleDto dto = new RoleDto
            {
                Name = item.Name,
                Code = item.Code,
                Desc = item.Desc,
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
            CheckHelper.CheckNotEmpty(item.Name, "角色名称", errorList);
            if (CheckHelper.CheckNotEmpty(item.Code, "角色代码", errorList))
            {
                CheckHelper.CheckNumLetter(item.Code, "角色代码", errorList);
                if (!roleService.IsUniqueCode(item.Code))
                {
                    errorList.Add("角色代码重复，请修改");
                }
            }
            if (!CheckUtility.IsEmpty(item.PermissionCodes))
            {
                string[] codes = item.PermissionCodes.Split(',');
                ValidatePermissionExistsEvent parameter = new ValidatePermissionExistsEvent(codes, dto.ApplicationId, true);
                DomainEvent.Publish<ValidatePermissionExistsEvent, ValidatePermissionExistsEventResult>(parameter,
                e =>
                {
                    if (e != null)
                    {
                        if (e.Success)
                        {
                            dto.Add(e.PermissionCodes);
                            if (!LengthUtility.IsNullOrEmpty(e.ParentPermissionCodes))
                            {
                                dto.Add(e.ParentPermissionCodes);
                            }
                        }
                        else
                        {
                            errorList.Add(e.ErrorMessage);
                        }
                    }
                });
            }
            CheckHelper.CheckByteLen(item.Desc, 200, "备注", errorList);
            return dto;
        }

        public override void Save(RoleDto dto, SysLoggerDto logger)
        {
            roleService.Save(dto, logger);
            if (dto.HasPermissionCode())
            {
                _authorizationManagerService.SavePermissionsToRole(dto.Id, dto.PermissionCode, logger);
            }
        }
        #endregion
    }
}

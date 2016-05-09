using System.Collections.Generic;
using System.Linq;
using Portal.Applications.Helper;
using Portal.Applications.Services;
using Portal.Domain.Aggregates.PermissionAgg.Events;
using Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Role;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Infrastructure.Exceptions;
using Portal.Web.Core;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Events.Handler
{
    /// <summary>
    /// 批量调整角色权限（新增权限、删除权限）
    /// </summary>
    public class ImportRolePermissionCheckEventHandler : BaseImportCheckEventHandler<ImportRolePermissionCheckEvent, ImportRolePermissionRequest, RolePermissionDto>
    {
        #region 字段
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthorizationManagerService _authorizationManagerService;
        #endregion

        #region 初始化
        public ImportRolePermissionCheckEventHandler(IRoleRepository RoleRepository, IAuthorizationManagerService AuthorizationManagerService)
        {
            Check.Argument.IsNotNull(RoleRepository, "RoleRepository");
            Check.Argument.IsNotNull(AuthorizationManagerService, "AuthorizationManagerService");
            this._roleRepository = RoleRepository;
            this._authorizationManagerService = AuthorizationManagerService;
        }
        #endregion

        #region 方法
        public override RolePermissionDto GetModel(ImportRolePermissionRequest item, ImportRolePermissionCheckEvent domainEvent, List<string> errorList)
        {
            RolePermissionDto dto = new RolePermissionDto() { CreatedBy = domainEvent.Logger.CreatedBy };
            if (CheckHelper.CheckNotEmpty(item.Code, "角色代码", errorList))
            {
                var domain = _roleRepository.Get(new RoleCodeSpecification(item.Code));
                if (domain == null)
                {
                    errorList.Add(string.Format("角色代码【{0}】不存在", item.Code));
                }
                else
                {
                    dto.RoleId = domain.Id;
                    if (domainEvent.IsCreate)
                    {
                        dto.ApplicationId = domain.ApplicationId;
                    }
                    dto.PermissionCode = domain.Permissions;
                }
            }
            if (CheckHelper.CheckNotEmpty(item.PermissionCodes, "权限码", errorList) && !CheckHelper.CheckComma(item.PermissionCodes, "权限码", errorList))
            {
                string[] perCodes = item.PermissionCodes.Split(',');
                if (!domainEvent.IsCreate)
                {
                    dto.Remove(perCodes);
                }
                ValidatePermissionExistsEvent parameter = new ValidatePermissionExistsEvent(perCodes, dto.ApplicationId, domainEvent.IsCreate);
                DomainEvent.Publish<ValidatePermissionExistsEvent, ValidatePermissionExistsEventResult>(parameter,
                e =>
                {
                    if (e != null)
                    {
                        if (e.Success)
                        {
                            if (domainEvent.IsCreate)
                            {
                                dto.Add(e.PermissionCodes);
                                if (!LengthUtility.IsNullOrEmpty(e.ParentPermissionCodes))
                                {
                                    dto.Add(e.ParentPermissionCodes);
                                }
                            }
                        }
                        else
                        {
                            errorList.Add(e.ErrorMessage);
                        }
                    }
                });
            }
            return dto;
        }

        public override void Save(RolePermissionDto dto,SysLoggerDto logger)
        {
            _authorizationManagerService.SavePermissionsToRole(dto.RoleId, dto.PermissionCode, logger);
        }
        #endregion
    }

    public class RolePermissionDto
    {
        #region 属性
        public string RoleId { get; set; }
        public string ApplicationId { get; set; }
        public IEnumerable<string> PermissionCode { get; set; }
        public string CreatedBy { get; set; }
        #endregion

        #region 方法
        public void Add(string[] codes)
        {
            PermissionCode = PermissionCode.IsNullOrEmpty() ? codes : PermissionCode.Union(codes);
        }

        public void Remove(string[] codes)
        {
            if (!PermissionCode.IsNullOrEmpty())
            {
                PermissionCode = PermissionCode.Where(item => !codes.Contains(item));
            }
        }
        #endregion
    }
}

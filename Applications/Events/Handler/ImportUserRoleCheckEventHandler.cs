using System;
using System.Collections.Generic;
using System.Linq;
using Portal.Applications.Helper;
using Portal.Applications.Services;
using Portal.Domain.Aggregates.RoleAgg.Events;
using Portal.Domain.Aggregates.RoleAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.User;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Infrastructure.Exceptions;
using Portal.Web.Core;
using UserType = Portal.Domain.Aggregates.UserAgg.UserType;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Events.Handler
{
    /// <summary>
    /// 批量调整用户角色（新增角色、删除角色）
    /// </summary>
    public class ImportUserRoleCheckEventHandler : BaseImportCheckEventHandler<ImportUserRoleCheckEvent, ImportUserRoleRequest, UserRoleDto>
    {
        #region 字段
        private readonly IUserRepository _userRepository;
        private readonly IAuthorizationManagerService _authorizationManagerService;
        #endregion

        #region 初始化
        public ImportUserRoleCheckEventHandler(IUserRepository UserRepository, IAuthorizationManagerService AuthorizationManagerService)
        {
            Check.Argument.IsNotNull(UserRepository, "UserRepository");
            Check.Argument.IsNotNull(AuthorizationManagerService, "AuthorizationManagerService");
            this._userRepository = UserRepository;
            this._authorizationManagerService = AuthorizationManagerService;
        }
        #endregion

        #region 方法
        public override UserRoleDto GetModel(ImportUserRoleRequest item, ImportUserRoleCheckEvent domainEvent, List<string> errorList)
        {
            UserRoleDto dto = new UserRoleDto() { CreatedBy = domainEvent.Logger.CreatedBy };
            if (CheckHelper.CheckNotEmpty(item.LoginName, "用户名称", errorList))
            {
                var domain = _userRepository.Get(new UserTypeSpecification(UserType.Employee).And(new UserLoginNameSpecification(item.LoginName)));
                if (domain == null)
                {
                    errorList.Add(string.Format("用户名称【{0}】不存在", item.LoginName));
                }
                else
                {
                    dto.UserId = domain.Id;
                    dto.RoleCode = domain.Roles;
                }
            }
            if (CheckHelper.CheckNotEmpty(item.RoleCodes, "角色代码", errorList) && !CheckHelper.CheckComma(item.RoleCodes, "角色代码", errorList))
            {
                string[] roleCodes = item.RoleCodes.Split(',');
                if (domainEvent.IsCreate)
                {
                    dto.Add(roleCodes);
                }
                else
                {
                    dto.Remove(roleCodes);
                }
                DomainEvent.Publish<ValidateRoleExistsEvent, ValidateRoleExistsEventResult>(new ValidateRoleExistsEvent(roleCodes),
                    e =>
                    {
                        if (e != null)
                        {
                            if (e.NoFoundRoleCodes != null && e.NoFoundRoleCodes.Any())
                            {
                                string codes = string.Join(",", e.NoFoundRoleCodes);
                                errorList.Add(ErrorMessage.NoFoundRoleCode.FormatWith(codes));
                            }
                        }
                    });
            }
            return dto;
        }

        public override void Save(UserRoleDto dto, SysLoggerDto logger)
        {
            _authorizationManagerService.SaveUserToRoles(dto.UserId, dto.RoleCode, logger);
        }
        #endregion
    }

    public class UserRoleDto
    {
        #region 属性
        public string UserId { get; set; }
        public IEnumerable<string> RoleCode { get; set; }
        public string CreatedBy { get; set; }
        #endregion

        #region 方法
        public void Add(string[] codes)
        {
            RoleCode = RoleCode.IsNullOrEmpty() ? codes : RoleCode.Union(codes);
        }

        public void Remove(string[] codes)
        {
            if (!RoleCode.IsNullOrEmpty())
            {
                RoleCode = RoleCode.Where(item => !codes.Contains(item));
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Portal.Applications.Helper;
using Portal.Applications.Services;
using Portal.Domain.Aggregates.PermissionAgg.Events;
using Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.User;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Web.Core;
using UserType = Portal.Domain.Aggregates.UserAgg.UserType;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Events.Handler
{
    /// <summary>
    /// 批量调整用户权限（新增权限、删除权限）
    /// </summary>
    public class ImportCustomerPermissionCheckEventHandler : BaseImportCheckEventHandler<ImportCustomerPermissionCheckEvent, ImportCustomerPermissionRequest, CustomerPermissionDto>
    {
        #region 字段
        private readonly IUserRepository _userRepository;
        private readonly IAuthorizationManagerService _authorizationManagerService;
        #endregion

        #region 初始化
        public ImportCustomerPermissionCheckEventHandler(IUserRepository UserRepository, IAuthorizationManagerService AuthorizationManagerService)
        {
            Check.Argument.IsNotNull(UserRepository, "UserRepository");
            Check.Argument.IsNotNull(AuthorizationManagerService, "AuthorizationManagerService");
            this._userRepository = UserRepository;
            this._authorizationManagerService = AuthorizationManagerService;
        }
        #endregion

        #region 方法
        public override CustomerPermissionDto GetModel(ImportCustomerPermissionRequest item, ImportCustomerPermissionCheckEvent domainEvent, List<string> errorList)
        {
            CustomerPermissionDto dto = new CustomerPermissionDto() { CreatedBy = domainEvent.Logger.CreatedBy };
            if (CheckHelper.CheckNotEmpty(item.ClientNo, "客户代码", errorList))
            {
                var domain = _userRepository.Get(new UserTypeSpecification(UserType.Customer).And(new UserClientNoSpecification(item.ClientNo)));
                if (domain == null)
                {
                    errorList.Add(string.Format("客户代码【{0}】不存在", item.ClientNo));
                }
                else
                {
                    dto.UserId = domain.Id;
                    dto.PermissionCode = domain.Permissions;
                }
            }
            if (CheckHelper.CheckNotEmpty(item.PermissionCodes, "权限码", errorList) && !CheckHelper.CheckComma(item.PermissionCodes, "权限码", errorList))
            {
                string[] perCodes = item.PermissionCodes.Split(',');
                if (domainEvent.IsCreate)
                {
                    dto.Add(perCodes);
                }
                else
                {
                    dto.Remove(perCodes);
                }
                ValidatePermissionExistsEvent parameter = new ValidatePermissionExistsEvent(perCodes, domainEvent.IsCreate);
                DomainEvent.Publish<ValidatePermissionExistsEvent, ValidatePermissionExistsEventResult>(parameter,
                e =>
                {
                    if (e != null)
                    {
                        if (e.Success)
                        {
                            if (domainEvent.IsCreate && !LengthUtility.IsNullOrEmpty(e.ParentPermissionCodes))
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
            return dto;
        }

        public override void Save(CustomerPermissionDto dto, SysLoggerDto logger)
        {
            _authorizationManagerService.SaveUserOwnedPermissions(dto.UserId, dto.PermissionCode, logger);
        }
        #endregion
    }

    public class CustomerPermissionDto
    {
        #region 属性
        public string UserId { get; set; }
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

using System.Collections.Generic;
using Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events.Callbacks;
using Portal.Domain.Aggregates.PermissionAgg;
using Portal.Domain.Model;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.ApiPermissionGroup;
using System;
using System.Linq;
using Portal.Domain.Specification.Permission;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events.Handlers
{
    public class ValidateApiPermissionGroupExistsEventHandler : IDomainEventHandler<ValidateApiPermissionGroupExistsEvent>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IApiPermissionGroupRepository _apiGroupRepository;
        public ValidateApiPermissionGroupExistsEventHandler(IApiPermissionGroupRepository apiGroupRepository, IPermissionRepository permissionRepository)
        {
            Check.Argument.IsNotNull(apiGroupRepository, "apiGroupRepository");
            Check.Argument.IsNotNull(permissionRepository, "permissionRepository");
            this._apiGroupRepository = apiGroupRepository;
            this._permissionRepository = permissionRepository;
        }

        public void Handle(ValidateApiPermissionGroupExistsEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现,因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidateApiPermissionGroupExistsEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");
            var groups = _apiGroupRepository.GetList(new ApiPermissionGroupCodeListSpecification(domainEvent.Codes));
            var noFoundApiPermissionGroups = domainEvent.Codes.Except(groups.Select(c => c.Code));
            if (noFoundApiPermissionGroups.Any())
            {
                var result = new ValidateApiPermissionGroupExistsEventResult(noFoundApiPermissionGroups.ToArray());
                if (callback != null)
                {
                    callback((TDomainEventResult)(IDomainEventResult)result);
                }
            }
            else
            {
                var codeList = new List<string>();
                foreach (var item in groups)
                {
                    codeList.AddRange(item.Permissions);
                }
                var permissions = this._permissionRepository.GetList(new PermissionCodeListSpecification(codeList.ToArray()));
                if (permissions == null || !permissions.Any())
                {
                    throw new PortalException(ErrorCodes.StringCodes.DeveloperApplicatinNoFoundCode, ErrorMessage.DeveloperApplicatinNoFoundCode);
                }
                var apiPermissions = Array.ConvertAll(permissions.ToArray(), item => (ApiPermission)item);
                ReferenceApiPermssionInfo[] permissionList = apiPermissions.Select(item => new ReferenceApiPermssionInfo(item.Code, item.IsOpened, item.IsCustomerGranted)).ToArray();
                var result = new ValidateApiPermissionGroupExistsEventResult(domainEvent.Codes, permissionList);
                if (callback != null)
                {
                    callback((TDomainEventResult)(IDomainEventResult)result);
                }
            }
        }

    }
}

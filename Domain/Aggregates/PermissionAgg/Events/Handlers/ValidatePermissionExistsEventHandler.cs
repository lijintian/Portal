using System.Collections.Generic;



using Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Permission;
using System;
using System.Linq;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.PermissionAgg.Events.Handlers
{
    public class ValidatePermissionExistsEventHandler : IDomainEventHandler<ValidatePermissionExistsEvent>
    {
        private readonly IPermissionRepository _permissionRepository;
        public ValidatePermissionExistsEventHandler(IPermissionRepository permissionRepository)
        {
            Check.Argument.IsNotNull(permissionRepository, "permissionRepository");
            this._permissionRepository = permissionRepository;
        }

        public void Handle(ValidatePermissionExistsEvent domainEvent)
        {
            throw new NotImplementedException();//不需要实现,因为需要CallBack
        }

        public void Handle<TDomainEventResult>(ValidatePermissionExistsEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");
            var list = _permissionRepository.GetList(new PermissionCodeListSpecification(domainEvent.PermissionCodes.ToArray()));
            var noFoundPermissions = domainEvent.PermissionCodes.Except(list.Select(c => c.Code)).ToArray();
            List<string> errorList = new List<string>();
            if (noFoundPermissions.Any())
            {
                errorList.Add(ErrorMessage.NoFoundPermissionCode.FormatWith(string.Join(",", noFoundPermissions)));
            }
            if (list.Any() && (domainEvent.IsOpend || domainEvent.IsUnCustomerGranted))
            {
                List<ApiPermission> apiList = list.Select(item => item as ApiPermission).Where(u => u != null).ToList();
                if (apiList.Any())
                {
                    string codes = string.Empty;
                    if (domainEvent.IsOpend)
                    {
                        codes = string.Join(",", apiList.Where(u => !u.IsOpened).Select(u => u.Code).ToArray());
                        if (!string.IsNullOrEmpty(codes))
                        {
                            errorList.Add(ErrorMessage.IsOpenedPermissionCode.FormatWith(codes));
                        }
                    }
                    if (domainEvent.IsUnCustomerGranted)
                    {
                        codes = string.Join(",", apiList.Where(u => u.IsCustomerGranted).Select(u => u.Code).ToArray());
                        if (!string.IsNullOrEmpty(codes))
                        {
                            errorList.Add(ErrorMessage.IsCustomerGrantedPermissionCode.FormatWith(codes));
                        }
                    }
                }
            }
            var result = new ValidatePermissionExistsEventResult(string.Join("；", errorList));
            //过滤属于某个应用系统下的权限
            if (list.Any() && !string.IsNullOrEmpty(domainEvent.ApplicationId))
            {
                var foundPermissions = list.Select(u => u.Code).ToArray();
                list = list.Where(u => u.ApplicationId == domainEvent.ApplicationId).ToList();
                result.PermissionCodes = list.Select(u => u.Code).ToArray();
                var appNoFoundPermissions = foundPermissions.Except(result.PermissionCodes).ToArray();
                if (appNoFoundPermissions.Any())
                {
                    result.Init(ErrorMessage.AppNoFoundPermissionCode.FormatWith(string.Join(",", appNoFoundPermissions)));
                }
            }
            if (list.Any() && domainEvent.IsGetParentPermissionCodes && result.Success)
            {
                List<FunctionPermission> funList = list.Select(u => u as FunctionPermission).Where(u => u != null).ToList();
                if (funList.Any())
                {
                    string[] parentIdList = funList.Where(u => u.HasParent()).Select(u => u.ParentId).Distinct().ToArray();
                    if (parentIdList.Any())
                    {
                        var parentList = _permissionRepository.GetList(new PermissionIdListSpecification(parentIdList));
                        if (parentList.Any())
                        {
                            result.ParentPermissionCodes = parentList.Select(u => u.Code).ToArray();
                        }
                    }
                }
            }
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}



using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using Portal.Domain.Aggregates.SynchronizationAgg;
using Portal.Domain.Repositories;
using System;
using System.Linq;

namespace Portal.Domain.Aggregates.PermissionAgg.Events.Handlers
{
    public class AddOrUpdateFunctionPermissionEventHandler : IDomainEventHandler<AddOrUpdateFunctionPermissionEvent>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ISynchronizationInfoRepository _synchronizationInfoRepository;
        public AddOrUpdateFunctionPermissionEventHandler(IPermissionRepository persssionRepository, ISynchronizationInfoRepository syncRepository)
        {
            Check.Argument.IsNotNull(persssionRepository, "persssionRepository");
            Check.Argument.IsNotNull(syncRepository, "syncRepository");

            this._permissionRepository = persssionRepository;
            this._synchronizationInfoRepository = syncRepository;
        }

        public void Handle(AddOrUpdateFunctionPermissionEvent domainEvent)
        {
            Check.Argument.IsNotNull(domainEvent, "domainEvent");

            string[] codes = domainEvent.FunctionPermissions.Select(c => c.Code).ToArray();
            var functionPermissions = _permissionRepository.GetFunctionPermission(codes).ToArray();

            var noFoundFuncPermissionCodes = codes.Except(functionPermissions.Select(c => c.Code));

            PagePermission source  = domainEvent.Source as PagePermission;
            if (source == null) throw new NullReferenceException("AddOrUpdateFunctionPermissionEvent source is null");
            //处理新增的
            var noFoundFuncPermissions = domainEvent.FunctionPermissions.Where(c => noFoundFuncPermissionCodes.Contains(c.Code));
            foreach (var item in noFoundFuncPermissions)
            {
                var newFunc = new FunctionPermission(source.ApplicationId, source.Id ,item.Code, item.Name)
                {
                    Desc = item.Desc,
                    IsCompatible = item.IsCompatible,
                    Order = item.Order,
                    Tag = item.Tag,
                    CreatedOn = source.UpdatedOn,
                    CreatedBy = source.UpdatedBy,
                    UpdatedOn = source.UpdatedOn,
                    UpdatedBy = source.UpdatedBy
                };

                _permissionRepository.Add(newFunc);

                if (newFunc.IsCompatible)
                {
                    _synchronizationInfoRepository.Add(new SynchronizationInfo() { ModifiedId = newFunc.Id, Type = ModifiedType.NewPermission });
                }
            }

            //处理修改的

            foreach (var item in functionPermissions)
            {
                var find = domainEvent.FunctionPermissions.FirstOrDefault(c => c.Code == item.Code);
                if (find != null)
                {
                    item.Desc = find.Desc;
                    item.IsCompatible = find.IsCompatible;
                    item.Tag = find.Tag;
                    item.Name = find.Name;
                    item.Order = find.Order;
                    item.UpdatedOn = source.UpdatedOn;
                    item.UpdatedBy = source.UpdatedBy;
                    _permissionRepository.Update(item);
                }
            }
        }

        public void Handle<TDomainEventResult>(AddOrUpdateFunctionPermissionEvent domainEvent, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            throw new NotSupportedException("不支持");
        }
    }
}

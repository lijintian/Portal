using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Aggregates.SynchronizationAgg;
using Portal.Domain.Repositories;
using EasyDDD.Infrastructure.Crosscutting.Event;
using EasyDDD.Core.Repository;

namespace Portal.Applications.Events.Handler
{
    /// <summary>
    /// 表示同数数据处理
    /// </summary>
    public class SynchronizationEventHandle : IEventHandler<NewPermissionCreatedEvent>, IEventHandler<UserInfoChangedEvent>, IEventHandler<UserPermissionChangedEvent>
    {
        private readonly ISynchronizationInfoRepository _repository;
        private readonly IRepositoryContext _context;
        public SynchronizationEventHandle(ISynchronizationInfoRepository repository, IRepositoryContext context)
        {
            this._repository = repository;
            this._context = context;
        }

        public void Handle(NewPermissionCreatedEvent newPermissionCreatedEvent)
        {
            this.SaveSyncInfo(new SynchronizationInfo() { ModifiedId = newPermissionCreatedEvent.ModifiedId, Type = newPermissionCreatedEvent.Type});
        }

       
        public void Handle(UserInfoChangedEvent userInfoChangedEvent)
        {
            this.SaveSyncInfo(new SynchronizationInfo() { ModifiedId = userInfoChangedEvent.ModifiedId, Type = userInfoChangedEvent.Type });
        }

        public void Handle(UserPermissionChangedEvent userPermissionChangedEvent)
        {
            this.SaveSyncInfo(new SynchronizationInfo() { ModifiedId = userPermissionChangedEvent.ModifiedId, Type = userPermissionChangedEvent.Type });
        }

        private void SaveSyncInfo(SynchronizationInfo syncInfo)
        {
            this._repository.Add(syncInfo);
            this._context.Commit();
        }
    }
}

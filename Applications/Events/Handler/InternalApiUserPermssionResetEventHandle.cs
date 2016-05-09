using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Applications.Services;
using Portal.Domain.Aggregates.DeveloperAppAgg;
using Portal.Domain.Repositories;
using Portal.Domain.Services;
using Portal.Domain.Specification.DeveloperApp;
using EasyDDD.Infrastructure.Crosscutting.Event;

namespace Portal.Applications.Events.Handler
{
    /// <summary>
    /// 表示内部API应用的权限变更处理
    /// </summary>
    public class InternalApiUserPermssionResetEventHandle : IEventHandler<InternalApiUserPermssionResetEvent>
    {
        private readonly IInteralApiUserTokenGenerateService _service;
        public InternalApiUserPermssionResetEventHandle(IInteralApiUserTokenGenerateService service)
        {
            this._service = service;
        }

        public void Handle(InternalApiUserPermssionResetEvent evnt)
        {
            this._service.Generate(evnt.User);
        }
    }
}

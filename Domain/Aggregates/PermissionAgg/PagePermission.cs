

using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;
using Portal.Domain.Aggregates.PermissionAgg.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Portal.Domain.Aggregates.PermissionAgg
{
    /// <summary>
    /// 表示页面权限
    /// </summary>
    public class PagePermission : Permission
    {


        public PagePermission(string applicationId, string code, string name)
            : base(applicationId, code, name)
        {
            _functionPermissions = new List<string>();
        }


        private List<string> _functionPermissions;
        public ReadOnlyCollection<string> FunctionPermissions
        {
            get
            {
                return this._functionPermissions.AsReadOnly();
            }
            private set { _functionPermissions = value.ToList(); }
        }

        public void AddFunctionPermission(FunctionPermissionInfo func)
        {
            Check.Argument.IsNotNull(func, "func");
            DomainEvent.Publish(new AddOrUpdateFunctionPermissionEvent(this, new FunctionPermissionInfo[] { func }));
            var find = _functionPermissions.FirstOrDefault(c => c == func.Code);
            if (find == null)
            {
                _functionPermissions.Add(func.Code);
            }
        }

        public void AddFunctionPermissionRange(FunctionPermissionInfo[] funcs)
        {
            Check.Argument.IsNotNull(funcs, "funcs");
            if (funcs.Any())
            {
                DomainEvent.Publish(new AddOrUpdateFunctionPermissionEvent(this, funcs));

                var notFounds = funcs.Select(c => c.Code).Except(_functionPermissions).ToArray();
                if (notFounds.Any())
                {
                    _functionPermissions.AddRange(notFounds);
                }
            }
        }
    }
}

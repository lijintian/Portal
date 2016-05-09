using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Portal.Domain.Aggregates.PermissionAgg.Events;
using Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks;
using Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events;
using Portal.Domain.Aggregates.ApiPermissionGroupAgg.Events.Callbacks;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Aggregate;
using System.Linq;
using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.ApiPermissionGroupAgg
{
    /// <summary>
    /// API权限分组
    /// </summary>
    public class ApiPermissionGroup : AggregateRoot
    {
        protected ApiPermissionGroup()
        {
            this.GenerateNewIdentity();
            _permissions = new List<string>();
        }

        public ApiPermissionGroup(string code, string name)
            : this()
        {
            CheckArgument.IsNotNullOrEmpty(name, "name");
            Validate(code, name);
            this.Code = code;
            this.Name = name;

        }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Desc { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        private List<string> _permissions;
        public ReadOnlyCollection<string> Permissions
        {
            get { return _permissions.AsReadOnly(); }
            private set { _permissions = value.ToList(); }
        }

        public void ChangeName(string name)
        {
            if (this.Name == name) return;
            Validate(this.Code, name);
            this.Name = name;
        }

        public void ClearPerissions()
        {
            this._permissions.Clear();
        }

        public void SavePermissions(string[] permissionCodes)
        {
            DomainEvent.Publish<ValidatePermissionExistsEvent, ValidatePermissionExistsEventResult>(new ValidatePermissionExistsEvent(permissionCodes,true,false),
                e =>
                {
                    if (e != null)
                    {
                        if (!e.Success)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.NoFoundPermissionCode, e.ErrorMessage);
                        }
                        _permissions.Clear();
                        _permissions.AddRange(permissionCodes);
                    }
                });
        }

        private void Validate(string code, string roleName)
        {
            DomainEvent.Publish<ValidateApiPermissionGroupExistsSameNameEvent, ValidateApiPermissionGroupExistsSameNameEventResult>(new ValidateApiPermissionGroupExistsSameNameEvent(this, code, roleName),
                e =>
                {
                    if (e != null)
                    {
                        if (e.ExistsSameCode)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.ExistsSameApiPermissionGroupCode, ErrorMessage.ExistsSameApiPermissionGroupCode);
                        }
                        if (e.ExistsSameName)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.ExistsSameApiPermissionGroupName, ErrorMessage.ExistsSameApiPermissionGroupName);
                        }
                    }
                });
        }
    }
}

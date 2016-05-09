using System;



using Portal.Domain.Aggregates.PermissionAgg.Events;
using Portal.Domain.Aggregates.PermissionAgg.Events.Callbacks;
using Portal.Domain.Aggregates.RoleAgg.Events;
using Portal.Domain.Aggregates.RoleAgg.Events.Callbacks;
using Portal.Infrastructure.Exceptions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Event;

namespace Portal.Domain.Aggregates.RoleAgg
{
    /// <summary>
    /// Represents a role of system
    /// </summary>
    public class Role : AggregateRoot
    {
        protected Role()
        {
            this.GenerateNewIdentity();
            _permissions = new List<string>();
        }

        public Role(string applicationId, string code, string name)
            : this()
        {
            CheckArgument.IsNotNullOrEmpty(applicationId, "applicationId");
            CheckArgument.IsNotNullOrEmpty(name, "name");

            Validate(applicationId, code, name);
            this.ApplicationId = applicationId;
            this.Code = code;
            this.Name = name;

        }

        public string ApplicationId { get; private set; }
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
            Validate(this.ApplicationId, this.Code, name);
            this.Name = name;
        }

        public void ClearPerissions()
        {
            this._permissions.Clear();
        }

        public void SavePermissions(string[] permissionCodes)
        {
            DomainEvent.Publish<ValidatePermissionExistsEvent, ValidatePermissionExistsEventResult>(new ValidatePermissionExistsEvent(permissionCodes),
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

        private void Validate(string applicationId, string code, string roleName)
        {
            DomainEvent.Publish<ValidateRoleExistsSameNameEvent, ValidateRoleExistsSameNameEventResult>(new ValidateRoleExistsSameNameEvent(this, applicationId, code, roleName),
                e =>
                {
                    if (e != null)
                    {
                        if (e.ExistsSameCode)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.ExistsSameRoleCode, ErrorMessage.ExistsSameRoleCode);
                        }
                        if (e.ExistsSameName)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.ExistsSameRoleName, ErrorMessage.ExistsSameRoleName);
                        }
                    }
                });
        }
    }
}

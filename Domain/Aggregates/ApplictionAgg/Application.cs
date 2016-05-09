using System;
using Portal.Domain.Aggregates.ApplictionAgg.Events;
using Portal.Domain.Aggregates.ApplictionAgg.Events.Callbacks;
using Portal.Infrastructure.Exceptions;
using EasyDDD.Core.Aggregate;
using EasyDDD.Core.Event;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Domain.Aggregates.ApplictionAgg
{
    /// <summary>
    /// Represents an appliction 
    /// </summary>
    public class Application : AggregateRoot
    {
        public Application()
            : base()
        {
            this.GenerateNewIdentity();
        }

        public Application(string name, string enName)
            : this()
        {
            SetName(name, enName);
        }

        public virtual string EnName { get; private set; }

        public virtual string Name { get; private set; }

        public string Url { get; set; }

        public string Desc { get; set; }

        public string Note { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public void SetName(string name,string enName)
        {
            if (this.Name == name) return;

            DomainEvent.Publish<ValidateApplicationExistsSameNameEvent, ValidateApplicationExistsSameNameEventResult>
            (
                new ValidateApplicationExistsSameNameEvent(this, name),
                (e) =>
                {
                    if (e != null)
                    {
                        if (e.Exists)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.ExistsSameApplicationName, ErrorMessage.ExistsSameApplicationName.FormatWith(name));
                        }
                        this.Name = name;
                        SetEnName(enName);
                    }
                }
            );
        }

        public void SetEnName(string enName)
        {
            if (this.EnName == enName) return;

            DomainEvent.Publish<ValidateApplicationExistsSameEnNameEvent, ValidateApplicationExistsSameEnNameEventResult>
            (
                new ValidateApplicationExistsSameEnNameEvent(this, enName),
                (e) =>
                {
                    if (e != null)
                    {
                        if (e.Exists)
                        {
                            throw new PortalException(ErrorCodes.StringCodes.ExistsSameApplicationEnName, ErrorMessage.ExistsSameApplicationEnName.FormatWith(enName));
                        }
                        this.EnName = enName;
                    }
                }
            );
        }

        public void SetUrl(string url)
        {
            if (!string.IsNullOrEmpty(url) && !string.Equals(this.Url, url, StringComparison.InvariantCultureIgnoreCase))
            {
                DomainEvent.Publish(new ValidateApplicationUrlEvent(this, url));
            }
            this.Url = url;
        }
        //public void AddRole(Role aRole)
        //{

        //}

        //public void AddPermission(Permission aPermission)
        //{

        //}

        //public void AddMenu(Menu aMenu)
        //{

        //}

        //public IEnumerable<Role> GetAllRoles()
        //{
        //    return null;
        //}

        //public IEnumerable<Permission> GetAllPermissions()
        //{
        //    return null;
        //}

        //public IEnumerable<Menu> GetAllMenus()
        //{
        //    return null;
        //}
    }
}

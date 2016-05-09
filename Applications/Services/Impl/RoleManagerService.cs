using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Permission;
using Portal.Domain.Specification.Role;
using Portal.Dto;
using DomainRole = Portal.Domain.Aggregates.RoleAgg.Role;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 表示角色管理服务实现 
    /// </summary>
    public class RoleManagerService : ServiceBase, IRoleManagerService
    {
        protected IRoleRepository RoleRepository { get; private set; }
        protected IPermissionRepository PermissionRepository { get; private set; }
        public RoleManagerService(IRepositoryContext context, IRoleRepository roleRepository, IPermissionRepository perRepository)
            : base(context)
        {
            this.RoleRepository = roleRepository;
            this.PermissionRepository = perRepository;
        }

        public Role Save(Role role, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(role, "role");

            logger.IsCreate = role.IsNew();
            if (logger.IsCreate)
            {
                var domainRole = new DomainRole(role.ApplicationId, role.Code, role.Name)
                {
                    Desc = role.Desc,
                    CreatedBy = role.CreatedBy,
                    CreatedOn = role.CreatedOn,
                    UpdatedBy = role.UpdatedBy,
                    UpdatedOn = role.UpdatedOn
                };
                this.RoleRepository.Add(domainRole);
                role.Id = domainRole.Id;
            }
            else
            {
                var dbRole = this.RoleRepository.GetByKey(role.Id);
                dbRole.ChangeName(role.Name);
                dbRole.Desc = role.Desc;
                dbRole.UpdatedBy = role.UpdatedBy;
                dbRole.UpdatedOn = role.UpdatedOn;
                this.RoleRepository.Update(dbRole);
            }
            Context.Commit();
            LoggerService.Create(logger, role, "{0}角色", "角色{0}成功：ID【{1}】,角色名称【{2}】,角色代码【{3}】", role.Id, role.Name, role.Code);
            return role;
        }

        public Role GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            return DtoDomainMapper.ConvertToDto(this.RoleRepository.GetByKey(id));
        }

        public Role GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            return DtoDomainMapper.ConvertToDto(this.GetRoleGetCode(code));
        }

        public bool IsUniqueCode(string code)
        {
            Check.Argument.IsNotNull(code, "code");

            return !this.RoleRepository.Exists(new RoleCodeSpecification(code));
        }

        public IEnumerable<Permission> GetRolePermissions(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Enumerable.Empty<Permission>();
            }

            var role = this.GetRoleGetCode(code);
            var perArray = role.Permissions.ToArray();

            if (perArray.Length == 0)
            {
                return Enumerable.Empty<Permission>();
            }
            else
            {
                var pers = this.PermissionRepository.GetList(new PermissionCodeListSpecification(role.Permissions.ToArray()));
                return Array.ConvertAll(pers.ToArray(), item => DtoDomainMapper.ConvertToDto(item));
            }
        }

        private DomainRole GetRoleGetCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            return this.RoleRepository.Get(new RoleCodeSpecification(code));
        }

        public IEnumerable<Role> GetByApplicationId(string appId)
        {
            if (string.IsNullOrEmpty(appId))
            {
                return Enumerable.Empty<Role>();
            }

            var roles = this.RoleRepository.GetList(new RoleApplicationIdSpecification(appId));
            return Array.ConvertAll(roles.OrderBy(u => u.Name).ToArray(), item => DtoDomainMapper.ConvertToDto(item));
        }
    }
}

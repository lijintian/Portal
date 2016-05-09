using System;
using System.Collections.Generic;
using System.Linq;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Permission;
using Portal.Domain.Specification.ApiPermissionGroup;
using Portal.Dto;
using DomainApiPermissionGroup = Portal.Domain.Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup;
using EasyDDD.Core.Repository;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// API权限分组管理
    /// </summary>
    public class ApiPermissionGroupManagerService : ServiceBase, IApiPermissionGroupManagerService
    {
        #region 初始化
        private readonly IApiPermissionGroupRepository _repository;
        private readonly IPermissionRepository _permissionRepository;
        public ApiPermissionGroupManagerService(IRepositoryContext context, IApiPermissionGroupRepository roleRepository, IPermissionRepository perRepository)
            : base(context)
        {
            this._repository = roleRepository;
            this._permissionRepository = perRepository;
        }
        #endregion

        #region 获取
        public ApiPermissionGroup GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            return Map(this._repository.GetByKey(id));
        }

        public IEnumerable<ApiPermissionGroup> GetList(FindApiPermissionGroupRequest request)
        {
            return MapList(this._repository.GetList(ConvertToSpec(request)).OrderByDescending(u => u.CreatedOn));
        }
        /// <summary>
        /// 获取所有分组信息
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ApiPermissionGroup> GetList()
        {
            return MapList(this._repository.GetList().OrderBy(u => u.Code));
        }

        public IEnumerable<ApiPermissionGroup> GetList(string[] code)
        {
            var groups = _repository.GetList(new ApiPermissionGroupCodeListSpecification(code));
            return MapList(groups);
        }

        public ApiPermissionGroup GetByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            return Map(this.GetApiPermissionGroupGetCode(code));
        }

        public bool IsUniqueCode(string code)
        {
            Check.Argument.IsNotNull(code, "code");
            return !this._repository.Exists(new ApiPermissionGroupCodeSpecification(code));
        }

        public IEnumerable<Permission> GetApiPermissionGroupPermissions(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Enumerable.Empty<Permission>();
            }
            var role = this.GetApiPermissionGroupGetCode(code);
            var perArray = role.Permissions.ToArray();

            if (perArray.Length == 0)
            {
                return Enumerable.Empty<Permission>();
            }
            else
            {
                var pers = this._permissionRepository.GetList(new PermissionCodeListSpecification(role.Permissions.ToArray()));
                return Array.ConvertAll(pers.ToArray(), item => DtoDomainMapper.ConvertToDto(item));
            }
        }

        /// <summary>
        /// 获取对外开放的API权限分组
        /// </summary>
        /// <returns></returns>
        public List<ApiPermissionGroup> GetOpenedGroupList(DeveloperAppDto info)
        {
            if (info == null)
            {
                return null;
            }
            var list = this._repository.GetList(new ApiPermissionGroupExistsPermissionSpecification());
            var group = MapList(list);
            if (group == null && !group.Any())
            {
                return null;
            }
            var existaRequest = info.ExistsRequestGroup();
            var existsApproved = info.ExistsApprovedGroup();
            if (existaRequest || existsApproved)
            {
                foreach (var item in group)
                {
                    item.Approved = existsApproved && info.ApprovedGroupList.Contains(item.Code);
                    item.Checked = item.Approved || (existaRequest && info.RequestGroupList.Contains(item.Code));
                }
            }
            return group.ToList();
        }
        #endregion

        #region 操作
        public ApiPermissionGroup Save(ApiPermissionGroup group, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(group, "group");
            logger.IsCreate = group.IsNew();
            if (logger.IsCreate)
            {
                var domainApiPermissionGroup = new DomainApiPermissionGroup(group.Code, group.Name)
                {
                    Desc = group.Desc,
                    CreatedBy = group.CreatedBy,
                    CreatedOn = group.CreatedOn,
                    UpdatedBy = group.UpdatedBy,
                    UpdatedOn = group.UpdatedOn
                };
                this._repository.Add(domainApiPermissionGroup);
                group.Id = domainApiPermissionGroup.Id;
            }
            else
            {
                var dbApiPermissionGroup = this._repository.GetByKey(group.Id);
                dbApiPermissionGroup.ChangeName(group.Name);
                dbApiPermissionGroup.Desc = group.Desc;
                dbApiPermissionGroup.UpdatedBy = group.UpdatedBy;
                dbApiPermissionGroup.UpdatedOn = group.UpdatedOn;
                this._repository.Update(dbApiPermissionGroup);
            }
            Context.Commit();
            LoggerService.Create(logger, group, "{0}API权限分组", "API权限分组{0}成功：ID【{1}】,名称【{2}】,编码【{3}】", group.Id, group.Name, group.Code);
            return group;
        }
        #endregion

        #region 私有方法
        private DomainApiPermissionGroup GetApiPermissionGroupGetCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            return this._repository.Get(new ApiPermissionGroupCodeSpecification(code));
        }
        private ApiPermissionGroup Map(DomainApiPermissionGroup source)
        {
            return source == null ? null : DtoDomainMapper.ConvertToDto(source);
        }
        private IEnumerable<ApiPermissionGroup> MapList(IEnumerable<DomainApiPermissionGroup> source)
        {
            return Array.ConvertAll(source.ToArray(), item => Map(item));
        }
        private ISpecification<DomainApiPermissionGroup> ConvertToSpec(FindApiPermissionGroupRequest request)
        {
            var specs = new List<ISpecification<DomainApiPermissionGroup>>();
            if (!string.IsNullOrEmpty(request.Name))
            {
                specs.Add(new ApiPermissionGroupContainNameSpecification(request.Name));
            }
            if (!string.IsNullOrEmpty(request.Code))
            {
                specs.Add(new ApiPermissionGroupContainCodeSpecification(request.Code));
            }
            if (specs.Count > 0)
            {
                var result = specs[0];
                for (int i = 1; i < specs.Count; i++)
                {
                    result = result.And(specs[i]);
                }
                return result;
            }
            return null;
        }
        #endregion
    }
}

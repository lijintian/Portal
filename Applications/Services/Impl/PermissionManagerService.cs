using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Permission;
using Portal.Dto;
using Portal.Domain.Aggregates.PermissionAgg;
using Portal.Infrastructure.Exceptions;
using DtoPagePermission = Portal.Dto.PagePermission;
using DtoApiPermission = Portal.Dto.ApiPermission;
using DtoPermission = Portal.Dto.Permission;
using DtoFunctionPermission = Portal.Dto.FunctionPermission;
using DomainPermission = Portal.Domain.Aggregates.PermissionAgg.Permission;
using DomainPagePermission = Portal.Domain.Aggregates.PermissionAgg.PagePermission;
using DomainApiPermission = Portal.Domain.Aggregates.PermissionAgg.ApiPermission;
using EasyDDD.Infrastructure.Crosscutting.Paged;
using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting.Event;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 表示权限管理服务实现 
    /// </summary>
    public class PermissionManagerService : ServiceBase, IPermissionManagerService
    {
        #region 初始化
        private readonly IPermissionRepository _permissionRepository;
        private readonly IEventBus _eventBus;

        public PermissionManagerService(IRepositoryContext context, IPermissionRepository perRepository, IEventBus eventBus)
            : base(context)
        {
            this._permissionRepository = perRepository;
            this._eventBus = eventBus;
        }
        #endregion

        #region 属性
        private IApplicationRepository _applicationRepository { get { return IoC.Resolve<IApplicationRepository>(); } }
        //private IDeveloperAppManagerService _appService { get { return IoC.Resolve<IDeveloperAppManagerService>(); } }
        #endregion

        #region 获取
        public FindPermissionResponse<T> FindPermissions<T>(FindPermissionRequest reqeust) where T : ApplicationPermission
        {
            if (reqeust == null)
            {
                reqeust = new FindPermissionRequest();
            }

            var t = typeof(T);
            bool isPageType = t == typeof(DtoPagePermission);
            bool isApiType = t == typeof(DtoApiPermission);

            if (isPageType)
            {
                var order = new Dictionary<Expression<Func<DomainPagePermission, object>>, SortOrder>();
                order.Add(item => item.CreatedBy, SortOrder.Descending);

                var pagedResult = this._permissionRepository.FindPagePermissionsInPage(reqeust.PageIndex, reqeust.PageSize,
                    this.ConvertToPageSpecification(reqeust), order);

                var pagePerms = PageMapList<T>(pagedResult.Data.ToArray());
                this.SetApplicationName(pagePerms);
                return new FindPermissionResponse<T>(pagedResult.TotalRecords, pagedResult.TotalPages, pagePerms);
            }
            if (isApiType)
            {
                var order = new Dictionary<Expression<Func<DomainApiPermission, object>>, SortOrder>();
                order.Add(item => item.CreatedBy, SortOrder.Descending);

                var pagedResult = this._permissionRepository.FindApiPermissionsInPage(reqeust.PageIndex, reqeust.PageSize, this.ConvertToApiSpecification(reqeust), order);
                var apiPerms = ApiMapList<T>(pagedResult.Data.ToArray());
                this.SetApplicationName(apiPerms);

                return new FindPermissionResponse<T>(pagedResult.TotalRecords, pagedResult.TotalPages, apiPerms);
            }

            throw new PortalException(ErrorCodes.StringCodes.UnSupportPermissionType, ErrorMessage.UnSupportPermissionType);
        }

        public IEnumerable<T> GetList<T>(FindPermissionRequest reqeust) where T : ApplicationPermission
        {
            if (reqeust == null)
            {
                reqeust = new FindPermissionRequest();
            }
            var t = typeof(T);
            bool isPageType = t == typeof(DtoPagePermission);
            bool isApiType = t == typeof(DtoApiPermission);
            if (isPageType)
            {
                var list = this._permissionRepository.GetPagePermissionList(this.ConvertToPageSpecification(reqeust));
                return PageMapList<T>(list.OrderBy(u => u.Order).ToArray());
            }
            if (isApiType)
            {
                var list = this._permissionRepository.GetApiPermissionList(this.ConvertToApiSpecification(reqeust));
                return ApiMapList<T>(list.OrderBy(u => u.Order).ToArray());
            }
            return null;
        }

        public IEnumerable<DtoPagePermission> GetPagePermissionList(string applicationId)
        {
            var list = string.IsNullOrEmpty(applicationId) ? this._permissionRepository.GetPagePermissionList() : this._permissionRepository.GetPagePermissionList(new PagePermissionApplicationIdSpecification(applicationId));
            var pagePerms = PageMapList<DtoPagePermission>(list.ToArray());
            this.SetApplicationName(pagePerms);
            return pagePerms;
        }

        public IEnumerable<AppPermission> GetApiPermissionList(string[] code)
        {
            if (code == null || !code.Any())
            {
                return null;
            }
            var list = this._permissionRepository.GetApiPermissionList(new ApiPermissionContainCodeSpecification(code));
            var apiPerms = list.Select(u => new AppPermission() { Name = u.Name, Code = u.Code, Order = u.Order }).ToList();
            return apiPerms;
        }


        public T GetByCode<T>(string code) where T : DtoPermission
        {
            if (string.IsNullOrEmpty(code))
            {
                return default(T);
            }

            var t = typeof(T);
            if (t == typeof(DtoPagePermission))
            {
                var pagePer = this._permissionRepository.GetPagePermission(code);
                var funcPers = this._permissionRepository.GetFunctionPermission(pagePer.FunctionPermissions.ToArray());
                return (T)(object)DtoDomainMapper.ConvertToDto(pagePer, funcPers);
            }
            else if (t == typeof(DtoApiPermission))
            {
                return (T)(object)DtoDomainMapper.ConvertToDto(this._permissionRepository.GetApiPermission(code));
            }

            throw new PortalException(ErrorCodes.StringCodes.UnSupportPermissionType, ErrorMessage.UnSupportPermissionType);
        }


        /// <summary>
        /// 获取对外开放的权限
        /// </summary>
        /// <returns></returns>
        public List<AppPermission> GetOpenedPermissionList(DeveloperAppDto info)
        {
            if (info == null)
            {
                return null;
            }
            var list = this._permissionRepository.GetApiPermissionList(new ApiPermissionIsOpenedSpecification(true)).OrderBy(u => u.Code);
            var apiPerms = list.Select(u => new AppPermission() { Name = u.Name, Code = u.Code, Order = u.Order }).ToList();
            var existaRequest = info.ExistsRequestPermssion();
            var existsApproved = info.ExistsApprovedPermssion();
            if (existaRequest || existsApproved)
            {
                foreach (var item in apiPerms)
                {
                    item.Approved = existsApproved && info.ApprovedPermssions.Contains(item.Code);
                    item.Checked = item.Approved || (existaRequest && info.RequestPermssions.Contains(item.Code));
                }
            }
            return apiPerms.ToList();
        }
        #endregion

        #region 操作
        public DtoPagePermission Save(DtoPagePermission pagePer, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(pagePer, "pagePer");
            logger.IsCreate = pagePer.IsNew();
            if (logger.IsCreate)
            {
                var newPagePer = new DomainPagePermission(pagePer.ApplicationId, pagePer.Code, pagePer.Name)
                {
                    Tag = pagePer.Tag,
                    Desc = pagePer.Desc,
                    IsCompatible = pagePer.IsCompatible,
                    Order = pagePer.Order,
                    CreatedOn = pagePer.CreatedOn,
                    CreatedBy = pagePer.CreatedBy,
                    UpdatedOn = pagePer.UpdatedOn,
                    UpdatedBy = pagePer.UpdatedBy
                };
                newPagePer.AddFunctionPermissionRange(Array.ConvertAll(pagePer.FunctionPermissions.ToArray(), item => this.ConvertToFuncPermissionInfo(item)));
                this._permissionRepository.Add(newPagePer);
                pagePer.Id = newPagePer.Id;
                /* 
                if (newPagePer.IsCompatible)
                {
                    this._eventBus.Publish(new NewPermissionCreatedEvent(newPagePer.Id));
                    this._eventBus.Commit();
                }
                 */
            }
            else
            {
                var dbPagePer = this._permissionRepository.GetPagePermission(pagePer.Code);
                dbPagePer.Desc = pagePer.Desc;
                dbPagePer.IsCompatible = pagePer.IsCompatible;
                dbPagePer.Order = pagePer.Order;
                dbPagePer.Tag = pagePer.Tag;
                dbPagePer.SetName(dbPagePer.ApplicationId, pagePer.Name);
                dbPagePer.UpdatedOn = pagePer.UpdatedOn;
                dbPagePer.UpdatedBy = pagePer.UpdatedBy;
                dbPagePer.AddFunctionPermissionRange(Array.ConvertAll(pagePer.FunctionPermissions.ToArray(), item => this.ConvertToFuncPermissionInfo(item)));
                this._permissionRepository.Update(dbPagePer);
            }
            Context.Commit();
            LoggerService.Create(logger, pagePer, "{0}系统页面权限", "系统页面权限{0}成功：ID【{1}】,名称【{2}】,权限码【{3}】", pagePer.Id, pagePer.Name, pagePer.Code);
            return pagePer;
        }

        public DtoApiPermission Save(DtoApiPermission apiPer, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(apiPer, "apiPer");
            logger.IsCreate = apiPer.IsNew();
            if (logger.IsCreate)
            {
                var domainApiPer = new DomainApiPermission(apiPer.ApplicationId, apiPer.Code, apiPer.Name)
                {
                    Tag = apiPer.Tag,
                    Desc = apiPer.Desc,
                    IsOpened = apiPer.IsOpened,
                    IsCustomerGranted = apiPer.IsCustomerGranted,
                    IsCompatible = apiPer.IsCompatible,
                    Order = apiPer.Order,
                    CreatedOn = apiPer.CreatedOn,
                    CreatedBy = apiPer.CreatedBy,
                    UpdatedOn = apiPer.UpdatedOn,
                    UpdatedBy = apiPer.UpdatedBy
                };
                this._permissionRepository.Add(domainApiPer);
                apiPer.Id = domainApiPer.Id;
            }
            else
            {
                var dbApiPer = this._permissionRepository.GetApiPermission(apiPer.Code);
                dbApiPer.SetName(dbApiPer.ApplicationId, apiPer.Name, true);
                dbApiPer.Tag = apiPer.Tag;
                dbApiPer.Desc = apiPer.Desc;
                //dbApiPer.IsOpened = apiPer.IsOpened;
                //dbApiPer.IsCustomerGranted = apiPer.IsCustomerGranted;
                dbApiPer.IsCompatible = apiPer.IsCompatible;
                dbApiPer.Order = apiPer.Order;
                dbApiPer.UpdatedOn = apiPer.UpdatedOn;
                dbApiPer.UpdatedBy = apiPer.UpdatedBy;
                this._permissionRepository.Update(dbApiPer);
            }
            Context.Commit();
            LoggerService.Create(logger, apiPer, "{0}API权限", "API权限{0}成功：ID【{1}】,名称【{2}】,权限码【{3}】", apiPer.Id, apiPer.Name, apiPer.Code);
            return apiPer;
        }

        public bool IsUniqueCode(string perCode)
        {
            Check.Argument.IsNotNull(perCode, "perCode");

            return !this._permissionRepository.Exists(new PermissionCodeSpecification(perCode));
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 私有方法
        private ISpecification<DomainPagePermission> ConvertToPageSpecification(FindPermissionRequest request)
        {
            var specs = new List<ISpecification<DomainPagePermission>>();

            if (request.IsSetValueForCode())
            {
                specs.Add(new PagePermissionCodeSpecification(request.Code));
            }
            if (request.IsSetValueForApplication())
            {
                specs.Add(new PagePermissionApplicationIdSpecification(request.ApplicationId));
            }
            if (request.IsSetValueForName())
            {
                specs.Add(new PagePermissionNameSpecification(request.Name));
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

        private void SetApplicationName(IEnumerable<ApplicationPermission> appPers)
        {
            var apps = this._applicationRepository.GetList();
            foreach (var per in appPers)
            {
                if (string.IsNullOrEmpty(per.ApplicationId)) continue;
                per.ApplictionName = apps.First(item => item.Id == per.ApplicationId).Name;
            }
        }

        private ISpecification<DomainApiPermission> ConvertToApiSpecification(FindPermissionRequest request)
        {
            var specs = new List<ISpecification<DomainApiPermission>>();

            if (request.IsSetValueForCode())
            {
                specs.Add(new ApiPermissionCodeSpecification(request.Code));
            }
            if (request.IsSetValueForApplication())
            {
                specs.Add(new ApiPermissionApplicationIdSpecification(request.ApplicationId));
            }
            if (request.IsSetValueForName())
            {
                specs.Add(new ApiPermissionNameSpecification(request.Name));
            }
            if (request.IsSetValueForIsOpened())
            {
                specs.Add(new ApiPermissionIsOpenedSpecification(request.IsOpened.Value));
            }
            if (request.IsSetValueForIsCustomerGranted())
            {
                specs.Add(new ApiPermissionIsOpenedSpecification(request.IsCustomerGranted.Value));
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

        private FunctionPermissionInfo ConvertToFuncPermissionInfo(DtoFunctionPermission funcPer)
        {
            return new FunctionPermissionInfo()
            {
                Code = funcPer.Code,
                Name = funcPer.Name,
                Tag = funcPer.Tag,
                Order = funcPer.Order,
                IsCompatible = funcPer.IsCompatible,
                Desc = funcPer.Desc
            };
        }

        private IEnumerable<T> PageMapList<T>(IEnumerable<DomainPagePermission> list)
        {
            var pagePerms = Array.ConvertAll(list.ToArray(), item => (T)(object)PageMap(item));
            return pagePerms;
        }
        private DtoPagePermission PageMap(DomainPagePermission source)
        {
            return DtoDomainMapper.ConvertToDto(source, this._permissionRepository.GetFunctionPermission(source.FunctionPermissions.ToArray()));
        }

        private IEnumerable<T> ApiMapList<T>(IEnumerable<DomainApiPermission> list)
        {
            var pagePerms = Array.ConvertAll(list.ToArray(), item => (T)(object)ApiMap(item));
            return pagePerms;
        }
        private DtoApiPermission ApiMap(DomainApiPermission source)
        {
            return DtoDomainMapper.ConvertToDto(source);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.DeveloperApp;
using Portal.Dto;
using Portal.Domain.Aggregates.DeveloperAppAgg;
using Portal.Dto.Client;
using Portal.Infrastructure.Exceptions;
using Portal.Web.Core;
using DomainApiPermisson = Portal.Domain.Aggregates.PermissionAgg.ApiPermission;
using SortOrder = EasyDDD.Infrastructure.Crosscutting.Paged.SortOrder;
using EasyDDD.Core.Specification;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.InversionOfControl;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 表示开发者应用管理服务
    /// </summary>
    public class DeveloperAppManagerService : PortalService<DeveloperAppDto, FindDeveloperAppRequest, DeveloperApp>, IDeveloperAppManagerService
    {
        #region 初始化
        public DeveloperAppManagerService(IDeveloperAppRepository developerAppRepository, IRepositoryContext context)
            : base(context, developerAppRepository)
        {
        }
        #endregion

        #region 属性
        private IPermissionManagerService _permissionService { get { return IoC.Resolve<IPermissionManagerService>(); } }
        private IApiPermissionGroupManagerService _groupService { get { return IoC.Resolve<IApiPermissionGroupManagerService>(); } }
        #endregion

        #region 获取
        public FindDeveloperAppResponse Search(FindDeveloperAppRequest request)
        {
            Check.Argument.IsNotNull(request, "request");
            var order = new Dictionary<Expression<Func<DeveloperApp, object>>, SortOrder>();
            order.Add(item => item.CreatedOn, SortOrder.Descending);
            return Search<FindDeveloperAppResponse>(request, order);
        }
        /// <summary>
        /// 获取应用信息及待审、已审权限信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeveloperAppDto GetAppPermssionsGroupById(string id)
        {
            var dto = this.GetById(id);
            if (dto.IsExternal)
            {
                dto.RequestGroupDesc = dto.RequestGroupList == null ? string.Empty : GetGroupDesc(_groupService.GetList(dto.RequestGroupList.ToArray()));
                dto.ApprovedGroupDesc = dto.ApprovedGroupList == null ? string.Empty : GetGroupDesc(_groupService.GetList(dto.ApprovedGroupList.ToArray()));
            }
            else
            {
                dto.ApprovedPermssionDesc = GetPermissionDesc(_permissionService.GetApiPermissionList(dto.ApprovedPermssions.ToArray()));
            }
            return dto;
        }

        /// <summary>
        /// 获取新应用
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        public DeveloperAppDto GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id");
            }
            return GetModel(id);
        }

        /// <summary>
        /// 根据ClientId获取应用
        /// </summary>
        /// <returns></returns>
        public DeveloperAppDto GetByClientId(string clientId)
        {
            return this.Map(this._repository.Get(new DeveloperAppClientIdSpecification(clientId)));
        }

        /// <summary>
        /// 获取权限编码列表
        /// </summary>
        public string GetOwnedPermissionCode(DeveloperAppDto dto)
        {
            if (dto.ExistsRequestGroup())
            {
                IEnumerable<ApiPermissionGroup> list = _groupService.GetList(dto.RequestGroupList.ToArray());
                return GetGroupDesc(list);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取流程进度信息
        /// </summary>
        /// <returns></returns>
        public FlowInfo GetFlowInfo(DeveloperAppState state)
        {
            FlowInfo info = new FlowInfo() { StepId = 1, Data = new List<FlowDetailInfo>() };
            string format = "应用信息{0}";
            var enumList = EnumListUtility<SearchDeveloperAppState>.GetList;
            //string value = ((int)state).ToString();
            foreach (var item in enumList)
            {
                info.Data.Add(new FlowDetailInfo(item.Text, string.Format(format, item.Text)));
                //if (item.Value == value)
                //{
                //    info.StepId = Convert.ToInt16(item.Value);
                //}
            }
            info.StepId = (int)state;
            info.Json = info.Data.ToJson();
            return info;
        }
        #endregion

        #region 操作
        public void Save(DeveloperAppDto app, SysLoggerDto logger)
        {
            logger.IsCreate = app.IsNew();
            if (logger.IsCreate)
            {
                Create(app);
            }
            else
            {
                Update(app, true);
            }
            LoggerService.Create(logger, app, "{0}注册应用", "注册应用{0}成功：ID【{1}】,名称【{2}】", app.Id, app.Name);
        }

        /// <summary>
        /// 更新应用
        /// </summary>
        public void Update(DeveloperAppDto app, SysLoggerDto logger)
        {
            Update(app, false);
            LoggerService.Create(logger, app, "{0}注册应用", "注册应用{0}成功：ID【{1}】,名称【{2}】", app.Id, app.Name);
        }

        public void Delete(string id, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(id, "id");
            var domain = GetDomain(id);
            if (domain == null)
            {
                throw new ArgumentException("domain");
            }
            domain.Delete();
            this.Update(domain);
            this.Context.Commit();
            LoggerService.Create(SysLoggerType.Delete, logger, "{0}注册应用", "注册应用{0}成功：ID【{1}】", id);
        }


        /// <summary>
        /// 保存请求权限
        /// </summary>
        /// <param name="id">应用标识</param>
        /// <param name="codes">请求权限列表</param>
        public void SaveRequestPermssions(string id, string codes, SysLoggerDto logger)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id");
            }
            if (string.IsNullOrEmpty(codes))
            {
                throw new ArgumentException("codes");
            }
            string[] codeList = codes.Split(',');
            if (codeList == null || codeList.Length == 0)
            {
                throw new ArgumentException("codes");
            }
            var app = this._repository.GetByKey(id);
            app.CanSumit();
            app.AddGroupRequestPermissions(codes);
            this._repository.Update(app);
            this.Context.Commit();
            LoggerService.Create(SysLoggerType.Update, logger, "注册应用提交API审核权限", "注册应用提交API审核权限成功：ID【{1}】,API权限分组【{2}】", id, codes);
        }

        /// <summary>
        /// 提交请求权限
        /// </summary>
        /// <param name="id">应用标识</param>
        public void SumitToApprove(string id, SysLoggerDto logger)
        {
            this.DoAction(id, app => app.SubmitToApprove());
            LoggerService.Create(SysLoggerType.Update, logger, "注册应用提交审核", "注册应用提交审核成功：ID【{1}】", id);
        }

        public void Approve(DeveloperAppSubmissionContext request, SysLoggerDto logger)
        {
            this.DoAction(request.Id, app => app.Approve(new StatusHandleContext(request.Manipulator, request.Remark)));
            LoggerService.Create(SysLoggerType.Update, logger, "审核注册应用", "注册应用审核通过：ID【{1}】,原因【{2}】", request.Id, request.Remark);
        }

        public void Reject(DeveloperAppSubmissionContext request, SysLoggerDto logger)
        {
            this.DoAction(request.Id, app => app.Reject(new StatusHandleContext(request.Manipulator, request.Remark)));
            LoggerService.Create(SysLoggerType.Update, logger, "审核注册应用", "注册应用驳回审请：ID【{1}】,原因【{2}】", request.Id, request.Remark);
        }

        private void DoAction(string id, Action<DeveloperApp> action)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("id");
            }

            var app = this._repository.GetByKey(id);
            action(app);

            this._repository.Update(app);
            this.Context.Commit();
        }
        #endregion

        #region 抽象方法实现
        protected override ISpecification<DeveloperApp> ConvertToSpec(FindDeveloperAppRequest request)
        {
            var specs = new List<ISpecification<DeveloperApp>>();
            if (request.IsUnDeleted)
            {
                specs.Add(new DeveloperAppIsDeletedSpecification(false));
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                specs.Add(new DeveloperAppContainNameSpecification(request.Name));
            }
            if (!string.IsNullOrEmpty(request.UserId))
            {
                specs.Add(new DeveloperAppUserIdSpecification(request.UserId));
            }
            if (!string.IsNullOrEmpty(request.UserName))
            {
                specs.Add(new DeveloperAppContainUserIdSpecification(request.UserName));
            }
            if (request.IsExternal.HasValue)
            {
                specs.Add(new DeveloperAppIsExternalSpecification(request.IsExternal.Value));
            }
            if (request.Type > 0)
            {
                specs.Add(new DeveloperAppTypeSpecification(request.Type));
            }
            if (request.State > 0)
            {
                specs.Add(new DeveloperAppStateSpecification(request.State));
            }
            if (request.StartTime.HasValue)
            {
                specs.Add(new DeveloperAppCreateOnStartSpecification(request.StartTime.Value));
            }
            if (request.EndTime.HasValue)
            {
                specs.Add(new DeveloperAppCreateOnEndSpecification(request.EndTime.Value));
            }
            return GetSpecs(specs);
        }

        protected override DeveloperAppDto Map(DeveloperApp source)
        {
            if (source == null)
            {
                throw new PortalException(ErrorCodes.StringCodes.DeveloperApplicatinUnExists, ErrorMessage.DeveloperApplicatinUnExists);
            }
            return new DeveloperAppDto()
            {
                Id = source.Id,
                Name = source.Name,
                IsExternal = source.IsExternal,
                AccessUrl = source.AccessUrl,
                RequestGroupList = source.RequestGroupList == null ? null : source.RequestGroupList.ToList(),
                RequestPermssions = source.RequestList == null ? null : source.RequestList.Select(item => item.Code).ToList(),
                ApprovedGroupList = source.RequestGroupList == null ? null : source.ApprovedGroupList.ToList(),
                ApprovedPermssions = source.ApprovedList == null ? null : source.ApprovedList.Select(item => item.Code).ToList(),
                AppType = source.AppType.GetEnum<DeveloperAppType>(),
                CallbackUrl = source.CallbackUrl,
                CreatedOn = source.CreatedOn,
                Desc = source.Desc,
                Remarks = source.Remarks,
                State = source.State.GetEnum<DeveloperAppState>(),
                UserId = source.UserId,
                ClientId = source.ClientId,
                Secret = source.Secret
            };
        }
        #endregion

        #region 私有方法
        ///// <summary>
        ///// 获取权限编码
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //private string GetOwnedPermissionCode(string id)
        //{
        //    DeveloperAppDto dto = GetById(id);
        //    return GetOwnedPermissionCode(dto);
        //}
        /// <summary>
        /// 获取权限码信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetPermissionDesc(IEnumerable<AppPermission> list)
        {
            string format = "{0}【{1}】";
            return list == null ? "" : string.Join(",", list.Select(u => string.Format(format, u.Name, u.Code)).ToArray());
        }

        /// <summary>
        /// 获取权限码信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetGroupDesc(IEnumerable<ApiPermissionGroup> list)
        {
            string format = "{0}【{1}】";
            return list == null ? "" : string.Join(",", list.Select(u => string.Format(format, u.Name, u.Code)).ToArray());
        }

        /// <summary>
        /// 创建新应用
        /// </summary>
        /// <param name="app">应用传输对象</param>
        private void Create(DeveloperAppDto app)
        {
            if (app == null)
            {
                throw new ArgumentException("app");
            }
            var domainApp = new DeveloperApp(app.UserId, app.Name, app.CallbackUrl, app.IsExternal)
            {
                AppType = app.AppType.GetEnum<ApplicationType>(),
                Desc = app.Desc,
                AccessUrl = app.AccessUrl,
            };
            Create(domainApp);
            app.Id = domainApp.Id;
            this.Context.Commit();
        }

        /// <summary>
        /// 更新应用
        /// </summary>
        /// <param name="app">应用传输对象</param>
        /// <param name="checkState">检查状态是否在开发中</param>
        private void Update(DeveloperAppDto app, bool checkState)
        {
            if (app == null)
            {
                throw new ArgumentException("app");
            }
            var domain = GetDomain(app.Id);
            if (domain == null)
            {
                throw new ArgumentException("domain");
            }
            domain.Set(app.Name, app.CallbackUrl, checkState);
            domain.AppType = app.AppType.GetEnum<ApplicationType>();
            domain.Desc = app.Desc;
            domain.AccessUrl = app.AccessUrl;
            this.Update(domain);
            this.Context.Commit();
        }
        #endregion
    }
}

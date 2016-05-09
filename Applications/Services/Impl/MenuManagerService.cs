using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Menu;
using Portal.Dto;
using Portal.Infrastructure.Exceptions;
using DomainMenu = Portal.Domain.Aggregates.MenuAgg.Menu;
using EasyDDD.Core.Specification;
using EasyDDD.Core.Repository;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Services.Impl
{
    /// <summary>
    /// 表示菜单管理服务实现
    /// </summary>
    public class MenuManagerService : PortalService<Menu, FindMenuRequest, DomainMenu>, IMenuManagerService
    {
        #region 初始化
        public MenuManagerService(IRepositoryContext context, IMenuRepository repository)
            : base(context, repository)
        {
        }
        #endregion

        #region 方法
        public Menu GetById(string menuId)
        {
            if (string.IsNullOrEmpty(menuId))
            {
                return null;
            }
            var domain = this._repository.GetByKey(menuId);
            return domain == null ? null : DtoDomainMapper.ConvertToDto(domain);
        }

        /// <summary>
        /// 根据应用系统获取菜单
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public IEnumerable<Menu> GetList(string applicationId)
        {
            return GetList(new MenuApplicationIdSpecification(applicationId));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public Menu Save(Menu menu, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(menu, "menu");
            logger.IsCreate = menu.IsNew();
            if (logger.IsCreate)
            {
                var newMenu = new DomainMenu(menu.ApplicationId, menu.ParentId, menu.Url, menu.PermissionCode)
                {
                    Desc = menu.Desc,
                    Name = menu.Name,
                    Sequence = menu.Order,
                    Target = menu.Target,
                    IsShare = menu.IsShare,
                    CreatedOn = menu.CreatedOn,
                    CreatedBy = menu.CreatedBy,
                    UpdatedOn = menu.UpdatedOn,
                    UpdatedBy = menu.UpdatedBy
                };
                this._repository.Add(newMenu);
                menu.Id = newMenu.Id;
            }
            else
            {
                var dbMenu = this._repository.GetByKey(menu.Id);
                dbMenu.SetParentId(menu.ApplicationId, menu.ParentId);
                dbMenu.SetUrl(menu.ApplicationId, menu.Url);
                dbMenu.SetPermissionCode(menu.ApplicationId, menu.PermissionCode);
                dbMenu.Name = menu.Name;
                dbMenu.Desc = menu.Desc;
                dbMenu.Sequence = menu.Order;
                dbMenu.Target = menu.Target;
                dbMenu.IsShare = menu.IsShare;
                dbMenu.UpdatedOn = menu.UpdatedOn;
                dbMenu.UpdatedBy = menu.UpdatedBy;
                this._repository.Update(dbMenu);
            }
            Context.Commit();
            LoggerService.Create(logger, menu, "{0}菜单", "菜单{0}成功：ID【{1}】,名称【{2}】", menu.Id, menu.Name);
            return menu;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="logger"></param>
        public void Remove(string menuId, SysLoggerDto logger)
        {
            Check.Argument.IsNotNull(menuId, "menuId");
            //是否有子节点
            if (this._repository.Get(new MenuParentIdSpecification(menuId)) != null)
            {
                throw new PortalException(ErrorCodes.StringCodes.MenuHasChild, ErrorMessage.MenuHasChild.FormatWith(menuId));
            }
            this._repository.Remove(this._repository.GetByKey(menuId));
            Context.Commit();
            LoggerService.Create(SysLoggerType.Delete, logger, "{0}菜单", "菜单{0}成功：ID【{1}】", menuId);
        }

        ///// <summary>
        ///// 修复数据(删除顶级目录，将2级目录菜单设置为顶级并将应用系统ID设置为applicationId)
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="applicationId"></param>
        //public string Repair(string id, string applicationId)
        //{
        //    string message = string.Empty;
        //    if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(applicationId))
        //    {
        //        message = "缺少必填参数！";
        //        return message;
        //    }
        //    var menu = this._repository.GetByKey(id);
        //    if (menu == null)
        //    {
        //        message = string.Format("菜单Id不存在：{0}", id);
        //        return message;
        //    }
        //    if (SetApplicationId(id, applicationId, true))
        //    {
        //        this._repository.Remove(menu);
        //    }
        //    Context.Commit();
        //    return message;
        //}
        #endregion

        #region 07.抽象方法实现
        protected override ISpecification<DomainMenu> ConvertToSpec(FindMenuRequest request)
        {
            var specs = new List<ISpecification<DomainMenu>>();
            if (!string.IsNullOrEmpty(request.ApplicationId))
            {
                specs.Add(new MenuApplicationIdSpecification(request.ApplicationId));
            }
            if (request.PermissionCodeList != null && request.PermissionCodeList.Count > 0)
            {
                specs.Add(new MenuPermissionCodeNullSpecification().Or(new MenuIsShareSpecification(true)).Or(new MenuPermissionCodeListSpecification(request.PermissionCodeList)));
            }
            else if (request.IsShare.HasValue)
            {
                specs.Add(new MenuPermissionCodeNullSpecification().Or(new MenuIsShareSpecification(request.IsShare.Value)));
            }
            return GetSpecs(specs);
        }

        protected override Menu Map(DomainMenu source)
        {
            return DtoDomainMapper.ConvertToDto(source);
        }
        #endregion

        #region 私有方法
        ///// <summary>
        ///// 根据父节点ID递归设置子节点的applicationId
        ///// </summary>
        //private bool SetApplicationId(string parentId, string applicationId, bool isFirst)
        //{
        //    var list = GetDomainList(new MenuParentIdSpecification(parentId)).ToArray();
        //    if (list.Any())
        //    {
        //        foreach (var menu in list)
        //        {
        //            if (isFirst)
        //            {
        //                menu.SetParentId(string.Empty);
        //            }
        //            menu.SetApplicationId(applicationId, false);
        //            this._repository.Update(menu);
        //            SetApplicationId(menu.Id, applicationId, false);
        //        }
        //        return true;
        //    }
        //    return false;
        //}
        #endregion
    }
}

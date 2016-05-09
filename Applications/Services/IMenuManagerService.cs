using System.Collections.Generic;
using EasyDDD.Core.Specification;
using Portal.Dto;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示菜单管理服务
    /// </summary>
    public interface IMenuManagerService
    {
        /// <summary>
        /// 根据Id查询菜单信息
        /// </summary>
        /// <param name="menuId">菜单Id</param>
        Menu GetById(string menuId);
        /// <summary>
        /// 获取所有菜单列表
        /// </summary>
        IEnumerable<Menu> GetList();

        /// <summary>
        /// 根据应用系统获取菜单
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        IEnumerable<Menu> GetList(string applicationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<Menu> GetList(FindMenuRequest request);


        IEnumerable<Menu> GetList(ISpecification<Domain.Aggregates.MenuAgg.Menu> specs);

        /// <summary>
        /// 移除指定的菜单
        /// </summary>
        /// <param name="menuId">移除菜单Id</param>
        void Remove(string menuId, SysLoggerDto logger);

        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <param name="menu">菜单信息</param>
        /// <returns>保存后菜单</returns>
        Menu Save(Menu menu, SysLoggerDto logger);


        ///// <summary>
        ///// 修复数据(删除顶级目录，将2级目录菜单设置为顶级并将应用系统ID设置为applicationId)
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="applicationId"></param>
        //string Repair(string id, string applicationId);
    }
}

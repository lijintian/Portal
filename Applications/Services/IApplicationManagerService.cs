using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Portal.Dto;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示应用程序管理服务
    /// </summary>
    public interface IApplicationManagerService
    {
        /// <summary>
        /// 根据id获取应用程序信息
        /// </summary>
        /// <param name="appId">应用程序Id</param>
        /// <returns>目标应用程序信息</returns>
        Application GetById(string appId);

        /// <summary>
        /// 获取所有应用程序列表
        /// </summary>
        /// <returns>应用程序列表</returns>
        IEnumerable<Application> GetAll();

        /// <summary>
        /// 保存应用程序更改,
        /// </summary>
        /// <param name="app">应用程序信息</param>
        /// <returns>保存后的应用程序</returns>
        /// <exception cref="Portal.Infrastructure.Exceptions.PortalException">名称已存在</exception>
        Application Save(Application app, SysLoggerDto logger);
    }
}

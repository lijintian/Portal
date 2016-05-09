/*
 EasyDDD
系统名称：  Portal门户系统管理平台
模块名称：  应用层
创建日期：  2016-03-02

内容摘要：  表管理服务类
*/
using System.Collections.Generic;
using Portal.Dto;

namespace Portal.Applications.Services
{
    public interface ISysLoggerManagerService
    {
        /// <summary>
        /// 根据条件查找
        /// </summary>
        /// <param name="request">查找请求</param>
        /// <returns>查询响应</returns>
        FindSysLoggerResponse Search(FindSysLoggerRequest request);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SysLoggerDto GetModel(string id);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="info"></param>
        void Save(SysLoggerDto info);

        /// <summary>
        /// 添加日志
        /// </summary>
        void Create<T>(SysLoggerDto logger, T dto, string title, string content, params object[] args) where T : AggregateDto;

        /// <summary>
        /// 添加日志
        /// </summary>
        void Create(SysLoggerType type, SysLoggerDto logger, string title, string content, params object[] args);

        /// <summary>
        /// 删除
        /// </summary>
        void Delete(string id);

        /// <summary>
        /// 批量删除
        /// </summary>
        void DeleteMore(string ids);
    }
}

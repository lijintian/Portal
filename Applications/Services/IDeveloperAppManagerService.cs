using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Dto;
using Portal.Dto.Client;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 表示开发者应用管理服务
    /// </summary>
    public interface IDeveloperAppManagerService
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        FindDeveloperAppResponse Search(FindDeveloperAppRequest request);

        /// <summary>
        /// 获取应用
        /// </summary>
        DeveloperAppDto GetById(string id);

        /// <summary>
        /// 获取应用根据ClientId
        /// </summary>
        DeveloperAppDto GetByClientId(string clientId);

        /// <summary>
        /// 获取应用信息及待审、已审权限信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DeveloperAppDto GetAppPermssionsGroupById(string id);

        /// <summary>
        /// 获取权限编码
        /// </summary>
        /// <returns></returns>
        string GetOwnedPermissionCode(DeveloperAppDto dto);

        FlowInfo GetFlowInfo(DeveloperAppState state);

        /// <summary>
        /// 创建新应用
        /// </summary>
        void Save(DeveloperAppDto app, SysLoggerDto logger);

        /// <summary>
        /// 更新应用
        /// </summary>
        void Update(DeveloperAppDto app, SysLoggerDto logger);

        /// <summary>
        /// 删除新应用
        /// </summary>
        void Delete(string id, SysLoggerDto logger);

        /// <summary>
        /// 保存审请权限
        /// </summary>
        void SaveRequestPermssions(string id, string codes, SysLoggerDto logger);

        /// <summary>
        /// 提交审核 
        /// </summary>
        void SumitToApprove(string id, SysLoggerDto logger);

        /// <summary>
        /// 审核通过
        /// </summary>
        void Approve(DeveloperAppSubmissionContext request, SysLoggerDto logger);

        /// <summary>
        /// 驳回审请
        /// </summary>
        void Reject(DeveloperAppSubmissionContext request, SysLoggerDto logger);


    }
}

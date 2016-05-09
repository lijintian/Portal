using System;
using System.Collections.Generic;
using System.Linq;
using Portal.Domain.Specification.Permission;
using Portal.Dto;
using Portal.Applications.Helper;
using Portal.Applications.Services;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.Application;
using Portal.Dto.Request;
using Portal.Web.Core;
using Portal.Web.Core.Common;
using PermissionDto = Portal.Dto.PagePermission;
using EasyDDD.Infrastructure.Crosscutting.Helpers;

namespace Portal.Applications.Events.Handler
{
    public class ImportPermissionCheckEventHandler : BaseImportCheckEventHandler<ImportPermissionCheckEvent, ImportPermissionRequest, PermissionDto>
    {
        #region 字段
        private readonly IPermissionRepository permissionRepository;
        private readonly IPermissionManagerService permissionService;
        private readonly IApplicationRepository applicationRepository;
        #endregion

        #region 初始化
        public ImportPermissionCheckEventHandler(IPermissionRepository PermissionRepository, IPermissionManagerService PermissionService, IApplicationRepository ApplicationRepository)
        {
            Check.Argument.IsNotNull(PermissionRepository, "PermissionRepository");
            Check.Argument.IsNotNull(PermissionService, "PermissionService");
            Check.Argument.IsNotNull(ApplicationRepository, "ApplicationRepository");
            this.permissionRepository = PermissionRepository;
            this.permissionService = PermissionService;
            this.applicationRepository = ApplicationRepository;
        }
        #endregion

        #region 方法
        public override PagePermission GetModel(ImportPermissionRequest item, ImportPermissionCheckEvent domainEvent, List<string> errorList)
        {
            int funIndex = 1;
            PermissionDto dto = new PermissionDto()
            {
                ApplicationId = string.Empty,
                Name = item.Name,
                Code = item.Code,
                Order = 0,
                IsCompatible = item.IsCompatible == "是",
                Desc = item.Desc,
                CreatedBy = domainEvent.Logger.CreatedBy,
                CreatedOn = DateTime.Now
            };
            if (CheckHelper.CheckNotEmpty(item.ApplicationCode, "应用系统英文名称", errorList))
            {
                var domain = applicationRepository.Get(new ApplicationEnNameSpecification(item.ApplicationCode));
                if (domain == null)
                {
                    errorList.Add(string.Format("应用系统英文名称【{0}】不存在", item.ApplicationCode));
                }
                else
                {
                    dto.ApplicationId = domain.Id;
                }
            }
            CheckHelper.CheckNotEmpty(item.Name, "名称", errorList);
            CheckHelper.CheckByteLen(item.Name, 20, "名称", errorList);
            CheckCode(item.Code, errorList);
            var orderResult = CheckHelper.CheckInt(item.Order, "排序", errorList, true);
            if (orderResult.Status)
            {
                dto.Order = orderResult.Data;
                CheckHelper.CheckRange(orderResult.Data, "排序", errorList);
            }
            if (CheckUtility.IsEmpty(item.FunctionPermissions))
            {
                CheckUniqueCode(item.Code, null, errorList);
            }
            else
            {
                List<FunctionPermission> funcList = CheckFunctionPermissions(item.FunctionPermissions, errorList);
                if (LengthUtility.IsNullOrEmpty(funcList))
                {
                    CheckUniqueCode(item.Code, null, errorList);
                    errorList.Add("【功能权限】输入格式有误");
                }
                else
                {
                    funIndex = 1;
                    CheckUniqueCode(item.Code, funcList.Where(u => !CheckUtility.IsEmpty(u.Code)).Select(u => u.Code).ToArray(), errorList);
                    foreach (var funcItem in funcList)
                    {
                        CheckHelper.CheckNotEmpty(funcItem.Name, string.Format("第{0}个功能权限名称", funIndex), errorList);
                        CheckCode(funcItem.Code, errorList, funIndex);
                        dto.AddFuncPermission(funcItem);
                        funIndex++;
                    }
                    var codes = funcList.Where(u => !CheckUtility.IsEmpty(u.Code)).GroupBy(u => u.Code).Select(u => new { Code = u.Key, Count = u.Count() }).Where(u => u.Count > 1).Select(u => u.Code);
                    if (codes.Any())
                    {
                        errorList.Add(string.Format("权限码提交了多次：{0}", string.Join(",", codes)));
                    }
                }
            }
            CheckHelper.CheckByteLen(item.Desc, 200, "描述", errorList);
            return dto;
        }

        public override void Save(PagePermission dto, SysLoggerDto logger)
        {
            permissionService.Save(dto, logger);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 检查权限码是否为空、是否由字母数字下划线组成。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorList"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool CheckCode(string code, List<string> errorList, int index = 0)
        {
            string format = string.Format(index > 0 ? "第{0}个功能权限码" : "权限码", index);
            bool result = CheckHelper.CheckNotEmpty(code, format, errorList) && CheckHelper.CheckByteLen(code, 50, format, errorList); ;
            if (result)
            {
                result = CheckHelper.CheckNumLetter(code, format, errorList);
                //if (!permissionService.IsUniqueCode(code))
                //{
                //    errorList.Add(string.Format("权限码[{0}]已被使用", code));
                //    return false;
                //}
            }
            return result;
        }
        /// <summary>
        /// 检查权限码唯一性性
        /// </summary>
        /// <returns></returns>
        private bool CheckUniqueCode(string code, string[] codeList, List<string> errorList)
        {
            bool result = true;
            if (!CheckUtility.IsEmpty(code))
            {
                if (!permissionService.IsUniqueCode(code))
                {
                    errorList.Add(string.Format("权限码[{0}]已被使用", code));
                    result = false;
                }
            }
            if (!LengthUtility.IsNullOrEmpty(codeList))
            {
                var list = permissionRepository.GetList(new PermissionCodeListSpecification(codeList));
                if (list.Any())
                {
                    errorList.Add(string.Format("功能权限码【{0}】已被使用", string.Join(",", list.Select(u => u.Code))));
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// 检查功能权限
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorList"></param>
        private List<FunctionPermission> CheckFunctionPermissions(string code, List<string> errorList)
        {
            code = code.Replace("&", "").Replace("，", "").Replace(",", "").Replace("【", "[").Replace("】", "]");
            if (CheckUtility.IsEmpty(code))
            {
                return null;
            }
            var list = RegexUtility.GetMatchListByBracket(code);
            if (LengthUtility.IsNullOrEmpty(list))
            {
                errorList.Add("功能权限码填写格式有误");
                return null;
            }
            List<FunctionPermission> funcList = new List<FunctionPermission>();
            var count = list.Count / 2;
            for (int i = 0; i < count; i++)
            {
                funcList.Add(new FunctionPermission() { Name = list[i * 2], Code = list[i * 2 + 1], Order = i + 1 });
            }
            return funcList;
        }
        #endregion
    }
}

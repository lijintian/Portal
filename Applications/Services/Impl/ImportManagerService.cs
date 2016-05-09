using System.Data;
using System.Web;

using Portal.Applications.Events;
using Portal.Dto;
using Portal.Dto.Request;
using Portal.Dto.Request.Enum;
using Portal.Web.Core;
using Portal.Web.Core.Model;
using Portal.Web.Core.Model.File;
using EasyDDD.Core.Event;

namespace Portal.Applications.Services.Impl
{
    public class ImportManagerService
    {
        public static ReturnModel<string> Execute(HttpPostedFileBase file, TemplateType type, SysLoggerDto logger)
        {
            ReturnModel<string> result = new ReturnModel<string>();
            switch (type)
            {
                //批量新增用户角色
                case TemplateType.CreateUserRole:
                    var importResult = PostedFileManage.Import<ImportUserRoleRequest>(file);
                    result = ImportData(importResult, new ImportUserRoleCheckEvent(importResult.List, importResult.ErrorTable, true, logger));
                    break;
                //批量删除用户角色
                case TemplateType.DeleteUserRole:
                    var importResult2 = PostedFileManage.Import<ImportUserRoleRequest>(file);
                    result = ImportData(importResult2, new ImportUserRoleCheckEvent(importResult2.List, importResult2.ErrorTable, false, logger));
                    break;
                //批量新增用户权限
                case TemplateType.CreateUserPermission:
                    var importResult3 = PostedFileManage.Import<ImportUserPermissionRequest>(file);
                    result = ImportData(importResult3, new ImportUserPermissionCheckEvent(importResult3.List, importResult3.ErrorTable, true, logger));
                    break;
                //批量删除用户权限
                case TemplateType.DeleteUserPermission:
                    var importResult4 = PostedFileManage.Import<ImportUserPermissionRequest>(file);
                    result = ImportData(importResult4, new ImportUserPermissionCheckEvent(importResult4.List, importResult4.ErrorTable, false, logger));
                    break;

                //批量新增客户角色
                case TemplateType.CreateCustomerRole:
                    var customerResult = PostedFileManage.Import<ImportCustomerRoleRequest>(file);
                    result = ImportData(customerResult, new ImportCustomerRoleCheckEvent(customerResult.List, customerResult.ErrorTable, true, logger));
                    break;
                //批量删除客户角色
                case TemplateType.DeleteCustomerRole:
                    var customerResult2 = PostedFileManage.Import<ImportCustomerRoleRequest>(file);
                    result = ImportData(customerResult2, new ImportCustomerRoleCheckEvent(customerResult2.List, customerResult2.ErrorTable, false, logger));
                    break;
                //批量新增客户权限
                case TemplateType.CreateCustomerPermission:
                    var customerResult3 = PostedFileManage.Import<ImportCustomerPermissionRequest>(file);
                    result = ImportData(customerResult3, new ImportCustomerPermissionCheckEvent(customerResult3.List, customerResult3.ErrorTable, true, logger));
                    break;
                //批量删除客户权限
                case TemplateType.DeleteCustomerPermission:
                    var customerResult4 = PostedFileManage.Import<ImportCustomerPermissionRequest>(file);
                    result = ImportData(customerResult4, new ImportCustomerPermissionCheckEvent(customerResult4.List, customerResult4.ErrorTable, false, logger));
                    break;

                //批量导入角色
                case TemplateType.ImportRole:
                    var importResult5 = PostedFileManage.Import<ImportRoleRequest>(file);
                    result = ImportData(importResult5, new ImportRoleCheckEvent(importResult5.List, importResult5.ErrorTable, logger));
                    break;
                //批量新增角色权限
                case TemplateType.CreateRolePermission:
                    var importResult6 = PostedFileManage.Import<ImportRolePermissionRequest>(file);
                    result = ImportData(importResult6, new ImportRolePermissionCheckEvent(importResult6.List, importResult6.ErrorTable, true, logger));
                    break;
                //批量删除角色权限
                case TemplateType.DeleteRolePermission:
                    var importResult7 = PostedFileManage.Import<ImportRolePermissionRequest>(file);
                    result = ImportData(importResult7, new ImportRolePermissionCheckEvent(importResult7.List, importResult7.ErrorTable, false, logger));
                    break;
                //批量导入权限
                case TemplateType.ImportPermission:
                    var importResult8 = PostedFileManage.Import<ImportPermissionRequest>(file);
                    result = ImportData(importResult8, new ImportPermissionCheckEvent(importResult8.List, importResult8.ErrorTable, logger));
                    break;
                //批量导入菜单
                case TemplateType.ImportMenu:
                    var importResult9 = PostedFileManage.Import<ImportMenuRequest>(file);
                    result = ImportData(importResult9, new ImportMenuCheckEvent(importResult9.List, importResult9.ErrorTable, logger));
                    break;
                default:
                    result.ErrorMessage = "导入的文件类型错误！";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 批量调整用户权限（新增权限、删除权限）
        /// </summary>
        /// <returns></returns>
        private static ReturnModel<string> ImportData<TRequest, TEvent>(ImportFileResult<TRequest> request, TEvent events) where TEvent : BaseImportCheckEvent<TRequest>
        {
            if (!request.Status)
            {
                return new ReturnModel<string>(request.ErrorMessage, request.Status);
            }
            var result = new ReturnModel<string>();
            DomainEvent.Publish<TEvent, Events.Callbacks.ImportCheckEventResult>(events,
               (e) =>
               {
                   if (e != null)
                   {
                       if (e.Data == null || e.Data.Rows.Count <= 0)
                       {
                           result = new ReturnModel<string>(e.ErrorMessage, e.Status);
                       }
                       else
                       {
                           string path = ExcelUtility.Save(e.Data.DefaultView);
                           result = new ReturnModel<string>(path, e.ErrorMessage, e.Status);
                       }
                   }
               });
            return result;
        }

        /// <summary>
        /// 获取模板文件路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFilePath(TemplateType type)
        {
            string format = "~/Content/Templates/{0}模板.xlsx";
            switch (type)
            {
                case TemplateType.CreateUserRole:
                case TemplateType.DeleteUserRole:
                    format = "~/Content/Templates/批量导入用户角色模板.xls";
                    break;
                case TemplateType.CreateUserPermission:
                case TemplateType.DeleteUserPermission:
                    format = "~/Content/Templates/批量导入用户权限模板.xls";
                    break;
                case TemplateType.CreateRolePermission:
                case TemplateType.DeleteRolePermission:
                    format = "~/Content/Templates/批量导入角色权限模板.xls";
                    break;
                case TemplateType.CreateCustomerRole:
                case TemplateType.DeleteCustomerRole:
                    format = "~/Content/Templates/批量导入客户角色模板.xls";
                    break;
                case TemplateType.CreateCustomerPermission:
                case TemplateType.DeleteCustomerPermission:
                    format = "~/Content/Templates/批量导入客户权限模板.xls";
                    break;
                default:
                    format = string.Format(format, type.GetDescription());
                    break;
            }
            return FileExtent.GetMapPath2(format);
        }
    }
}

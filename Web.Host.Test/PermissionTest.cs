using System.Collections.Generic;
using Portal.Dto;
using Portal.Web.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Host.Test.Model;

namespace Web.Host.Test
{
    [TestClass]
    public class PermissionTest : BaseTest
    {
        #region 属性
        #endregion

        #region 初始化
        public PermissionTest()
            : base("Permission")
        { }
        #endregion

        #region 方法
        /// <summary>
        /// 系统页面权限管理页面
        /// </summary>
        [TestMethod]
        public void Index()
        {
            var result = GetContent(ExecuteType.Index);
        }

        /// <summary>
        /// API权限管理页面
        /// </summary>
        [TestMethod]
        public void Api()
        {
            var result = GetContent("Api");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Delete()
        {
            var parameter = new { Id = 1 };
            var result = Delete(parameter);
        }

        /// <summary>
        /// 添加系统页面权限信息
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Create()
        {
            PagePermission info = new PagePermission()
            {
                ApplicationId = "1",
                Name = "账号页面",
                Code = "FinanceAccount",
                IsCompatible = false,
                Order = 1,
                Desc = "",
            };
            List<FunctionPermission> list = new List<FunctionPermission>()
            {
                new FunctionPermission(){Name = "新增",Code = "PageCreate",Order = 1},
                new FunctionPermission(){Name = "修改",Code = "PageUpdate",Order = 2},
                new FunctionPermission(){Name = "删除",Code = "PageDelete",Order = 3},
            };
            info.FunctionPermissionSummary = list.ToJson();
            var result = Save(info);
        }

        /// <summary>
        /// 修改系统页面权限信息
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Update()
        {
            PagePermission info = new PagePermission()
            {
                Id = "6",
                ApplicationId = "2",
                Name = "账号测试页面",
                Code = "PageCodex",
                IsCompatible = true,
                Order = 1,
                Desc = "账号测试页面备注",
            };
            List<FunctionPermission> list = new List<FunctionPermission>()
            {
                new FunctionPermission(){Name = "新增",Code = "Page2Create",Order = 1},
                new FunctionPermission(){Name = "修改",Code = "Page2Update",Order = 2},
                new FunctionPermission(){Name = "删除",Code = "Page2Delete",Order = 3},
            };
            info.FunctionPermissionSummary = list.ToJson();
            var result = Save(info);
        }

        /// <summary>
        /// 添加API权限信息
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void ApiCreate()
        {
            ApiPermission info = new ApiPermission()
            {
                ApplicationId = "1",
                Name = "API账号",
                Code = "APIUserCreate",
                Order = 10,
                Desc = "API账号描述",
            };
            var result = Execute(info, "ApiSave");
        }
        /// <summary>
        /// 修改API权限信息
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void ApiUpdate()
        {
            ApiPermission info = new ApiPermission()
            {
                Id = "16",
                ApplicationId = "1",
                Name = "测试API",
                Code = "APITest",
                Order = 10,
                Desc = "APITest描述",
            };
            var result = Execute(info, "ApiSave");
        }
        #endregion
    }
}

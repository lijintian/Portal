using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Web.Admin.Controllers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using Portal.Dto;
using System.Web.Mvc;
using Portal.Web.Core.Model;
using Portal.Web.Admin.Model.Role;
using System.Collections.Generic;

namespace Admin
{
    [TestClass]
    public class RoleTest
    {
        public IUnityContainer container;
        public RoleController rc;

        public RoleTest()
        {
            container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(container, "testContainer");
            rc = container.Resolve<RoleController>();
        }

 
        [TestMethod]
        public void SaveRoleTest()
        {
            Role r = new Role()
            {
                Id = "1",
                Code = "roleCode1",
                ApplicationId = "1",
                Desc = "remark",
                Name = "角色1"

            };

            JsonResult jr = (JsonResult)rc.Save(r);

            #region Assert->JsonResult Data(ReturnModel<int>)
            ReturnModel<int> rtData = jr.Data as ReturnModel<int>;
            Assert.IsTrue(rtData.Status, "保存角色失败");
            #endregion
        }

        [TestMethod]
        public void SaveRolePermissionTest()
        {
            RolePermission r = new RolePermission()
            {
                 RoleId="1",
                 Permission="permission1,permission2"
            };
            JsonResult jr = (JsonResult)rc.SavePermission(r);

            #region Assert->JsonResult Data(ReturnModel<int>)
            ReturnModel<int> rtData = jr.Data as ReturnModel<int>;
            Assert.IsTrue(rtData.Status, "保存角色权限失败");
            #endregion
        }

        [TestMethod]
        public void  TestRoleListPartial()
        {
            Role parameter = new Role()
            {
                ApplicationId = "1"
            };
            PartialViewResult vr = (PartialViewResult)rc.ListPartial(parameter);
               

            #region Assert->ViewData["AllApplication"]
            int expectLeng = 2;
            List<Application> allApps = (List<Application>)vr.ViewData["AllApplication"];
            Assert.AreEqual(expectLeng, allApps.Count);
            #endregion

            #region Assert->Model(List<Role>)
            expectLeng = 2;
            List<Role> appRoles = vr.Model as List<Role>;
            Assert.IsNotNull(appRoles, "返回Model(List<Role>)为空"); ;
            #endregion
        }



    }
}

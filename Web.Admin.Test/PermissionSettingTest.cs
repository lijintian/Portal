using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Web.Admin.Controllers;
using System.Web.Mvc;
using Portal.Dto;
using Microsoft.Practices.Unity;
using Portal.Applications.Services;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Portal.Web.Admin.Model.PermissionSetting;
using Portal.Web.Core.Model;

namespace Admin
{
    [TestClass]
    public class PermissionSettingTest
    {

        public IUnityContainer container;
        public PermissionSettingController xpc;

        public PermissionSettingTest()
        {
            container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(container, "testContainer");
            xpc= container.Resolve<PermissionSettingController>();
        }

        [TestMethod]
        public void UserRoleSettingTest()
        {
            PartialViewResult vr = (PartialViewResult)xpc.AsynUserRoleSetting("2");

            #region Assert->ViewData["CurrentUser"]
            User expectUser = new User()
            {
                Id = "2",
                Desc = "testdesc",
                CreatedOn = new DateTime(2015,8,6),
                DisplayName = "testnickname",
                Email = "test@163.com",
                EmployeeNo = "1",
                IsApproved = true,
                LoginIp = string.Empty,
                LoginName = "aEmployee",
                Password = "test123",
                UserType = UserType.Employee
            };

            User actualyUser = (User)vr.ViewData["CurrentUser"];
            Assert.AreEqual(expectUser.Id, actualyUser.Id);
            #endregion

            #region Assert->ViewData["AllApplication"]
            int expectLeng = 2;

            List<Application> allApps = (List<Application>)vr.ViewData["AllApplication"];
            Assert.AreEqual(expectLeng, allApps.Count);
            #endregion

            #region Assert->ViewData["currentUserRoles"]
            List<Role> actuallyRoles = vr.ViewData["currentUserRoles"] as List<Role>;
            Assert.IsNotNull(actuallyRoles,"当前用户角色为空");
            #endregion

            #region Assert->Model（List<ApplicationRole>）
            expectLeng = 2;
            List<ApplicationRole> appRoles = vr.Model as List<ApplicationRole>;
            Assert.IsNotNull(appRoles, "返回Model(List<ApplicationRole>)为空"); ;
            #endregion
        }

        [TestMethod]
        public void UserPermissionSettingTest()
        {
            PartialViewResult vr = (PartialViewResult)xpc.AsynUserPermissionSetting("2");

            #region Assert->ViewData["CurrentUser"]
            User expectUser = new User()
            {
                Id = "2",
                Desc = "testdesc",
                CreatedOn = new DateTime(2015, 8, 6),
                DisplayName = "testnickname",
                Email = "test@163.com",
                EmployeeNo = "1",
                IsApproved = true,
                LoginIp = string.Empty,
                LoginName = "aEmployee",
                Password = "test123",
                UserType = UserType.Employee
            };

            User actualyUser = (User)vr.ViewData["CurrentUser"];
            Assert.AreEqual(expectUser.Id, actualyUser.Id);
            #endregion

            #region Assert->ViewData["AllApplication"]
            int expectLeng = 2;

            List<Application> allApps = (List<Application>)vr.ViewData["AllApplication"];
            Assert.AreEqual(expectLeng, allApps.Count);
            #endregion

            #region Assert->ViewData["curUserPermissions"]
            List<Permission> actuallyPermissions = vr.ViewData["currentUserRoles"] as List<Permission>;
            Assert.IsNotNull(actuallyPermissions, "当前用户权限为空");
            #endregion

            #region Assert->Model(List<ApplicationPagePermission>)
            expectLeng = 2;
            List<ApplicationPagePermission> appPagePermissions = vr.Model as List<ApplicationPagePermission>;
            Assert.IsNotNull(appPagePermissions, "返回Model(List<ApplicationPagePermission>)为空"); ;
            #endregion
        }

        [TestMethod]
        public void ApiUserPermissionSettingTest()
        {
            PartialViewResult vr = (PartialViewResult)xpc.AsynUserPermissionSetting("2");

            #region Assert->ViewData["CurrentUser"]
            User expectUser = new User()
            {
                Id = "2",
                Desc = "testdesc",
                CreatedOn = new DateTime(2015, 8, 6),
                DisplayName = "testnickname",
                Email = "test@163.com",
                EmployeeNo = "1",
                IsApproved = true,
                LoginIp = string.Empty,
                LoginName = "aEmployee",
                Password = "test123",
                UserType = UserType.Employee
            };

            User actualyUser = (User)vr.ViewData["CurrentUser"];
            Assert.AreEqual(expectUser.Id, actualyUser.Id);
            #endregion

            #region Assert->ViewData["AllApplication"]
            int expectLeng = 2;

            List<Application> allApps = (List<Application>)vr.ViewData["AllApplication"];
            Assert.AreEqual(expectLeng, allApps.Count);
            #endregion

            #region Assert->ViewData["curUserPermissions"]
            List<Permission> actuallyPermissions = vr.ViewData["currentUserRoles"] as List<Permission>;
            Assert.IsNotNull(actuallyPermissions, "当前用户权限为空");
            #endregion

            #region Assert->Model(List<ApplicationApiPermission>)
            expectLeng = 2;
            List<ApplicationApiPermission> appApiPermissions = vr.Model as List<ApplicationApiPermission>;
            Assert.IsNotNull(appApiPermissions, "返回Model(List<ApplicationApiPermission>)为空"); ;
            #endregion
        }

        [TestMethod]
        public void SaveUserRoleTest()
        {
            UserRole userRole = new UserRole() { 
              UserId="2",
              RoleCode="rolecode1,rolecode2"
            };

            JsonResult jr = (JsonResult)xpc.SaveUserRole(userRole);

            #region Assert->JsonResult Data(ReturnModel<int>)
            ReturnModel<int> rtData = jr.Data as ReturnModel<int>;
            Assert.IsTrue(rtData.Status,"保存用户角色失败");
            #endregion
        }

        [TestMethod]
        public void SaveUserPermissionTest()
        {
            UserPermission userP = new UserPermission()
            {
                UserId = "2",
                PermissionCode = "pagecode1,pagecode2"
            };

            //JsonResult jr = (JsonResult)xpc.SaveUserPermission(userP);

            #region Assert->JsonResult Data(ReturnModel<int>)
            //ReturnModel<int> rtData = jr.Data as ReturnModel<int>;
            //Assert.IsTrue(rtData.Status, "保存用户权限失败");
            #endregion
        }
    }
}

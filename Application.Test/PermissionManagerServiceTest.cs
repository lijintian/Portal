using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.Applications.Services;
using Portal.Applications.Services.Impl;
using Portal.Dto;

namespace Application.Test
{
    [TestClass]
    public class PermissionManagerServiceTest
    {
        private IPermissionManagerService Service
        {
            get;
            set;
        }

        public PermissionManagerServiceTest()
        {
            //this.Service = new MockPermissionManagerService();
        }
        
        [TestMethod]
        public void TestSavePagePermission()
        {
            var pagePer = new PagePermission()
            {
                ApplicationId = "1", 
                Code = "Order", Desc = "test", IsCompatible = false, Name = "订单页面", Order = 1, Tag = ""};
            
            pagePer.AddFuncPermission(new FunctionPermission(){ Code = "OrderCreate", Desc = "", IsCompatible = false, Order = 100, Tag = ""});

            var savedPagePer = this.Service.Save(pagePer,null);
            var savedFuncPer = savedPagePer.FunctionPermissions.First();

            Assert.IsFalse(string.IsNullOrEmpty(savedPagePer.Id));
            Assert.AreEqual(savedPagePer.Id, savedFuncPer.ParentId);
            Assert.IsFalse(string.IsNullOrEmpty(savedPagePer.Id));
        }

        [TestMethod]
        public void TestSaveApiPermission()
        {
            var apiPer = new ApiPermission()
            {
                ApplicationId = "1",
                Code = "api",
                Desc = "test",
                IsCompatible = false,
                Name = "forapi"
            };

            var savedApiPer = this.Service.Save(apiPer,null);
            Assert.IsFalse(string.IsNullOrWhiteSpace(savedApiPer.Id));
        }
    }
}

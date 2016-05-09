using System;
using System.Text;
using System.Collections.Generic;
using Portal.Applications.Services;
using Portal.Applications.Services.Impl;
using Portal.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DtoApplication = Portal.Dto.Application;

namespace Application.Test
{
    /// <summary>
    /// ApplicationServiceTest 的摘要说明
    /// </summary>
    [TestClass]
    public class ApplicationServiceTest
    {
        private IApplicationManagerService AppService
        {
            get;
            set;
        }
        public ApplicationServiceTest()
        {
            this.AppService = new ApplicationManagerService(null,null);
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestAddNewOne()
        {
           var newApp = this.AppService.Save(new DtoApplication(){
                CnName = "订单系统", 
                EnName = "OMS", 
                Desc = "物流订单系统",
                Remark = "tesst"
            },null);

            Assert.IsNotNull(newApp);
            Assert.IsFalse(string.IsNullOrEmpty(newApp.Id));
        }
    }
}

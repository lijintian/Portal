using Portal.Dto;
using Portal.Web.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Host.Test.Model;

namespace Web.Host.Test
{
    [TestClass]
    public class MenuTest : BaseTest
    {
        #region 属性
        #endregion

        #region 初始化
        public MenuTest()
            : base("Menu")
        { }
        #endregion

        #region 方法
        [TestMethod]
        public void Index()
        {
            var result = GetContent(ExecuteType.Index);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Delete()
        {
            var parameter = new { Id = 9 };
            var result = Delete(parameter);
        }

        /// <summary>
        /// 添加菜单信息
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Create()
        {
            Menu info = new Menu()
            {
                ParentId = "2",
                Name = "测试页面1",
                PermissionCode = "TestPage1",
                Url = "Manage/TestPage1",
                Order = 10,
                Desc = "测试页面备注"
            };
            var result = Save(info, ExecuteType.Save);
        }

        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Update()
        {
            Menu info = new Menu()
            {
                Id = "8",
                ParentId = "1",
                Name = "测试页面2",
                PermissionCode = "TestPage2",
                Url = "Manage/TestPage2",
                Order = 10,
                Desc = "测试页面2备注"
            };
            var result = Save(info, ExecuteType.Save);
        }

        public ReturnModel<int> Save(Menu info, ExecuteType type)
        {
            var result = GetResult<Menu, int>(info, type);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ErrorType == 1);
            return result;
        }
        #endregion
    }
}

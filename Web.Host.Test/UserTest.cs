using Portal.Dto;
using Portal.Web.Admin.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Host.Test.Model;

namespace Web.Host.Test
{
    [TestClass]
    public class UserTest : BaseTest
    {
        #region 属性
        #endregion

        #region 初始化
        public UserTest()
            : base("User")
        { }
        #endregion

        #region 方法
        /// <summary>
        /// 用户管理页面
        /// </summary>
        [TestMethod]
        public void Index()
        {
            var result = GetContent(ExecuteType.Index);
        }

        /// <summary>
        /// API帐号管理页面
        /// </summary>
        [TestMethod]
        public void Api()
        {
            var result = GetContent("Api");
        }

        /// <summary>
        /// 启用
        /// </summary>
        [TestMethod]
        public void Approve()
        {
            SysUserParameter parameter = new SysUserParameter() { LoginName = "aCustomer", State = 1 };
            var result = Delete(parameter);
        }
        /// <summary>
        /// 禁用
        /// </summary>
        [TestMethod]
        public void DisApprove()
        {
            SysUserParameter parameter = new SysUserParameter() { LoginName = "aCustomer" };
            var result = Delete(parameter);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        [TestMethod]
        public void Reset()
        {
            SysUserParameter parameter = new SysUserParameter() { LoginName = "aCustomer" };
            var result = Execute(parameter, "ResetPassword");
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Create()
        {
            User info = new User() { LoginName = "guest", UserType = UserType.Customer, Password = "111111", Email = "admin1@chukou1.com", Desc = "描述内容111" };
            //User info = new User() { LoginName = "admin1", Email = "admin1@chukou1.com", Desc = "描述内容111" };
            var result = Save(info);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void Update()
        {
            User info = new User() { Id = "55cbf6d546b9e41ecc54b2cd", Email = "apiTest@chukou1.com", Desc = "api帐号描述内容333" };
            var result = Save(info);
        }

        /// <summary>
        /// 检测用户名称是否重复
        /// </summary>
        [TestMethod]
        public void Check()
        {
            SysUserParameter parameter = new SysUserParameter() { LoginName = "aCustomerxxx" };
            var result = Execute(parameter, "Check");
        }
        #endregion
    }
}

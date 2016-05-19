using System;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using Portal.Dto.Request;
using Portal.Dto.Response;
using Portal.SDK.ServiceClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SDK.Test
{
    [TestClass]
    public class UserServiceTest
    {
        private string _employee = "aEmployee";
        private string _password = "111111";
        private string _customer = "linjie01";

        [TestMethod]
        public void TestGetUserByIdentity()
        {
            var customer = UserServiceClient.GetUser(_employee);
            AssertUserInfo(customer);

            var employee = UserServiceClient.GetUser(_customer);
            AssertUserInfo(employee);
        }

        private void AssertUserInfo(GetUserResponse response)
        {
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.LoginName);
            Assert.IsFalse(response.HasError);
        }

        [TestMethod]
        public void TestUpdate()
        {
            var updateDesc = "updatedesc";
            var updateDis = "updatenickname";
            var updateEmail = "123@abc.com";
            var request = new UpdateUserRequest()
            {
                Desc = updateDesc,
                DisplayName = updateDis,
                Email = updateEmail,
                ClientNo = "123",
            };

            var response = UserServiceClient.Update("123@abc.com", request);
            Assert.IsNotNull(response);
            Assert.IsFalse(response.HasError);

            var getUserRep = UserServiceClient.GetUser("123@abc.com");
            Assert.AreEqual(getUserRep.Desc, updateDesc);
            Assert.AreEqual(getUserRep.DisplayName, updateDis);
            Assert.AreEqual(getUserRep.Email, updateEmail);
        }

        [TestMethod]
        public void TestCreate()
        {
            var request = new CreateUserRequest()
            {
                Desc = "newDesc",
                DisplayName = "jjy_01@163.com",
                Email = "",
                ClientNo = "12333",
                EmployeeNo = "666",
                IsApproved = true,
                LoginName = "jjy_01@163.com",
                Password = "test123",
                IsCustomer = false
            };
            var rep = UserServiceClient.Create(request);
            Assert.IsNotNull(rep);
            Assert.IsFalse(rep.HasError);

            var getUserRep = UserServiceClient.GetUser("666");
            Assert.IsNotNull(getUserRep);
            Assert.AreEqual(getUserRep.LoginName, "jjy_8888@163.com");
        }

        [TestMethod]
        public void TestCreate2()
        {
            var request = new CreateUserRequest()
            {
                Desc = string.Empty,
                DisplayName = "abc@163.com",
                Email = "abc@163.com",
                EmployeeNo = string.Empty,
                IsApproved = false,
                LoginName = "abc@163.com",
                Password = "79218965EB72C92A549DD5A",
                IsCustomer = true
            };
            var rep = UserServiceClient.Create(request);
            Assert.IsNotNull(rep);
            Assert.IsFalse(rep.HasError);
        }

        [TestMethod]
        public void TestChangePasswordNeedProvideOldPassword()
        {
            var identity = "testfor0909";
            var rep = UserServiceClient.ChangePassword(identity,
                new ChangePasswordRequest() { OldPassword = "newPwd", NewPassword = "111111" });
            Assert.IsNotNull(rep);
            Assert.IsFalse(rep.HasError);

            var rep2 = LoginServiceClient.LoginIn(identity, "111111");
            Assert.IsTrue(rep2.IsSuccessed());
        }

        [TestMethod]
        public void TestApprove()
        {
            var rep = UserServiceClient.Approve("testfor0909");
            Assert.IsNotNull(rep);
            Assert.IsFalse(rep.HasError);
        }

        [TestMethod]
        public void TestDisApprove()
        {
            var rep = UserServiceClient.DisApprove("testfor0909");
            Assert.IsNotNull(rep);
            Assert.IsFalse(rep.HasError);
        }

        [TestMethod]
        public void TestChangePasswordJustProvideNewPassword()
        {
            var identity = "testfor0909";
            var newPassword = "newPasswordtesting";

            var rep = UserServiceClient.ChangePassword(identity, newPassword);
            Assert.IsFalse(rep.HasError);

            var rep2 = LoginServiceClient.LoginIn(identity, newPassword);
            Assert.IsTrue(rep2.IsSuccessed());
        }

        [TestMethod]
        public void TestResetPassword()
        {
            var identity = "testfor0909";
            //var newPassword = "test123";

            var rep = UserServiceClient.ResetPassword(identity);
            Assert.IsFalse(rep.HasError);

            //var rep2 = LoginServiceClient.LoginIn(identity, newPassword);
            //Assert.IsTrue(rep2.IsSuccessed());
        }

        [TestMethod]
        public void TestGetUsersUseEmployeeNo()
        {
            var response = UserServiceClient.GetUsers(new GetUsersRequest() { Employees = new string[] { "00392", "00234" }, Name = "" });
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Users.Length > 0);
        }

        [TestMethod]
        public void TestGetUserMenusUserAppId()
        {
            var appId = "55e015777ff79b13d0342e4d";
            var user = "employeeTest";
            var rep = UserServiceClient.GetUserMenus(user, appId);

            Assert.IsTrue(rep.Menus.Count() > 0);
        }

        [TestMethod]
        public void TestGetUserMenusEmptyAppId()
        {
            var appId = "";
            var user = "employeeTest";
            var rep = UserServiceClient.GetUserMenus(user, appId);

            Assert.IsTrue(rep.Menus.Count() > 0);
        }

        [TestMethod]
        public void TestGetUserMenusEmptyAppIdAndUser()
        {
            var appId = "";
            var user = "";
            var rep = UserServiceClient.GetUserMenus(user, appId);

            Assert.IsTrue(rep.HasError);
        }
    }
}
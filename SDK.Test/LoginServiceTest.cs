using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Portal.Dto.Response;
using Portal.SDK.ServiceClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Portal.SDK;

namespace SDK.Test
{
    /// <summary>
    /// LoginServiceTest 的摘要说明
    /// </summary>
    [TestClass]
    public class LoginServiceTest
    {
        private string _employee = "aEmployee";
        private string _password = "111111";
        private string _customer = "linjie01";

        public LoginServiceTest()
        {
        }

        [TestMethod]
        public void TestSuccessLoginIn()
        {
            var info = LoginServiceClient.LoginIn("", _password);

            var customer = LoginServiceClient.LoginIn(_customer, _password);
            Assert.IsNotNull(customer);
            Assert.IsTrue(customer.IsSuccessed());
            Assert.IsTrue(customer.IsCustomer);

            var customerRep = LoginServiceClient.LoginIn(_customer, _password);
            Assert.IsNotNull(customerRep);
            Assert.IsTrue(customerRep.IsSuccessed());
            Assert.IsTrue(customerRep.IsCustomer);

            var employeeRep = LoginServiceClient.LoginIn(_employee, _password);
            Assert.IsNotNull(employeeRep);
            Assert.IsTrue(employeeRep.IsSuccessed());
            Assert.IsTrue(employeeRep.IsEmployee);
        }

        [TestMethod]
        public void TestFailLoginIn()
        {
            var customerRep = LoginServiceClient.LoginIn(_customer, "notright");
            Assert.IsNotNull(customerRep);
            Assert.IsFalse(customerRep.IsSuccessed());
        }

        [TestMethod]
        public void TestLoginOut()
        {
            var rep = LoginServiceClient.LoginOut("aCustomer");
            Assert.IsNotNull(rep);
            Assert.IsFalse(rep.HasError);
        }

        [TestMethod]
        public void TestSuccessVerifyToken()
        {
            var customerRep = LoginServiceClient.LoginIn(_customer, _password);
            Assert.IsNotNull(customerRep);
            Assert.IsNotNull(customerRep.Token);

            VerifyTokenResponse verifyRep = LoginServiceClient.VerifyToken(customerRep.Token);
            Assert.IsNotNull(verifyRep);
            Assert.IsTrue(verifyRep.IsSuccessed);
        }



        [TestMethod]
        public void TestFailVerifyToken()
        {
            VerifyTokenResponse verifyRep = LoginServiceClient.VerifyToken("notexitstoken");
            Assert.IsNotNull(verifyRep);
            Assert.IsFalse(verifyRep.IsSuccessed);
        }

        [TestMethod]
        public void TestErrorPatamerLogin()
        {
            var response = LoginServiceClient.Login(null, null, null);
            Assert.IsTrue(response.HasError);

            response = LoginServiceClient.Login(null, "xxx", "fortest");
            Assert.IsTrue(response.HasError);

            response = LoginServiceClient.Login("xxx", null, "fortest");
            Assert.IsTrue(response.HasError);
        }

        [TestMethod]
        public void TestSuccessLogin()
        {
            var response = LoginServiceClient.Login(_customer, _password, "forTest");
            Assert.IsTrue(response.IsSuccessed());

            response = LoginServiceClient.Login(_employee, _password, "forTest");
            Assert.IsTrue(response.IsSuccessed());
        }

        [TestMethod]
        public void TestHashLogin()
        {
            _customer = "协会和005";
            _password = "79218965EB72C92A549DD5A";
            var response = LoginServiceClient.HashLogin(_customer, _password, "forTest");
            Assert.IsTrue(response.IsSuccessed());
        }

        [TestMethod]
        public void TestNormalLogout()
        {
            //Regex reg = new Regex("\\S+\\((\\d+)\\)");
            //String content = "木马(123)";
            //var mc = reg.Matches(content);
            //if (mc.Count > 0)
            //{
            //    string value = mc[0].Value;
            //}
            //var input = "bbb{aaa}答案:正确 题型:判断题{333}";
            //var match = Regex.Match(input, "(.*){.*}");
            ////var match = Regex.Match(input, @"(?<=([[{}]+)?)[^]{}]+\\{\\d+}\\");
            //string str=match.Groups[0].Value;
            //string str2 = match.Groups[1].Value;

            //string tempStr = "CCC[ABC][D123]{CD2F}[GFDD2]{WED1}[FDDW]";
            ////string pattern = @"(?<=[[{]+)[^]}]+";
            //string pattern = @"[(.*?)\]";
            ////取括号外的字符:  [^{}]+(?={|$)
            ////取括号内的字符:  (?<={)[^{}]+(?=}) 
            //var match = Regex.Matches(tempStr, pattern);
            //List<string> temp_list = match.Cast<Match>().Select(a => a.Value).ToList();
            //string str = match[0].Groups[0].Value;
            //string str2 = match[0].Groups[1].Value;

            //string tempStr = "ABCD123{CD2F}GFDD2{WED1}FDDW";
            //string pattern = @"(?<=([[{}]+)?)[^]{}]+";
            //List<string> temp_list = Regex.Matches(tempStr, pattern).Cast<Match>().Select(a => a.Value).ToList();

            //string tempStr = "111【222】3333[444]555【666】";
            //string pattern = @"(?<=([[【】]+)?)[^]【】]+";
            //List<string> temp_list = Regex.Matches(tempStr, pattern).Cast<Match>().Select(a => a.Value).ToList();

            string tempStr = "111[222】3333【444]555【666】aaa[asf]";
            //string pattern = @"(?<=([[\[\]]+)?)[^]\[\]]+";
            string pattern = @"(?<=([[(\[|【)(\]|】)]+)?)[^](\[|【)(\]|】)]+";
            List<string> temp_list = Regex.Matches(tempStr, pattern).Cast<Match>().Select(a => a.Value).ToList();
            return;
            var response = LoginServiceClient.Login(_customer, _password, "forTest");
            Assert.IsTrue(response.IsSuccessed());

            var rep = LoginServiceClient.LogOut(response.Token);
            Assert.IsNotNull(rep);
            Assert.IsFalse(rep.HasError);
        }

        [TestMethod]
        public void TestNullTokenLogout()
        {
            var rep = LoginServiceClient.LogOut(null);
            Assert.IsTrue(rep.HasError);
        }

        [TestMethod]
        public void TestNormalValidateToken()
        {
            var response = LoginServiceClient.Login(_customer, _password, "forTest");
            Assert.IsTrue(response.IsSuccessed());

            var result = LoginServiceClient.ValidateToken(response.Token);
            Assert.IsTrue(result.IsSuccessed);
        }

        [TestMethod]
        public void TestFailValidateToken()
        {
            var result = LoginServiceClient.ValidateToken("notexist");
            Assert.IsFalse(result.IsSuccessed);
        }
    }
}

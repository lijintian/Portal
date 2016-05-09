using System;
using System.Text;
using System.Collections.Generic;
using Portal.SDK.ServiceClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SDK.Test
{
    /// <summary>
    /// UnitTest1 的摘要说明
    /// </summary>
    [TestClass]
    public class OauthServiceTest
    {
        public OauthServiceTest()
        {
            
        }


        [TestMethod]
        public void TestGetAccessToken()
        {
           var result = OauthServiceClient.GetAccessToken("7f4a5861-93a0-4380-b3b3-fbeb72a5e2e4",
                "abbcb34d-c4a6-47b5-bf60-26994d7eb36d", "http://www.wish.com/back",
                "refresh_token", "", "YWMxZjFjYWQtZmMwMC00ZGM3LWE5NjktMzI3NzM2NjRmNjJl");
            Assert.IsNotNull(result.AccessToken);
        }

        [TestMethod]
        public void TestValidateToken()
        {
            var result = OauthServiceClient.ValidateToken("15c0d9f2-7527-402b-acfc-e2bde1755eaf", "Add_New_Order");
        }
    }
}

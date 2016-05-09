using Portal.Dto;
using Portal.SDK.ServiceClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SDK.Test
{
    [TestClass]
    public class LoggerServiceTest
    {
        [TestMethod]
        public void TestSearch()
        {
            FindSysLoggerRequest request = new FindSysLoggerRequest() { PageIndex = 1, PageSize = 10 };
            var result = LoggerServiceClient.Search(request);
            Assert.IsNotNull(result);

            request.KeyWord = "登陆Portal";
            result = LoggerServiceClient.Search(request);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestFailCreate()
        {
            var result = LoggerServiceClient.Create("", "xxxxxxx111", "serviceTest");
            Assert.IsFalse(result.HasError);
            result = LoggerServiceClient.Create("xxxxxxxxxx222", "", "serviceTest");
            Assert.IsFalse(result.HasError);
        }

        [TestMethod]
        public void TestSuccessCreate()
        {
            var result = LoggerServiceClient.Create("测试1", "cafdsadfsafdsafdsaf11111", "serviceTest");
            Assert.IsFalse(result.HasError);
            result = LoggerServiceClient.Create("测试2", "更新了鞢在脸sdfdsafdsaf222", "serviceTest222", SysLoggerType.Update);
            Assert.IsFalse(result.HasError);
            result = LoggerServiceClient.Create("测试3", "sagserwqrewqrewqrewqrewqetrrteyutiy333", "serviceTest222", SysLoggerLevel.Info, SysLoggerType.Other, SysLoggerRight.All);
            Assert.IsFalse(result.HasError);
        }


    }
}

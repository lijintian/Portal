using Portal.Web.Core;
using Portal.Web.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Web.Host.Test.Model
{
    public abstract class BaseTest
    {
        #region 属性
        private string PortalUrl = "http://portal-test.abc.cn/Admin";
        private string ControllerName = "";
        #endregion

        #region 初始化
        public BaseTest()
        {
        }
        public BaseTest(string controller)
        {
            this.ControllerName = controller;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual ReturnModel<int> Create<T>(T parameter) 
        {
            return Execute(parameter, ExecuteType.Create);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual ReturnModel<int> Update<T>(T parameter) 
        {
            return Execute(parameter, ExecuteType.Update);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual ReturnModel<int> Save<T>(T parameter) 
        {
            return Execute(parameter, ExecuteType.Save);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual ReturnModel<int> Delete<T>(T parameter) 
        {
            return Execute(parameter, ExecuteType.Delete);
        }
        #endregion

        #region 公用方法
        public ReturnModel<int> Execute<T>(T parameter, ExecuteType type, bool isTest = true)
        {
            return Execute(parameter, type.ToString(), isTest);
        }
        public ReturnModel<int> Execute<T>(T parameter, string type, bool isTest = true) 
        {
            var result = GetResult<T, int>(parameter, type);
            if (isTest)
            {
                Test(result);
            }
            return result;
        }
        public string Execute2<T>(T parameter, ExecuteType type) 
        {
            return Execute2(parameter, type.ToString());
        }
        public string Execute2<T>(T parameter, string type) 
        {
            var result = GetContent(parameter, type);
            Test(result);
            return result;
        }
        public ReturnModel<T2> GetResult<T, T2>(T parameter, ExecuteType type) 
        {
            return GetResult<T, T2>(parameter, type.ToString());
        }
        public ReturnModel<T2> GetResult<T, T2>(T parameter, string type) 
        {
            var info = GetContent(parameter, type);
            return string.IsNullOrEmpty(info) ? null : info.FromJson<ReturnModel<T2>>();
        }
        public string GetContent<T>(T parameter, ExecuteType type) 
        {
            return GetContent(parameter, type.ToString());
        }
        public string GetContent<T>(T parameter, string type) 
        {
            string url = string.Format("{0}/{1}/{2}", PortalUrl, ControllerName, type);
            return HttpHelper.GetContent(url, parameter, 3);
        }
        public string GetContent(ExecuteType type, bool isTest = true)
        {
            return GetContent(type.ToString(), isTest);
        }
        public string GetContent(string type, bool isTest = true)
        {
            string url = string.Format("{0}/{1}/{2}", PortalUrl, ControllerName, type);
            var result = HttpHelper.GetContent(url);
            if (isTest)
            {
                Test(result);
            }
            return result;
        }
        public void Test(ReturnModel<int> result)
        {
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data == 1);
        }
        public void Test<T>(ReturnModel<T> result)
        {
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Status);
        }
        public void Test(string result)
        {
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
        #endregion
    }
}

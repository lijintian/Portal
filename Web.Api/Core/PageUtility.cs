using System;
using System.Configuration;
using System.Net.Http;
using Portal.Dto;
using Portal.Web.Api.Extends;

namespace Portal.Web.Api.Core
{
    public static class PageUtility
    {
        #region 字段
        public static string DecryptKey = "CK1@PJ@CRYKEY";
        public static string ApplicationParamKey = "ApplicationSource";
        //public static string ApplicationNameKey = "ApplicationName";
        //public static string ApplicationNameValue = "portal-api";
        #endregion

        /// <summary>
        /// 初始化日志
        /// </summary>
        public static SysLoggerDto GetLogger(this HttpRequestMessage request)
        {
            return GetLogger(request, "api");
        }

        /// <summary>
        /// 初始化日志
        /// </summary>
        public static SysLoggerDto GetLogger(this HttpRequestMessage request, string identity)
        {
            SysLoggerDto logger = new SysLoggerDto
            {
                ApplicationName = RequestHelper.Query(ApplicationParamKey),
                CreatedBy = identity,
            };
            InitInfo(request, logger);
            try
            {
                logger.ApplicationName = string.IsNullOrEmpty(logger.ApplicationName) ? Environment.MachineName : CryptUtility.Decrypt(DecryptKey, logger.ApplicationName);
            }
            catch (Exception)
            {
                logger.ApplicationName = string.Empty;
            }
            //if (string.IsNullOrEmpty(logger.ApplicationName))
            //{
            //    logger.ApplicationName = Get(ApplicationNameKey, ApplicationNameValue);
            //}
            if (string.IsNullOrEmpty(logger.CreatedBy))
            {
                logger.CreatedBy = identity;
            }
            return logger;
        }

        #region 私有方法
        /// <summary>
        /// 初始化日志
        /// </summary>
        /// <param name="item">实体</param>
        public static void InitInfo(this HttpRequestMessage request, SysLoggerDto item)
        {
            item.Ip = request.GetClientIpAddress();
            //设访问页面
            if (request != null && request.RequestUri != null && string.IsNullOrEmpty(item.Url))
            {
                //设访问页面
                item.Url = request.RequestUri.ToString();
            }
        }
        /// <summary>
        /// <para>获取配置文件中当前键值对应的值，并转换为相应的类型</para>
        /// <para>当配置项为空，返回传入的默认值</para>
        /// </summary>
        /// <typeparam name="T">想要转换的类型</typeparam>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>配置项值</returns>
        public static T Get<T>(string key, T defaultValue)
        {
            var v = ConfigurationManager.AppSettings[key];
            return String.IsNullOrEmpty(v) ? defaultValue : (T)Convert.ChangeType(v, typeof(T));
        }
        #endregion
    }
}

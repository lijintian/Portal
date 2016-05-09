/*
版权所有：  tangss版权所有(C)
系统名称：  文件处理层
模块名称：  获取文件扩展名及检测
创建日期：  2012-9-27

内容摘要： 
*/
using System;
using System.Globalization;
using System.IO;
using System.Web;

namespace Portal.Web.Core
{
    /// <summary>
    /// 文件扩展类
    /// </summary>
    public class FileExtent
    {
        #region 01.字段
        protected static string AllowExt = "7z|aiff|asf|avi|bmp|csv|doc|docx|fla|flv|gif|gz|gzip|jpeg|jpg|mid|mov|mp3|mp4|mpc|mpeg|mpg|ods|odt|pdf|png|ppt|pptx|pxd|qt|ram|rar|rm|rmi|rmvb|rtf|sdc|sitd|swf|sxc|sxw|tar|tgz|tif|tiff|txt|vsd|wav|wma|wmv|xls|xlsx|xml|zip";//支持的文件格式 
        protected static string AllowImgExt = "gif|jpg|jpeg|png|bmp";
        #endregion

        #region 02.检测扩展名的有效性 bool CheckValidExt(string sExt)

        /// <summary> 
        /// 检测扩展名的有效性 
        /// </summary>
        /// <param name="allext">所有扩展名</param>
        /// <param name="strExt">文件名扩展名</param> 
        /// <returns>如果扩展名有效,返回true,否则返回false.</returns> 
        public static bool CheckValidExt(string allext, string strExt)
        {
            if (string.IsNullOrEmpty(allext)) return true;
            if (string.Equals(allext, "image", StringComparison.CurrentCultureIgnoreCase)) return CheckImg(strExt);
            return allext.IndexOf(strExt, StringComparison.CurrentCultureIgnoreCase) > 0;
        }
        #endregion

        #region 03.检查是否为图片格式
        /// <summary>
        /// 检查是否为图片格式
        /// </summary>
        /// <param name="strExt"></param>
        /// <returns></returns>
        public static bool CheckImg(string strExt)
        {
            return CheckValidExt(AllowImgExt, strExt);
        }
        #endregion

        #region 04.检测扩展名的有效性
        /// <summary> 
        /// 检测扩展名的有效性 
        /// </summary> 
        /// <param name="sExt">文件名扩展名</param>
        /// <returns>如果扩展名有效,返回true,否则返回false.</returns> 
        public static bool CheckValidExt(string strExt)
        {
            return CheckValidExt(AllowExt, strExt);
        }
        #endregion

        #region 05.获取文件URL中的扩展名
        /// <summary>
        /// 获取文件URL中的扩展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileExt(string path)
        {
            var startPos = path.LastIndexOf(".");
            if (startPos >= 0)
            {
                return path.Substring(startPos).ToLower();
            }
            return string.Empty;
        }
        #endregion

        internal static string FileName
        {
            get { return Guid.NewGuid().ToString("N"); }
        }
        /// <summary>
        /// 生成年月日时分秒文件名
        /// </summary>
        /// <returns></returns>
        public static string GetFileName(string fileExt)
        {
            return string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss", DateTimeFormatInfo.InvariantInfo), Guid.NewGuid().ToString("N"), fileExt);
        }

        /// <summary>
        /// 获取生成的Excel保存位置
        /// </summary>
        /// <returns></returns>
        public static string GetExcelExportPath
        {
            get
            {
                return string.Format("{0}\\{1}.xls", HostUtility.ExcelExportPath, Guid.NewGuid().ToString("N"));
            }
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        /// <summary>
        /// 根据虚似路径获取绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <param name="createDirectory">是否创建目录</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath, bool createDirectory = true)
        {
            string vpath = GetMapPath2(strPath);
            if (createDirectory)
            {
                CreateDirectory(vpath);
            }
            return vpath;
        }

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath2(string strPath)
        {
            if (strPath.StartsWith("~/")) strPath = RegexUtility.RegexReplace(strPath, "^~/", "");
            strPath = strPath.Replace("/", "\\");
            if (strPath.StartsWith("\\"))
            {
                strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
            }
            return Path.Combine(HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~") : AppDomain.CurrentDomain.BaseDirectory, strPath);
        }
        #endregion
    }
}

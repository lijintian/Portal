using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Portal.Dto.Request;
using Portal.Web.Core.Model.File;

namespace Portal.Web.Core
{
    public class PostedFileManage
    {
        public static ImportFileResult<T> Import<T>(HttpPostedFileBase postFile) where T : new()
        {
            var result = new ImportFileResult<T>();
            try
            {
                UploadFileInfo file = Save(postFile);
                if (file == null)
                {
                    return new ImportFileResult<T>("请上传附件！", false);
                }
                var table = ExportHelper.ExcelToDataTable(file.SaveName, null, true);
                if (table == null || table.Rows.Count <= 0)
                {
                    return new ImportFileResult<T>("上传模板记录为空，导入失败!", false);
                }
                Dictionary<string, string> fields = ConvertHelper<T>.GetFields();
                string columnName = BaseImportRequest.GetItemName();
                string[] heads = fields.Where(u => !u.Value.Equals(columnName, StringComparison.CurrentCultureIgnoreCase)).Select(u => u.Value).ToArray();
                var unExistsList = CheckColumns(table, heads);
                if (unExistsList.Any())
                {
                    return new ImportFileResult<T>(string.Format("模板匹配不成功，不存在列：{0}，请重新按照模板的标准来上传！", string.Join("、", unExistsList)), false);
                }
                List<T> list = ConvertHelper<T>.ConvertToList(table, fields);
                if (list == null || list.IsNullOrEmpty())
                {
                    return new ImportFileResult<T>("上传模板记录为空，导入失败!", false);
                }
                result.InitInfo(list, CreateErrorTable(heads));
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.ErrorMessage = string.Format("上传文件格式有误，请下载模版文件！错误信息：{0}", ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 获取上传附件信息
        /// </summary>
        /// <param name="postFile"></param>
        /// <returns></returns>
        public static UploadFileInfo Save(HttpPostedFileBase postFile)
        {
            var info = new UploadFileInfo { Name = Path.GetFileName(postFile.FileName) };
            //文件扩展名 
            string fileExt = FileExtent.GetFileExt(info.Name);
            #region 生成存放路径并保存文件
            string srcName = FileExtent.GetFileName(fileExt);
            string filePath = string.Format("{0}\\Excel\\", HostUtility.SaveRoot);
            FileExtent.CreateDirectory(filePath);
            info.SaveName = string.Format("{0}{1}", filePath, srcName);
            postFile.SaveAs(info.SaveName);
            #endregion
            return info;
        }

        /// <summary>
        /// 检查列是否全存在
        /// </summary>
        /// <returns></returns>
        private static List<string> CheckColumns(DataTable dt, string[] heads)
        {
            return heads.Where(item => !dt.Columns.Contains(item)).ToList();
        }

        /// <summary>
        /// 创建错误Table
        /// </summary>
        /// <param name="heads"></param>
        private static DataTable CreateErrorTable(string[] heads)
        {
            DataTable dt = new DataTable();
            foreach (var item in heads)
            {
                dt.Columns.Add(item);
            }
            dt.Columns.Add("错误", typeof(string));
            return dt;
        }
    }
}
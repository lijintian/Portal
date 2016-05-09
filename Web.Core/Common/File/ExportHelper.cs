/*
系统名称：  文件处理层
模块名称：  文件下载
创建日期：  2012-9-27

内容摘要： 
*/

using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using Portal.Web.Core.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WebGrease.Activities;

namespace Portal.Web.Core
{
    public class ExportHelper
    {
        #region 01.Word文件下载
        /// <summary>
        /// Word文件下载
        /// </summary>
        /// <param name="filepath">下载来源地址：d://</param>
        /// <param name="filename">下载文名，如：table.xls</param>
        public static void Word(string filepath, string filename)
        {
            // 指定返回的是个不能被客户端读取的流，必须被下载 
            Export(filepath, "application/msword", filename);
        }
        #endregion

        #region 02.Excel文件下载
        /// <summary>
        /// Excel文件下载
        /// </summary>
        /// <param name="filepath">下载来源地址：d://</param>
        /// <param name="filename">下载文名，如：table.xls</param>
        public static void Excel(string filepath, string filename)
        {
            // 指定返回的是个不能被客户端读取的流，必须被下载 
            Export(filepath, "application/ms-excel", filename);
        }
        #endregion

        #region 03.文件名编码及下载
        /// <summary>
        /// 下载文件方法(下载文件重命名)
        /// </summary>
        /// <param name="filepath">下载来源地址：d://</param>
        /// <param name="filename">下载文名，如：table.xls</param>
        public static bool Export(string filepath, string filename)
        {
            int extIndex = filename.LastIndexOf(".");
            string fileext = extIndex > 0 && extIndex < filename.Length ? filename.Substring(extIndex) : "";
            string typename = Model.FileTypes.GetContentType(fileext);
            return Export(filepath, typename, filename);
        }
        /// <summary>
        /// 文件名编码
        /// </summary>
        /// <param name="filepath">路径</param>
        /// <param name="contentType">数据流类型</param>
        /// <param name="fileName">下载之后的文件名</param>
        public static bool Export(string filepath, string contentType, string fileName)
        {
            if (string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(contentType) || File.Exists(filepath) == false)
            {
                return false;
            }
            var file = new FileInfo(filepath);
            var response = CreateResponse(fileName, contentType);
            HttpUtility.UrlEncodeToBytes(fileName, Encoding.UTF8);
            // 添加头信息，指定文档大小，让浏览器能够显示下载进度 
            response.AddHeader("Content-Length", file.Length.ToString());
            // 指定返回的是个不能被客户端读取的流，必须被下载 
            response.ContentType = contentType + "; charset=UTF-8";
            // 把文档流发送到客户端 
            response.WriteFile(file.FullName);
            response.Flush();
            // 停止页面的执行 
            // response.End();
            return true;
        }

        /// <summary>
        /// 下载生成Excel   
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="contentType">数据流类型</param>
        /// <param name="fileName">文件名</param>
        public void Excel(MemoryStream stream, string contentType, string fileName)
        {
            if (string.IsNullOrEmpty(contentType) || File.Exists(fileName) == false)
            {
                return;
            }
            var response = CreateResponse(fileName, contentType);
            try
            {
                var buffer = stream.ToArray();
                response.BinaryWrite(buffer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                stream.Dispose();
            }
            response.End();
        }
        #endregion

        #region 04.将excel中的数据导入到DataTable中
        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string filePath, string sheetName, bool isFirstRowColumn)
        {
            IWorkbook workbook = null;
            FileStream fs = null;
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //文件扩展名 
            string fileExt = FileExtent.GetFileExt(filePath);
            if (fileExt.Equals(".xlsx",StringComparison.CurrentCultureIgnoreCase)) // 2007版本
            {
                workbook = new XSSFWorkbook(fs);
            }
            else if (fileExt.Equals(".xls", StringComparison.CurrentCultureIgnoreCase)) // 2003版本
            {
                workbook = new HSSFWorkbook(fs);
            }
            if (sheetName != null)
            {
                sheet = workbook.GetSheet(sheetName);
                if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                {
                    sheet = workbook.GetSheetAt(0);
                }
            }
            else
            {
                sheet = workbook.GetSheetAt(0);
            }
            if (sheet != null)
            {
                IRow firstRow = sheet.GetRow(0);
                if (firstRow == null)
                {
                    return data;
                }
                int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数
                if (isFirstRowColumn)
                {
                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        ICell cell = firstRow.GetCell(i);
                        if (cell != null)
                        {
                            string cellValue = ReadCell(cell, workbook);
                            if (cellValue != null)
                            {
                                DataColumn column = new DataColumn(cellValue);
                                data.Columns.Add(column);
                            }
                        }
                    }
                    startRow = sheet.FirstRowNum + 1;
                }
                else
                {
                    startRow = sheet.FirstRowNum;
                }
                //最后一列的标号
                int rowCount = sheet.LastRowNum;
                for (int i = startRow; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; //没有数据的行默认是null　　　　　　　
                    DataRow dataRow = data.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    data.Rows.Add(dataRow);
                }
            }
            return data;
        }
        #endregion

        #region 05.私有方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType">数据流类型</param>
        /// <param name="fileName">下载之后的文件名</param>
        /// <returns></returns>
        private static HttpResponse CreateResponse(string fileName, string contentType)
        {
            string filename = fileName;
            var response = HttpContext.Current.Response;
            response.Clear();
            response.Charset = "GB2312";
            response.ContentEncoding = Encoding.UTF8;
            //在https协议下没有给缓存权限，故需要以下二行
            response.AddHeader("Pragma", "public");
            //response.AddHeader("Cache-Control", "max-age=30");
            response.AddHeader("Cache-Control", "public");
            // 添加头信息，为"文档下载/另存为"对话框指定默认文档名 
            var userAgent = HttpContext.Current.Request.UserAgent.ToLower();
            if (userAgent.IndexOf("msie") > -1)
            {
                filename = HttpUtility.UrlPathEncode(filename);
            }
            if (userAgent.IndexOf("firefox") > -1)
            {
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
            }
            else
            {
                response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            }
            // 指定返回的是个不能被客户端读取的流，必须被下载 
            response.ContentType = contentType + "; charset=UTF-8";
            return response;
        }
        private static string ReadCell(ICell cell, IWorkbook workbook)
        {
            string cellValue = string.Empty;
            //读取Excel格式，根据格式读取数据类型
            switch (cell.CellType)
            {
                case CellType.Blank: //空数据类型处理
                    break;
                case CellType.String: //字符串类型
                    cellValue = cell.StringCellValue;
                    break;
                case CellType.Numeric: //数字类型                                   
                    if (DateUtil.IsValidExcelDate(cell.NumericCellValue))
                    {
                        cellValue = cell.DateCellValue.ToString();
                    }
                    else
                    {
                        cellValue = cell.NumericCellValue.ToString();
                    }
                    break;
                case CellType.Formula:
                    HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(workbook);
                    cellValue = e.Evaluate(cell).StringValue;
                    break;
                default:
                    break;
            }
            return cellValue;
        }
        #endregion
    }
}

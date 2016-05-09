/*
系统名称：  文件处理层
模块名称：  EXCEL文件导出
创建日期：  2012-9-27

内容摘要：  
*/
using System;
using System.Collections.Generic;
using System.IO;
using Portal.Web.Core.Model;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using System.Data;
using System.Web;
using NPOI.SS.UserModel;

namespace Portal.Web.Core
{
    public class ExcelUtility
    {
        #region 01.导出EXCEL
        /// <summary>
        /// 根据结果集和字段名json格式字符串，导出EXCEL
        /// 例：
        /// string str = "{\"MenuName\":\"菜单名称\",\"ParentMenuName\":\"父菜单名称\",\"Url\":\"URL路径\"}";
        /// ExcelManager.GetInstance.OutputExcel(list, str, "综合统计.xls");
        /// </summary>
        /// <typeparam name="T">结果集</typeparam>
        /// <param name="list">字段名json格式字符串</param>
        /// <param name="fieldstr"></param>
        /// <param name="filename"></param>
        public static void Export<T>(List<T> list, string fieldstr, string filename)
        {
            if (string.IsNullOrEmpty(fieldstr))
            {
                return;
            }
            Dictionary<string, string> dic = fieldstr.FromJson<Dictionary<string, string>>();
            DataTable table = new DataTable();
            foreach (KeyValuePair<string, string> item in dic)
            {
                table.Columns.Add(item.Value);
            }
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    DataRow row = table.NewRow();
                    foreach (KeyValuePair<string, string> kvitem in dic)
                    {
                        row[kvitem.Value] = Portal.Web.Core.Property.GetValue<string>(item, kvitem.Key);
                    }
                    table.Rows.Add(row);
                }
            }
            Export(table.DefaultView, filename);
        }

        /// <summary>
        /// 根据视图导出EXCEL
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="filename"></param>
        public static void Export(DataView dv, string filename)
        {
            Export(dv, null, filename);
        }

        /// <summary>
        /// 导出到临时文件夹下
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="path"></param>
        public static string ExportByPath(DataView dv, string path)
        {
            string filename = FileExtent.FileName;
            FileExtent.CreateDirectory(path);
            path = path + filename + ".xls";
            Export(dv, path, filename);
            return path;
        }

        /// <summary>
        /// 根据视图导出EXCEL
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public static void Export(DataView dv, string path, string filename)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = FileExtent.GetExcelExportPath;
            }
            Save(dv, path);
            ExportHelper.Excel(path, filename);
        }

        /// <summary>
        /// 根据视图导出EXCEL(大数据量)
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="filename"></param>
        /// <param name="sheetMax"></param>
        public static void ExportBig(DataView dv, string filename, int sheetMax)
        {
            string path = SaveBig(dv, sheetMax);
            ExportHelper.Excel(path, filename);
        }

        /// <summary>
        /// 以流形式输出Excel文件
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="mergeDic"></param>
        /// <param name="filename"></param>
        public static void ExportSheets(DataView dv, Dictionary<Tuple<int, int, int, int>, string> mergeDic, string filename)
        {
            ExportSheets(dv, mergeDic, null, filename);
        }

        /// <summary>
        /// 以流形式输出Excel文件
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="mergeDic"></param>
        /// <param name="filename"></param>
        public static void ExportSheets(DataView dv, Dictionary<Tuple<int, int, int, int>, string> mergeDic, Dictionary<Tuple<int, int, int, int>, string> contentDic, string filename)
        {
            string path = SaveSheets(dv, mergeDic, contentDic);
            ExportHelper.Excel(path, filename);
        }

        //public static void ExportSheets(List<ExcelCommon> list, string filename)
        //{
        //    ExportSheets(list, filename);
           
        //}
        public static void ExportSheets(List<ExcelCommon> list, string filename)
        {
            string path = SaveSheetsHeads(list);
            ExportHelper.Excel(path, filename);
        }

        #endregion

        #region 02.从表态模板读取格式并导入数据
        /// <summary>
        /// 从表态模板读取格式并导入数据
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="str"></param>
        /// <param name="filePath"></param>
        /// <param name="rowIndex"></param>
        /// <param name="filename"></param>
        public static void ExportByTempFile(DataView dv, string str, string filePath, int? rowIndex, string filename)
        {
            FileStream tempfile = new FileStream(filePath, FileMode.Open, FileAccess.Read);//读入excel模板
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(tempfile);

            //create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI TeamFile";
            hssfworkbook.DocumentSummaryInformation = dsi;

            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK TeamFile";
            hssfworkbook.SummaryInformation = si;

            ICellStyle style = hssfworkbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            IFont font = hssfworkbook.CreateFont();
            font.FontHeight = 15 * 20;
            style.SetFont(font);

            ISheet sheet1 = hssfworkbook.GetSheet("Sheet1");
            if (sheet1 == null)
                sheet1 = hssfworkbook.CreateSheet("Sheet1");

            rowIndex = rowIndex ?? 1;

            int[] weidth = new int[dv.Table.Columns.Count];
            for (int i = 0; i < dv.Table.Columns.Count; i++)
            {
                //var r = sheet1.CreateRow(0).CreateCell(i);
                //r.SetCellValue(dv.Table.Columns[i].ColumnName);
                //r.CellStyle = style;
                weidth[i] = ExcelBottom.GetLength(dv.Table.Columns[i].ColumnName) * 2;
            }
            //var ii = 1;
            foreach (DataRowView row in dv)
            {
                IRow dataRow = sheet1.GetRow(rowIndex.Value);
                if (dataRow == null)
                {
                    dataRow = sheet1.CreateRow(rowIndex.Value);
                }
                for (int i = 0; i < dv.Table.Columns.Count; i++)
                {
                    ICell datacell = dataRow.GetCell(i);
                    if (datacell == null)
                    {
                        datacell = dataRow.CreateCell(i);
                    }
                    datacell.SetCellValue(row[dv.Table.Columns[i].ColumnName].ToString());
                    var w = ExcelBottom.GetLength(row[dv.Table.Columns[i].ColumnName].ToString().Trim());
                    if (weidth[i] < w)
                    {
                        weidth[i] = w;
                    }
                }
                rowIndex++;
            }
            //设置列宽
            for (int i = 0; i < dv.Table.Columns.Count; i++)
            {
                sheet1.SetColumnWidth(i, weidth[i] * 256);
            }
            string path = HttpContext.Current.Server.MapPath("~");
            path = path[path.Length - 1] == '\\' ? path : path + "\\";
            path = path + "TempFiles\\";
            FileExtent.CreateDirectory(path);
            path = path + FileExtent.FileName + ".xls";
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            ExportHelper.Excel(path, filename);
        }
        #endregion

        #region 03.生成Excel并(保存到指定目录\指定文件名)
        /// <summary>
        /// 生成Excel并保存到指定目录
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="filePath">XLS文件完整路径</param>
        public static string Save(DataView dv, string filePath = "")
        {
            return Save(dv, null, filePath);
        }
        /// <summary>
        /// 生成Excel并保存到指定目录
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="filename"></param>
        /// <param name="filePath"></param>
        public static string Save(DataView dv, string filename, string filePath)
        {
            ICellStyle style;
            var hssfworkbook = ExcelBottom.CreateWorkbook(out style);
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
            ExcelBottom.LoadSheet(dv, 0, dv.Table.Rows.Count, sheet1, style);
            string path = filePath;
            if (string.IsNullOrEmpty(filename))
            {
                if (string.IsNullOrEmpty(path))
                {
                    path = FileExtent.GetExcelExportPath;
                }
            }
            else
            {
                path = path[path.Length - 1] == '\\' ? path : path + "\\";
                path = path + "TempFiles\\";
                FileExtent.CreateDirectory(path);
                path = path + filename + ".xls";
            }
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }
        /// <summary>
        /// 根据视图生成EXCEL(大数据量)
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="sheetMax">一个sheet的最大行数</param>
        public static string SaveBig(DataView dv, int sheetMax)
        {
            int maxCount = sheetMax;
            ICellStyle style;
            var hssfworkbook = ExcelBottom.CreateWorkbook(out style);
            var rowCount = dv.Table.Rows.Count;
            for (int i = 0; ; rowCount -= maxCount, i++)
            {
                ISheet sheet = hssfworkbook.CreateSheet(string.Format("Sheet{0}", i + 1));
                if (rowCount > maxCount)
                {
                    ExcelBottom.LoadSheet(dv, maxCount * i, maxCount, sheet, style);
                }
                else
                {
                    ExcelBottom.LoadSheet(dv, maxCount * i, rowCount, sheet, style);
                    break;
                }
            }
            string path = FileExtent.GetExcelExportPath;
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }

        /// <summary>
        /// 以流形式输出Excel文件
        /// </summary>
        /// <param name="dv">数据源</param>
        /// <param name="mergeDic">合并单元格的起止行号列号</param>
        /// <param name="contentDic">内容合并单元格的起止行号列号</param>
        public static string SaveSheets(DataView dv, Dictionary<Tuple<int, int, int, int>, string> mergeDic, Dictionary<Tuple<int, int, int, int>, string> contentDic = null)
        {
            if (mergeDic == null || mergeDic.Count == 0)
            {
                return Save(dv, null);
            }
            ICellStyle style;
            var hssfworkbook = ExcelBottom.CreateWorkbook(out style);
            style.VerticalAlignment = VerticalAlignment.Center;
            ISheet sheet1 = hssfworkbook.CreateSheet("Sheet1");
            var rowCount = 0;
            ExcelBottom.CreateCells(mergeDic, rowCount, sheet1, style);
            ExcelBottom.LoadSheet(dv, 0, dv.Table.Rows.Count, sheet1, style, rowCount + 1, false);
            ExcelBottom.CreateCells(contentDic, rowCount, sheet1, style);
            string path = FileExtent.GetExcelExportPath;
            //保存文件到硬盘
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }

        /// <summary>
        /// 以流形式输出Excel文件
        /// </summary>
        /// <param name="list">视图及字典集合</param>
        public static string SaveSheets(List<ExcelCommon> list)
        {
            ICellStyle style;
            var hssfworkbook = ExcelBottom.CreateWorkbook(out style);
            style.VerticalAlignment = VerticalAlignment.Center;
            int i = 1;
            int rowCount = 0;
            foreach (var item in list)
            {
                //if ((item.MergeDic == null || item.MergeDic.Count == 0) && (item.ContentDic == null || item.ContentDic.Count == 0))
                //{
                //    return SaveExcel(item.DV, null);
                //}
                rowCount = 0;
                ISheet sheet = hssfworkbook.CreateSheet("Sheet" + i + "");
                ExcelBottom.CreateCells(item.MergeDic, rowCount, sheet, style);
                ExcelBottom.LoadSheet(item.DV, 0, item.DV.Table.Rows.Count, sheet, style, rowCount + 1, false);
                ExcelBottom.CreateCells(item.ContentDic, rowCount, sheet, style, true);
                i++;
            }
            string path = FileExtent.GetExcelExportPath;
            //保存文件到硬盘
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }

        /// <summary>
        /// 以流形式输出Excel文件 创建表头
        /// </summary>
        /// <param name="list">视图及字典集合</param>
        public static string SaveSheetsHeads(List<ExcelCommon> list)
        {
            ICellStyle style;
            var hssfworkbook = ExcelBottom.CreateWorkbook(out style);
            style.VerticalAlignment = VerticalAlignment.Center;
            int rowCount = 0;
            foreach (var item in list)
            {
                rowCount = 0;
                ISheet sheet = hssfworkbook.CreateSheet(item.SheetName + "");
                ExcelBottom.CreateCells(item.MergeDic, rowCount, sheet, style);
                ExcelBottom.LoadSheet(item.DV, 0, item.DV.Table.Rows.Count, sheet, style, rowCount + 1, true);
                ExcelBottom.CreateCells(item.ContentDic, rowCount, sheet, style, true);
            }
            string path = FileExtent.GetExcelExportPath;
            //保存文件到硬盘
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }

        #endregion

        #region 04.把Excel转换成DataTable、DataSet
        /// <summary>
        /// 把Excel文件数据流转换成DataTable
        /// </summary>
        /// <param name="postfile"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(HttpPostedFile postfile)
        {
            return ToDataTable(postfile.InputStream);
        }
        /// <summary>
        /// 把Excel文件数据流转换成DataTable
        /// </summary>
        /// <param name="stream">Excel Stream</param>
        /// <returns></returns>
        public static DataTable ToDataTable(Stream stream)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(stream);
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            return ExcelBottom.GetTable(hssfworkbook, sheet);
        }

        /// <summary>
        /// 把Excel文件数据流转换成DataTable
        /// </summary>
        /// <param name="stream">Excel Stream</param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(Stream stream, string sheetName)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(stream);
            ISheet sheet = hssfworkbook.GetSheet(sheetName);
            return ExcelBottom.GetTable(hssfworkbook, sheet);
        }

        /// <summary>
        /// 把Excel文件数据流转换成DataSet
        /// </summary>
        /// <param name="stream">Excel Stream</param>
        /// <returns></returns>
        public static DataSet ToDataSet(Stream stream)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook(stream);
            DataSet dataSet = new DataSet();
            int count = hssfworkbook.NumberOfSheets;
            for (int i = 0; i < count; i++)
            {
                ISheet sheet = hssfworkbook.GetSheetAt(i);
                ExcelBottom.AddTable(hssfworkbook, sheet, dataSet);
            }
            return dataSet;
        }
        #endregion
    }
}

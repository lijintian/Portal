/*
系统名称：  文件处理层
模块名称：  EXCEL文件导出
创建日期：  2012-9-27

内容摘要：  
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Portal.Web.Core
{
    internal class ExcelBottom
    {
        #region 01.属性
        internal static string FileName
        {
            get { return DateTime.Now.ToString("yyyyMMddHHmmss"); }
        }
        #endregion

        #region 02.创建合并的单元格
        /// <summary>
        /// 创建合并的单元格
        /// </summary>
        /// <param name="mergeDic"></param>
        /// <param name="rowCount"></param>
        /// <param name="sheet1"></param>
        /// <param name="style"></param>
        /// <param name="isContent">是否为内容的字典</param>
        internal static void CreateCells(Dictionary<Tuple<int, int, int, int>, string> mergeDic, int rowCount, ISheet sheet1, ICellStyle style, bool isContent = false)
        {
            if ((mergeDic != null) && (mergeDic.Count > 0))
            {
                foreach (var key in mergeDic.Keys)
                {
                    Tuple<int, int, int, int> tuple = key;
                    rowCount = (tuple.Item2 > rowCount) ? tuple.Item2 : rowCount;
                    CellRangeAddress region = new CellRangeAddress(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
                    sheet1.AddMergedRegion(region);
                    ICell cell = sheet1.CreateRow(tuple.Item1).CreateCell(tuple.Item3);
                    cell.SetCellValue(mergeDic[key]);
                    if (!isContent || tuple.Item3 == 0)
                    {
                        cell.CellStyle = style;
                    }
                }
            }
        }
        #endregion

        #region 03.按数据源的格式保存数据
        /// <summary>
        /// 按数据源的格式保存数据
        /// </summary>
        internal static void SetCellValue(ICell newCell, string cellValue, Type columnType)
        {
            if (string.IsNullOrEmpty(cellValue))
            {
                newCell.SetCellValue("");
                return;
            }

            #region 设置数据格式

            switch (columnType.ToString())
            {
                case "System.String": //字符串类型
                    newCell.SetCellValue(cellValue);
                    break;
                case "System.DateTime": //日期类型
                    DateTime dateV;
                    DateTime.TryParse(cellValue, out dateV);
                    newCell.SetCellValue(dateV);
                    break;
                case "System.Boolean": //布尔型                        
                    bool boolV;
                    bool.TryParse(cellValue, out boolV);
                    newCell.SetCellValue(boolV);
                    break;
                case "System.Int16": //整型                    
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    int intV;
                    int.TryParse(cellValue, out intV);
                    newCell.SetCellValue(intV);
                    break;
                case "System.Decimal": //浮点型                    
                case "System.Double":
                    double doubV;
                    double.TryParse(cellValue, out doubV);
                    newCell.SetCellValue(doubV);
                    break;
                case "System.DBNull": //空值处理                        
                    newCell.SetCellValue("");
                    break;
                default:
                    //throw (new Exception(rowType.ToString() + "：类型数据无法处理!"));
                    newCell.SetCellValue(cellValue);
                    break;
            }

            #endregion
        }
        #endregion

        #region 04.操作HSSFWorkbook
        /// <summary>
        /// 添加table数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="hssfworkbook"></param>
        /// <returns></returns>
        internal static void AddTable(HSSFWorkbook hssfworkbook, ISheet sheet, DataSet dataSet)
        {
            DataTable dt = GetTable(hssfworkbook, sheet);
            dataSet.Tables.Add(dt);
        }
        /// <summary>
        /// 获取table数据
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="hssfworkbook"></param>
        /// <returns></returns>
        internal static DataTable GetTable(HSSFWorkbook hssfworkbook, ISheet sheet)
        {
            IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable { TableName = sheet.SheetName };
            //DataSet dataSet = new DataSet();
            bool flag = false;
            int cellNum = 0;
            while (rows.MoveNext())
            {
                flag = AddRow(rows, dt, flag, cellNum);
            }
            //dataSet.Tables.Add(dt);
            return dt;
        }
        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="dt"></param>
        /// <param name="flag"></param>
        /// <param name="cellNum"></param>
        /// <returns></returns>
        internal static bool AddRow(IEnumerator rows, DataTable dt, bool flag, int cellNum)
        {
            HSSFRow row = (HSSFRow)rows.Current;
            DataRow dr = dt.NewRow();
            if (!flag)
            {
                cellNum = row.LastCellNum;
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
                }
                flag = true;
            }
            for (int i = 0; i < cellNum; i++)
            {
                ICell cell = row.GetCell(i);
                if (cell == null)
                {
                    dr[i] = null;
                }
                else
                {
                    dr[i] = cell.ToString();
                }
            }
            dt.Rows.Add(dr);
            return flag;
        }

        #endregion

        #region 05.其他方法
        /// <summary>
        /// 创建HSSFWorkbook
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        internal static HSSFWorkbook CreateWorkbook(out ICellStyle style)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            hssfworkbook.DocumentSummaryInformation = dsi;
            //create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            hssfworkbook.SummaryInformation = si;
            style = hssfworkbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            IFont font = hssfworkbook.CreateFont();
            font.FontHeight = 15 * 20;
            style.SetFont(font);
            return hssfworkbook;
        }

        /// <summary>
        /// 往载sheet填入数据
        /// </summary>
        /// <param name="dv">数据</param>
        /// <param name="begin">开始行</param>
        /// <param name="count">sheet的总行数</param>
        /// <param name="sheet">要填入数据的sheet</param>
        /// <param name="style">每个cell的格式</param>
        /// <param name="rowIndex">数据从哪一行开始加载</param>
        /// <param name="createHead">创建列头</param>
        internal static void LoadSheet(DataView dv, int begin, int count, ISheet sheet, ICellStyle style, int rowIndex = 1, bool createHead = true)
        {
            try
            {
                int[] weidth = new int[dv.Table.Columns.Count];
                if (createHead)
                {
                    IRow row = sheet.CreateRow(0);
                    for (int i = 0; i < dv.Table.Columns.Count; i++)
                    {
                        var cell = row.CreateCell(i);
                        cell.SetCellValue(dv.Table.Columns[i].ColumnName);
                        cell.CellStyle = style;
                    }
                }
                for (int i = 0; i < dv.Table.Columns.Count; i++)
                {
                    weidth[i] = GetLength(dv.Table.Columns[i].ColumnName) * 2;
                }
                Type columnType;//列类型
                string cellValue = string.Empty;//列值
                for (int i = begin; i < begin + count; i++)
                {
                    DataRowView row = dv[i];
                    IRow iRow = sheet.CreateRow(rowIndex);
                    for (int j = 0; j < dv.Table.Columns.Count; j++)
                    {
                        cellValue = row[dv.Table.Columns[j].ColumnName].ToString();
                        columnType = dv.Table.Columns[j].DataType;
                        ICell newCell = iRow.CreateCell(j);
                        SetCellValue(newCell, cellValue, columnType);
                        var w = GetLength(row[dv.Table.Columns[j].ColumnName].ToString().Trim());
                        if (weidth[j] < w)
                        {
                            weidth[j] = w;
                        }
                    }
                    rowIndex++;
                }
                //设置列宽
                for (int i = 0; i < dv.Table.Columns.Count; i++)
                {
                    sheet.SetColumnWidth(i, weidth[i] * 256);
                }
            }
            catch { }
        }

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static int GetLength(string str)
        {
            int intLen = str.Length;
            int i;
            char[] chars = str.ToCharArray();
            for (i = 0; i < chars.Length; i++)
            {
                if (Convert.ToInt32(chars[i]) > 255)
                {
                    intLen++;
                }
            }
            return intLen;
        }
        #endregion
    }
}

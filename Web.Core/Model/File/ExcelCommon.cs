using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Portal.Web.Core.Model
{
    public class ExcelCommon
    {
        /// <summary>
        /// 数据表
        /// </summary>
        public DataView DV { get; set; }

        /// <summary>
        /// 合并的单元格坐标（标题列）
        /// </summary>
      public  Dictionary<Tuple<int, int, int, int>, string> MergeDic { get; set; }

        /// <summary>
        /// 合并的单元格坐标（内列类）
        /// </summary>
       public Dictionary<Tuple<int, int, int, int>, string> ContentDic { get; set; }
       /// <summary>
       /// Execl Sheet名称
       /// </summary>
       public string SheetName { get; set; }
    }
}

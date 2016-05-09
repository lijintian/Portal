using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Portal.Web.Core;
using Portal.Web.Core.Common;
using Portal.Web.Core.Model;
using EasyDDD.Core.Event;


namespace Portal.Applications.Helper
{
    public static class CheckHelper
    {
        #region 字段
        private const int MaxInt = 2147483647;
        #endregion

        /// <summary>
        /// 检查变量是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <param name="columnName"></param>
        /// <param name="errorList"></param>
        /// <returns></returns>
        public static ReturnModel<T> CheckEnum<T>(string str, string columnName, List<string> errorList)
        {
            ReturnModel<T> info = EnumUtility.GetValueFromDescription<T>(str);
            if (info == null || !info.Status)
            {
                errorList.Add(string.Format("{0}【{1}】输入有误", columnName, str));
                return new ReturnModel<T>(false);
            }
            else
            {
                return info;
            }
        }

        /// <summary>
        /// 检查字符是否是整数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="columnName"></param>
        /// <param name="errorList"></param>
        /// <param name="canEmpty">是否允许为空</param>
        /// <returns></returns>
        public static ReturnModel<int> CheckInt(string str, string columnName, List<string> errorList, bool canEmpty = false)
        {
            if (CheckUtility.IsEmpty(str))
            {
                if (!canEmpty)
                {
                    errorList.Add(string.Format("【{0}】不能为空", columnName));
                }
                return new ReturnModel<int>(canEmpty);
            }
            bool success = CheckUtility.IsInteger(str, canEmpty);
            if (!success)
            {
                errorList.Add(string.Format("【{0}】请输入大于0的整数", columnName));
                return new ReturnModel<int>(false);
            }
            int value = 0;
            success = int.TryParse(str, out value);
            if (!success)
            {
                errorList.Add(string.Format("【{0}】不能大于{1}", columnName, MaxInt));
                return new ReturnModel<int>(false);
            }
            return new ReturnModel<int>(value, string.Empty, true);
        }

        /// <summary>
        /// 检查整数是否在指定范围内
        /// </summary>
        /// <param name="value"></param>
        /// <param name="columnName"></param>
        /// <param name="errorList"></param>
        /// <returns></returns>
        public static bool CheckRange(int value, string columnName, List<string> errorList)
        {
            return CheckRange(value, 0, MaxInt, columnName, errorList);
        }

        /// <summary>
        /// 检查整数是否在指定范围内
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="columnName"></param>
        /// <param name="errorList"></param>
        /// <returns></returns>
        public static bool CheckRange(int value, int min, int max, string columnName, List<string> errorList)
        {
            //int value = Convert.ToInt32(str);
            var success = true;
            if (value < min)
            {
                success = false;
                errorList.Add(string.Format("【{0}】不能小于{1}", columnName, min));
            }
            if (value > max)
            {
                success = false;
                errorList.Add(string.Format("【{0}】不能大于{1}", columnName, max));
            }
            return success;
        }

        /// <summary>
        /// 判断是否包含中文逗号
        /// </summary>
        /// <param name="str"></param>
        /// <param name="columnName"></param>
        /// <param name="errorList"></param>
        /// <returns></returns>
        public static bool CheckComma(string str, string columnName, List<string> errorList)
        {
            var result = str.Contains("，");
            if (str.Contains("，"))
            {
                errorList.Add(string.Format("【{0}】输入内容有误，包含中文逗号", columnName));
            }
            return result;
        }

        /// <summary>
        /// 判断是否是字母数字下划线
        /// </summary>
        /// <param name="str"></param>
        /// <param name="columnName"></param>
        /// <param name="errorList"></param>
        /// <returns></returns>
        public static bool CheckNumLetter(string str, string columnName, List<string> errorList)
        {
            var result = CheckUtility.IsNumLetter(str);
            if (!result)
            {
                errorList.Add(string.Format("【{0}】只能输入字母、数字、下划线", columnName));
            }
            return result;
        }

        /// <summary>
        /// 检查变量是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <param name="columnName"></param>
        /// <param name="errorList"></param>
        /// <returns></returns>
        public static bool CheckNotEmpty(string str, string columnName, List<string> errorList)
        {
            bool result = CheckUtility.IsEmpty(str);
            if (result)
            {
                errorList.Add(string.Format("【{0}】不能为空", columnName));
            }
            return !result;
        }

        /// <summary>
        /// 检查字符串长度是否在指定范围内
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool CheckByteLen(string str, int maxLength, string columnName, List<string> errorList)
        {
            if (CheckUtility.IsEmpty(str))
            {
                return true;
            }
            bool result = str.GetByteLen() <= maxLength;
            if (!result)
            {
                errorList.Add(string.Format("【{0}】长度不能超过{1}个字节", columnName, maxLength));
            }
            return result;
        }

        /// <summary>
        /// 添加错误行数据
        /// </summary>
        /// <param name="tabErr"></param>
        /// <param name="dr"></param>
        /// <param name="errorList"></param>
        public static void AddErrorRow(DataTable tabErr, DataRow dr, List<string> errorList)
        {
            DataRow errorRow = tabErr.NewRow();
            int i = 0;
            foreach (DataColumn culumn in tabErr.Columns)
            {
                i++;
                if (i == tabErr.Columns.Count)
                {
                    errorRow["错误"] = string.Join("；", errorList);
                }
                else
                {
                    errorRow[culumn.ColumnName] = dr[culumn.ColumnName];
                }
            }
            tabErr.Rows.Add(errorRow);
        }

        public static void CallBack<TDomainEventResult>(string msg, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            Events.Callbacks.ImportCheckEventResult result = new Events.Callbacks.ImportCheckEventResult(msg);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
        public static void CallBack<TDomainEventResult>(DataTable dt, int count, int successCount, Action<TDomainEventResult> callback) where TDomainEventResult : IDomainEventResult
        {
            bool success = true;
            string message = "操作已成功";
            if (successCount == 0)
            {
                message = "操作已完成,所有数据导入失败,请点击下载查看";
                success = false;
            }
            else if (successCount < count)
            {
                message = "操作已完成,但以下数据导入失败,请点击下载查看";
                success = false;
            }
            Events.Callbacks.ImportCheckEventResult result = new Events.Callbacks.ImportCheckEventResult(dt, message, success);
            if (callback != null)
            {
                callback((TDomainEventResult)(IDomainEventResult)result);
            }
        }
    }
}

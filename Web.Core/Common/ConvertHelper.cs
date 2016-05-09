using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Threading;
using Portal.Dto.Request;

namespace Portal.Web.Core
{
    public class ConvertHelper<T> where T : new()
    {
        /// <summary>  
        /// DataTable转换为List 
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static List<T> ConvertToList(DataTable dt)
        {
            Dictionary<string, string> fields = GetFields();
            return ConvertToList(dt, fields);
        }

        /// <summary>  
        /// DataTable转换为List 
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static List<T> ConvertToList(DataTable dt, Dictionary<string, string> fields)
        {
            // 定义集合  
            List<T> list = new List<T>();
            //定义一个临时变量  
            string tempName = string.Empty;
            string drName = BaseImportRequest.GetItemName();
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                if (IsWhiteRow(dr, dt.Columns.Count))
                    continue;
                T obj = new T();
                var propertys = obj.GetProperties();
                //遍历该对象的所有属性  
                foreach (PropertyInfo att in propertys)
                {
                    if (!att.CanWrite) continue;
                    tempName = fields[att.Name];
                    if (tempName.Equals(drName))
                    {
                        att.SetValue(obj, dr, null);
                    }
                    //检查DataTable是否包含此列（列名==对象的属性名）    
                    else if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter  
                        if (!att.CanWrite) continue;//该属性不可写，直接跳出  
                        string value = dr[tempName] != DBNull.Value ? dr[tempName].ToString() : string.Empty;
                        var type = att.PropertyType;
                        //如果非空，则赋给对象的属性  
                        if (!string.IsNullOrEmpty(value))
                        {
                            att.SetValue(obj, ChangeType(value.Trim(), type), null);
                        }
                    }
                }
                //对象添加到泛型集合中  
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetFields()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var propertys = Property.GetProperties((typeof(T)));
            string tempName = string.Empty;
            //遍历该对象的所有属性  
            foreach (PropertyInfo att in propertys)
            {
                if (!att.CanWrite) continue;
                tempName = att.Name; //将属性名称赋值给临时变量  
                //取描述
                Object[] objs = att.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs.Length > 0)
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    if (da != null && !string.IsNullOrEmpty(da.Description))
                    {
                        tempName = da.Description;
                    }
                }
                dic.Add(att.Name, tempName);
            }
            return dic;
        }

        /// <summary>
        /// 是否为空白记录
        /// </summary>
        /// <param name="row"></param>
        /// <param name="colLen"></param>
        /// <returns></returns>
        private static bool IsWhiteRow(DataRow row, int colLen)
        {
            for (int j = 0; j < colLen; j++)
            {
                if (!string.IsNullOrWhiteSpace(row[j].ToString()))
                    return false;
            }
            return true;
        }

        #region 强制类型转换
        public static object ChangeType(object obj, Type conversionType)
        {
            return ChangeType(obj, conversionType, Thread.CurrentThread.CurrentCulture);
        }
        public static object ChangeType(object obj, Type conversionType, IFormatProvider provider)
        {
            #region Nullable
            Type nullableType = Nullable.GetUnderlyingType(conversionType);
            if (nullableType != null)
            {
                if (obj == null)
                {
                    return null;
                }
                return Convert.ChangeType(obj, nullableType, provider);
            }
            #endregion
            if (typeof(System.Enum).IsAssignableFrom(conversionType))
            {
                return Enum.Parse(conversionType, obj.ToString());
            }
            return Convert.ChangeType(obj, conversionType, provider);
        }
        #endregion
    }
}

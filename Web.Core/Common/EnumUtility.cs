using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Portal.Web.Core.Model;

namespace Portal.Web.Core
{
    public static class EnumUtility
    {
        /// <summary>
        /// 根据INT类型获取枚举描述
        /// </summary>
        /// <returns></returns>
        public static T GetEnum<T>(this object enum1)
        {
            return (T)Enum.Parse(typeof(T), enum1.ToString());
        }

        #region 获取枚举类子项描述信息
        /// <summary>
        /// 根据INT类型获取枚举描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription<T>(int value)
        {
            return GetEnum<T>(value).GetDescription();
        }
        /// <summary>
        /// 获取枚举类子项描述信息
        /// </summary>
        /// <example>
        /// 示例：GetDescription(PermissionTypeEm.Page) 
        ///       PermissionTypeEm.Page.GetDescription()
        /// </example>
        /// <param name="enum1">枚举类子项</param>        
        public static string GetDescription(this object enum1)
        {
            enum1 = (Enum)enum1;
            string strValue = enum1.ToString();
            FieldInfo fieldinfo = enum1.GetType().GetField(strValue);
            if (fieldinfo != null)
            {

                Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs.Length == 0)
                {
                    return strValue;
                }
                DescriptionAttribute da = (DescriptionAttribute)objs[0];
                return da.Description;
            }
            return "不限";
        }
        #endregion

        #region 根据枚举类型返回类型中的所有值，文本及描述
        /// <summary>
        /// 根据枚举类型返回类型中的所有值，文本及描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns>返回三列数组，第0列为Description,第1列为Value，第2列为Text</returns>
        public static List<string[]> GetList(Type type)
        {
            List<string[]> list = new List<string[]>();
            FieldInfo[] fields = type.GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                string[] strEnum = new string[3];
                FieldInfo field = fields[i];
                //值列
                strEnum[1] = ((int)Enum.Parse(type, field.Name)).ToString();
                //文本列赋值
                strEnum[2] = field.Name;

                object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs.Length == 0)
                {
                    strEnum[0] = field.Name;
                }
                else
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    strEnum[0] = da.Description;
                }

                list.Add(strEnum);
            }
            return list;
        }
        #endregion

        #region 字符串转换成枚举
        /// <summary>
        /// 字符串转换成枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumtext"></param>
        /// <returns></returns>
        public static T GetEnum<T>(string enumtext)
        {
            return (T)Enum.Parse(typeof(T), enumtext);
        }
        public static T GetEnum<T>(int value)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            return (T)Enum.ToObject(type, value);
        }
        /// <summary>
        /// 根据枚举值注释获取对应枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="description">枚举值注释</param>
        /// <returns></returns>
        public static ReturnModel<T> GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null)
                {
                    if (attribute.Description == description)
                    {
                        return new ReturnModel<T>((T)field.GetValue(null), string.Empty, true);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return new ReturnModel<T>((T)field.GetValue(null), string.Empty, true);
                    }
                }
            }
            return new ReturnModel<T>(false);
        }
        #endregion

        /// <summary>
        /// 将枚举转换为List数据源
        /// </summary>
        /// <returns></returns>
        public static EnumModel GetModel(this object enum1)
        {
            return new EnumModel(Convert.ToInt32(enum1), enum1.GetDescription());
        }

        /// <summary>
        /// 将枚举转换为List数据源
        /// </summary>
        /// <returns></returns>
        public static EnumModel GetModel2(this object enum1)
        {
            return new EnumModel(enum1.ToString(), enum1.GetDescription());
        }
    }
}

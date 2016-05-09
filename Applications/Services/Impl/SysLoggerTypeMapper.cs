using System;
using DtoLoggerLevel = Portal.Dto.SysLoggerLevel;
using DomainLoggerLevel = Portal.Domain.Aggregates.SysLoggerLevel;
using DtoLoggerType = Portal.Dto.SysLoggerType;
using DomainLoggerType = Portal.Domain.Aggregates.SysLoggerType;
using DtoLoggerRight = Portal.Dto.SysLoggerRight;
using DomainLoggerRight = Portal.Domain.Aggregates.SysLoggerRight;

namespace Portal.Applications.Services.Impl
{
    static class SysLoggerTypeMapper
    {
        public static DomainLoggerLevel MapToLevel(DtoLoggerLevel level)
        {
            return GetEnum<DomainLoggerLevel>(level.ToString());
        }

        public static DtoLoggerLevel MapToLevel(DomainLoggerLevel level)
        {
            return GetEnum<DtoLoggerLevel>(level.ToString());
        }

        public static DomainLoggerType MapToType(DtoLoggerType Type)
        {
            return GetEnum<DomainLoggerType>(Type.ToString());
        }

        public static DtoLoggerType MapToType(DomainLoggerType Type)
        {
            return GetEnum<DtoLoggerType>(Type.ToString());
        }

        public static DomainLoggerRight MapToRight(DtoLoggerRight Right)
        {
            return GetEnum<DomainLoggerRight>(Right.ToString());
        }

        public static DtoLoggerRight MapToRight(DomainLoggerRight Right)
        {
            return GetEnum<DtoLoggerRight>(Right.ToString());
        }

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
    }
}

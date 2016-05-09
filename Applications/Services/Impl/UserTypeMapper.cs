using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DtoUserType = Portal.Dto.UserType;
using DomainUserType = Portal.Domain.Aggregates.UserAgg.UserType;

namespace Portal.Applications.Services.Impl
{
    static class UserTypeMapper
    {
        private readonly static IList<Tuple<DtoUserType, DomainUserType>> Maps = new List<Tuple<DtoUserType, DomainUserType>>();
        static UserTypeMapper()
        {
            UserTypeMapper.Maps.Add(new Tuple<DtoUserType, DomainUserType>(DtoUserType.InternalApi, DomainUserType.InternalApi));
            UserTypeMapper.Maps.Add(new Tuple<DtoUserType, DomainUserType>(DtoUserType.Customer, DomainUserType.Customer));
            UserTypeMapper.Maps.Add(new Tuple<DtoUserType, DomainUserType>(DtoUserType.Employee, DomainUserType.Employee));
            UserTypeMapper.Maps.Add(new Tuple<DtoUserType, DomainUserType>(DtoUserType.ExternalApi, DomainUserType.ExternalApi));
        }

        public static DomainUserType MapToDomainUserType(DtoUserType dtoUserType)
        {
            return UserTypeMapper.Maps.First(item => item.Item1 == dtoUserType).Item2;
        }

        public static DtoUserType MapToDtoUserType(DomainUserType domainUserType)
        {
            return UserTypeMapper.Maps.First(item => item.Item2 == domainUserType).Item1;
        }
    }
}

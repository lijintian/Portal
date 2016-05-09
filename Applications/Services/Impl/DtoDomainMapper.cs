using System.Collections.Generic;
using System.Linq;
using Portal.Domain.Aggregates.AuthorizationCodeAgg;
using Portal.Domain.Aggregates.TokenWrapperAgg;
using Portal.Dto;
using DomainRole = Portal.Domain.Aggregates.RoleAgg.Role;
using DomainPermission = Portal.Domain.Aggregates.PermissionAgg.Permission;
using DomainPagePermission = Portal.Domain.Aggregates.PermissionAgg.PagePermission;
using DomainFunctionPermission = Portal.Domain.Aggregates.PermissionAgg.FunctionPermission;
using DomainApiPermission = Portal.Domain.Aggregates.PermissionAgg.ApiPermission;
using DomainUser = Portal.Domain.Aggregates.UserAgg.User;
using DomainMenu = Portal.Domain.Aggregates.MenuAgg.Menu;
using DomainApiPermissionGroup = Portal.Domain.Aggregates.ApiPermissionGroupAgg.ApiPermissionGroup;

namespace Portal.Applications.Services.Impl
{
    static class DtoDomainMapper
    {
        public static Role ConvertToDto(DomainRole role)
        {
            return new Role()
            {
                ApplicationId = role.ApplicationId,
                Code = role.Code,
                Id = role.Id,
                Name = role.Name,
                Desc = role.Desc
            };
        }

        public static Permission ConvertToDto(DomainPermission per)
        {
            return new Permission()
            {
                Id = per.Id,
                Code = per.Code,
                Desc = per.Desc,
                IsCompatible = per.IsCompatible,
                Name = per.Name,
                Order = per.Order,
                Tag = per.Tag,
            };
        }

        public static PagePermission ConvertToDto(DomainPagePermission pagePer, IEnumerable<DomainFunctionPermission> funcPers)
        {
            var dtoPagePer = new PagePermission()
            {
                Id = pagePer.Id,
                ApplicationId = pagePer.ApplicationId,
                Tag = pagePer.Tag,
                Code = pagePer.Code,
                Name = pagePer.Name,
                IsCompatible = pagePer.IsCompatible,
                Desc = pagePer.Desc,
                Order = pagePer.Order
            };

            funcPers = funcPers ?? Enumerable.Empty<DomainFunctionPermission>();
            foreach (var per in funcPers)
            {
                dtoPagePer.AddFuncPermission(DtoDomainMapper.ConvertToDto(per));
            }

            return dtoPagePer;
        }

        public static ApiPermission ConvertToDto(DomainApiPermission per)
        {
            return new ApiPermission()
            {
                Id = per.Id,
                ApplicationId = per.ApplicationId,
                Code = per.Code,
                Desc = per.Desc,
                IsOpened = per.IsOpened,
                IsCustomerGranted = per.IsCustomerGranted,
                IsCompatible = per.IsCompatible,
                Name = per.Name,
                Order = per.Order,
                Tag = per.Tag
            };
        }

        public static FunctionPermission ConvertToDto(DomainFunctionPermission funcPer)
        {
            return new FunctionPermission()
            {
                Id = funcPer.Id,
                Code = funcPer.Code,
                Desc = funcPer.Desc,
                IsCompatible = funcPer.IsCompatible,
                Name = funcPer.Name,
                Order = funcPer.Order,
                ParentId = funcPer.ParentId,
                Tag = funcPer.Tag
            };
        }

        public static User ConvertToDto(DomainUser user)
        {
            return new User()
            {
                Id = user.Id,
                Desc = user.Desc,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Phone = user.Phone,
                IsApproved = user.IsApproved,
                EmployeeNo = user.EmployeeNo,
                ClientNo = user.ClientNo,
                LoginName = user.LoginName,
                Password = user.Password,
                CreatedOn = user.CreatedOn,
                UserType = (UserType)user.UserType

            };
        }

        public static Menu ConvertToDto(DomainMenu menu)
        {
            return new Menu()
            {
                Id = menu.Id,
                ApplicationId = menu.ApplicationId,
                Desc = menu.Desc,
                Name = menu.Name,
                Order = menu.Sequence,
                ParentId = menu.ParentId,
                Target = menu.Target,
                IsShare = menu.IsShare,
                PermissionCode = menu.PermissionCode,
                Url = menu.Url,
                IsAbsoluteUrl = menu.IsAbsoluteUrl,
            };
        }

        public static TokenWrapperDto ConvertToDto(TokenWrapper token)
        {
            return new TokenWrapperDto()
            {
                Id = token.Id,
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                IsExternal = token.IsExternal,
                CustomerIdentity = token.CustomerIdentity,
                ClientId = token.ClientId,
                DeveloperAppName = token.DeveloperAppName,
                AccessTokenExpiredTime = token.AccessTokenExpiredTime,
                RefreshTokenExpiredTime = token.RefreshTokenExpiredTime,
                IsExpired = token.IsExpired(),
                CreatedOn = token.CreatedOn,
                IsDisabled = token.IsDisabled,
                Codes = token.ApiPermissionCodes == null ? null : token.ApiPermissionCodes.Select(item => item.Code).ToArray()
            };
        }

        public static AuthorizationCodeDto ConvertToDto(AuthorizationCode token)
        {
            return new AuthorizationCodeDto()
            {
                Id = token.Id,
                Code = token.Code,
                Permssions = token.Permssions == null ? null : token.Permssions.Select(item => item.Code).ToArray(),
                CustomerIdentity = token.CustomerIdentity,
                ClientId = token.ClientId,
                DeveloperAppName = token.DeveloperAppName,
                AuthorizationTime = token.AuthorizationTime,
                ExpiredTime = token.ExpiredTime,
                IsExpired = token.IsExpired(),
                IsUsed = token.IsUsed,
                UsedTime = token.UsedTime,
                IsDisabled = token.IsDisabled
            };
        }

        public static ApiPermissionGroup ConvertToDto(DomainApiPermissionGroup group)
        {
            return new ApiPermissionGroup()
            {
                Code = group.Code,
                Id = group.Id,
                Name = group.Name,
                Desc = group.Desc,
                HasPermission = group.Permissions != null && group.Permissions.Any()
            };
        }
    }
}



using EasyDDD.Core.Specification;
using EasyDDD.Infrastructure.Crosscutting;
using EasyDDD.Infrastructure.Crosscutting.Caching;
using Portal.Domain.Aggregates.UserAgg;
using Portal.Domain.Aggregates.UserAgg.State;
using Portal.Domain.Model;
using Portal.Domain.Model.Strategies;
using Portal.Domain.Repositories;
using Portal.Domain.Specification.User;
using Portal.Infrastructure.Exceptions;

namespace Portal.Domain.Services.Impl
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerateStrategy _tokenGenerateStrategy;
        public LoginService(IUserRepository userRepository, ITokenGenerateStrategy tokenGenerateStrategy)
        {
            _userRepository = userRepository;
            _tokenGenerateStrategy = tokenGenerateStrategy;
        }

        public UserIdentity Login(string loginName, string password, LoginType type)
        {
            if (string.IsNullOrWhiteSpace(loginName))
            {
                throw new PortalValidateException(ErrorCodes.StringCodes.IdentityCannotBeNull, ErrorMessage.IdentityCannotBeNull);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new PortalValidateException(ErrorCodes.StringCodes.PasswordCannotBeNull, ErrorMessage.PasswordCannotBeNull);
            }
            //支持登录名和员工号登录
            var user = _userRepository.Get(new OrSpecification<User>(new UserLoginNameSpecification(loginName), new EmployeeNoSpecification(loginName)));

            //用户名密码校验
            if (user == null)
            {
                // todo 需要增加一些防暴力破解的手段
                throw new PortalException(ErrorCodes.StringCodes.LoginNameOrPasswordInvalid, ErrorMessage.LoginNameOrPasswordInvalid);
            }
            bool isMatched = type == LoginType.HashPassword ? user.IsMatchedHashPassword(password) : user.IsMatchedPasword(password);
            if (!isMatched)
            {
                throw new PortalException(ErrorCodes.StringCodes.LoginNameOrPasswordInvalid, ErrorMessage.LoginNameOrPasswordInvalid);
            }
            if (!user.IsApproved)
            {
                throw new PortalException(ErrorCodes.StringCodes.LoginNameIsApproved, ErrorMessage.LoginNameIsApproved);
            }

            var token = _tokenGenerateStrategy.Generate(loginName);
            CacheManager.Set(Token.GetCacheTokenKey(token.Value), token, Token.ConfigExpired);

            return new UserIdentity(loginName, user.DisplayName, token.Value, user.UserType);
        }
    }
}

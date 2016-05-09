using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Dto;

namespace Portal.Applications.Services
{
    /// <summary>
    /// 授权码管理服务
    /// </summary>
    public interface IAuthorizationCodeManagerService
    {
        /// <summary>
        /// 表示分页查询授权码
        /// </summary>
        FindAuthorizationCodesResponse FindTokens(FindAuthorizationCodesRequest request);
    }
}

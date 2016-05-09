using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Portal.Domain.Aggregates.UserAgg;
using EasyDDD.Core.Aggregate;

namespace Portal.Domain.Aggregates.ApiUserAgg
{
    public class ApiUser : AggregateRoot
    {

        public string UserId { get; private set; }

        /// <summary>
        /// 绑定用户账户
        /// </summary>
        /// <param name="userId"></param>
        public void BindUserAccount(string userId)
        {
            //todo 领域事件验证userId的存在性
        }


    }
}

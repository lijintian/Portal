using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Services.Impl.Validator
{   
    /// <summary>
    /// 表示一个验证
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// 执行验证动作
        /// </summary>
        void Validate();
    }
}

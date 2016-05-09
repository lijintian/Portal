using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Services.Impl.Validator
{
    /// <summary>
    /// 表示验证基类
    /// </summary>
    public abstract class Validator : IValidator
    {
        /// <summary>
        /// 验证动作
        /// </summary>
        public abstract void Validate();

        /// <summary>
        /// 两个验证组合成一个新的验证器
        /// </summary>
        public IValidator And(IValidator other)
        {
            if (other == null)
            {
                throw new ArgumentException("other");
            }
            return new AndValidator(this, other);
        }
    }
}

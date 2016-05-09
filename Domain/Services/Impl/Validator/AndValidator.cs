using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Domain.Services.Impl.Validator
{
    /// <summary>
    /// 表示组合验证器
    /// </summary>
    class AndValidator : IValidator
    {
        private readonly IValidator _left;
        private readonly IValidator _right;
        public AndValidator(IValidator left, IValidator right)
        {
            this._left = left;
            this._right = right;
        }

        public void Validate()
        {
            this._left.Validate();
            this._right.Validate();
        }
    }
}

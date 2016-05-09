using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Portal.WebApi
{
    /// <summary>
    /// 表示错误通知
    /// </summary>
    public static class ErrorNotify
    {
        private static Action<Exception> _notify;

        public static void SetNotify(Action<Exception> notify)
        {
            _notify = notify;
        }

        public static void Notify(Exception ex)
        {
            if (_notify != null)
            {
                _notify(ex);
            }
        }
    }
}

using System;

namespace Portal.Web.Core.Common
{
    public static class DateTimeUtility
    {
        public static string Show(this DateTime owen, string format = null, int tzo = 0)
        {
            if (owen < WebConst.Mindatetime)
                return "";
            var time = owen.AddHours(tzo);
            if (string.IsNullOrWhiteSpace(format))
            {
                return time.ToString();
            }
            return time.ToString(format);
        }
    }
}

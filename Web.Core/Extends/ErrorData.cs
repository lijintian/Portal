using System;

namespace Portal.Web.Core.Extends
{
    public class ErrorData
    {
        public string TicketId { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public Uri RequestUri { get; set; }
        public string ErrorCode { get; set; }
        public string RequestIp { get; set; }
    }
}
